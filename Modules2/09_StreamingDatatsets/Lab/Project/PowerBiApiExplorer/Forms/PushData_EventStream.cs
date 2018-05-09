using System;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace PowerBiApiExplorer.Forms
{
    public partial class PushData_EventStream : Form
    {
        private Main mainForm;

        private SqlConnection cnn;
        private DataTable eventsTable = null;

        private int columnCount = 0;
        private int rowCount = 0;
        private string rowTemplate = String.Empty;
        
        private int ticks = 0;
        private int requestsSent = 0;
        private AutoResetEvent resetEvent = new AutoResetEvent(false);

        public PushData_EventStream()
        {
            InitializeComponent();
        }

        private void PushData_EventStream_Load(object sender, EventArgs e)
        {
            mainForm = (Main)this.Owner;

            // Cannot output to main form textbox on the worker thread
            mainForm.SetOutToTextBox = null;

            // Populate the Event Source dropdown list
            try
            {
                cnn = new SqlConnection(ConfigurationManager.AppSettings["DbConnection"]);
                SqlCommand cmd = new SqlCommand("SELECT CONCAT(QUOTENAME([s].[name]), N'.', QUOTENAME([p].[name])) AS [DatabaseObject] FROM [sys].[procedures] AS [p] INNER JOIN [sys].[schemas] AS [s] ON [s].[schema_id] = [p].[schema_id] WHERE [p].[object_id] NOT IN (SELECT [object_id] FROM [sys].[parameters]) ORDER BY [DatabaseObject];", cnn);

                cnn.Open();

                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dr.Read())
                {
                    cboEventSource.Items.Add(dr[0]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (cnn != null)
                {
                    if (cnn.State == ConnectionState.Open)
                    {
                        cnn.Close();
                    }
                }
            }
        }

        private void cboEventSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            // Cache the event dataset
            try
            {
                tslStatus.Text = "Caching";
                tslStatus.ForeColor = Color.Black;
                statusStrip1.Refresh();

                cnn.Open();

                SqlCommand cmd = new SqlCommand(cboEventSource.Text, cnn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet("Events");

                da.Fill(ds);

                cnn.Close();

                eventsTable = ds.Tables[0];
                columnCount = eventsTable.Columns.Count;
                rowCount = eventsTable.Rows.Count;
                rowTemplate = GetRowTemplate();
            }
            catch (Exception ex)
            {
                eventsTable = null;
                columnCount = 0;
                rowCount = 0;
                rowTemplate = String.Empty;

                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (cnn != null)
                {
                    if (cnn.State == ConnectionState.Open)
                    {
                        cnn.Close();
                    }
                }
            }

            nudBatchSize.Enabled = nudDelayBetweenRequests.Enabled = btnStart.Enabled = (rowCount > 0);
            btnStart.Enabled = (eventsTable != null);

            tslStatus.Text = "Stopped";
            tslStatus.ForeColor = Color.Black;

            this.Cursor = Cursors.Default;
        }

        private string GetRowTemplate()
        {
            StringBuilder sb = new StringBuilder("{{");
            char doubleQuotes = '"';
            bool requiresDoubleQuotes;
            int i = 0;

            foreach (DataColumn column in eventsTable.Columns)
            {
                requiresDoubleQuotes = ((column.DataType.Name == "String") || (column.DataType.Name == "DateTime"));

                sb.Append(doubleQuotes);
                sb.Append(column.ColumnName);
                sb.Append(doubleQuotes);
                sb.Append(": ");
                if (requiresDoubleQuotes) { sb.Append(doubleQuotes); };
                sb.Append("{");
                sb.Append(i++);
                sb.Append("}");
                if (requiresDoubleQuotes) { sb.Append(doubleQuotes); };
                sb.Append(", ");
            }

            sb.Remove(sb.Length - 2, 2).Append("}}, ");

            return sb.ToString();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (!backgroundWorker1.IsBusy)
            {
                cboEventSource.Enabled = false;
                nudBatchSize.Enabled = false;
                nudDelayBetweenRequests.Enabled = false;
                
                btnStart.Enabled = false;
                btnStop.Enabled = true;

                ticks = 0;
                timer1.Start();
                tslStatus.Text = "Running";
                tslStatus.ForeColor = Color.Green;
                tslTimer.Text = "00:00:00";
                tslBatchesSent.Text = "Batches: 0";
                tslRowsSent.Text = "Rows: 0";

                backgroundWorker1.RunWorkerAsync();
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            backgroundWorker1.CancelAsync();
            resetEvent.WaitOne();
            
            timer1.Stop();
            tslStatus.Text = "Stopped";
            tslStatus.ForeColor = Color.Black;

            cboEventSource.Enabled = true;
            nudBatchSize.Enabled = true;
            nudDelayBetweenRequests.Enabled = true;

            btnStart.Enabled = true;
            btnStop.Enabled = false;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            ticks += 1;

            TimeSpan span = TimeSpan.FromSeconds(ticks);
            tslTimer.Text = String.Format("{0:00}:{1:00}:{2:00}", span.Hours, span.Minutes, span.Seconds);
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            StringBuilder sb;
            int i = 0;
            int j = 0;
            object[] columnValues;
            int currentRowIndex = 0;

            while(true)
            {
                if (worker.CancellationPending == true)
                {
                    e.Cancel = true;

                    break;
                }
                else
                {
                    // Build JSON document to add rows
                    sb = new StringBuilder("{ \"rows\": [");

                    for (i = 0; i < nudBatchSize.Value; i++)
                    {
                        columnValues = new object[columnCount];

                        if (currentRowIndex == rowCount) { currentRowIndex = 0; };

                        for (j = 0; j < columnCount; j++)
                        {
                            columnValues[j] = eventsTable.Rows[currentRowIndex][j];
                        }

                        sb.Append(String.Format(rowTemplate, columnValues));

                        currentRowIndex++;
                    }

                    sb.Remove(sb.Length - 2, 2).Append("]}");

                    if (PowerBI.AddTableRows(mainForm.selectedGroup, mainForm.selectedDataset, mainForm.selectedTable, sb.ToString()))
                    {
                        worker.ReportProgress(requestsSent++);
                    }
                    
                    if (nudDelayBetweenRequests.Value > 0)
                    {
                        Thread.Sleep((int)nudDelayBetweenRequests.Value * 1000);
                    }
                }
            }

            resetEvent.Set();
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            tslBatchesSent.Text = String.Format("Batches: {0:N0}", e.ProgressPercentage);
            tslRowsSent.Text = String.Format("Rows: {0:N0}", (e.ProgressPercentage * nudBatchSize.Value));

            mainForm.txtConsole.AppendText("> Calling AddTableRows()\r\n");
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (backgroundWorker1.IsBusy)
            {
                backgroundWorker1.CancelAsync();
                resetEvent.WaitOne();
            }
            
            mainForm.SetOutToTextBox = mainForm.txtConsole;

            if (requestsSent > 0)
            {
                Console.WriteLine(">");
            }

            this.Close();
        }
    }
}