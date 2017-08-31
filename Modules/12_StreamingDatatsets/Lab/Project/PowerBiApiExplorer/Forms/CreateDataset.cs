using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PowerBiApiExplorer.Forms
{
    public partial class CreateDataset : Form
    {
        private Main mainForm;
        internal TreeNode nodeDatasets;
        private SqlConnection cnn = null;

        public CreateDataset()
        {
            InitializeComponent();
        }

        private void CreateDataset_Load(object sender, EventArgs e)
        {
            mainForm = (Main)this.Owner;

            // Set textbox tab spacing
            txtJsonContent.SelectionTabs = new int[] { 20, 40, 60, 80, 100, 120, 140, 160, 180, 200 };

            // Default to the first dropdown item
            cboDefaultRetentionPolicy.SelectedIndex = 0;

            // Set help tooltip text
            ToolTip tt = new ToolTip();
            tt.SetToolTip(pbxDefaultRetentionPolicy, "\"basicFIFO\" will keep up to 200,000 rows and then start dropping the oldest rows as new rows come in. If no policy is set, rows will be collected up to the limit.");
        }

        private void RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            foreach (Control control in gbxCreateMethod.Controls)
            {
                if (control is RadioButton)
                {
                    RadioButton rbtn = control as RadioButton;

                    if (rbtn.Checked)
                    {
                        if (rbtn.Name == "rbtnDirectEntry")
                        {
                            txtJsonContent.Clear();
                            txtJsonContent.Focus();
                            cboEventSource.Items.Clear();
                            cboEventSource.Enabled = false;
                        }
                        else if (rbtn.Name == "rbtnEventSource")
                        {
                            PopulateEventSource();

                            cboEventSource.Enabled = true;
                            cboEventSource.Focus();
                        }

                        break;
                    }
                }
            }
        }

        private void PopulateEventSource()
        {
            this.Cursor = Cursors.WaitCursor;

            cboEventSource.Items.Clear();
            cboEventSource.SelectedIndex = -1;

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

            this.Cursor = Cursors.Default;
        }

        private void cboEventSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            string errorMessage = String.Empty;

            try
            {
                // Derive dataset and table names from stored procedure name
                string datasetName = cboEventSource.Text.Replace("[", "").Replace(".", "_").Replace("]", "");
                string tableName = datasetName;

                // Build up a tab-formatted JSON document
                StringBuilder sb = new StringBuilder("{\r\n\t\"name\": \"");
                sb.Append(datasetName);
                sb.Append("\",\r\n\t\"tables\": [\r\n\t\t{\r\n\t\t\t\"name\": \"");
                sb.Append(tableName);
                sb.Append("\",\r\n\t\t\t\"columns\": [\r\n");

                SqlCommand cmd = new SqlCommand(cboEventSource.Text, cnn);
                cmd.CommandType = CommandType.StoredProcedure;

                cnn.Open();

                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                string[] columnNames = new string[dr.FieldCount];
                string dataTypeSql = String.Empty;
                string dataTypeEdm = String.Empty;

                for (int i = 0; i < dr.FieldCount; i++)
                {
                    columnNames[i] = dr.GetName(i);

                    if (columnNames[i].Length == 0)
                    {
                        errorMessage += (String.Format("The column at position {0} does not have a name.\r\n", (i + 1)));
                    }
                    
                    dataTypeSql = dr.GetFieldType(i).Name;

                    // Tranlate SQL Server data types to supported EDM data types
                    switch(dataTypeSql)
                    {
                        // These are supported EDM data types
                        case "Boolean":
                        case "DateTime":
                        case "Double":
                        case "Int64":
                        case "String":
                            {
                                dataTypeEdm = dataTypeSql;
                                break;
                            }
                        case "Byte":
                        case "Int16":
                        case "Int32":
                            {
                                dataTypeEdm = "Int64";
                                break;
                            }
                        case "Single":
                        case "Decimal":
                            {
                                dataTypeEdm = "Double";
                                break;
                            }
                        default:
                            {
                                // Unsupported data type
                                dataTypeEdm = String.Empty;
                                break;
                            }
                    }

                    if (dataTypeEdm == String.Empty)
                    {
                        errorMessage += (String.Format("The column at position {0} does not have a valid data type [{1}].\r\n", (i + 1), dataTypeSql));
                    }

                    sb.Append("\t\t\t\t{\r\n\t\t\t\t\t\"name\": \"");
                    sb.Append(columnNames[i]);
                    sb.Append("\",\r\n\t\t\t\t\t\"dataType\": \"");
                    sb.Append(dataTypeEdm);
                    sb.Append("\"\r\n\t\t\t\t},\r\n");
                }

                if (columnNames.Distinct().Count() != columnNames.Length)
                {
                    errorMessage += String.Format("There are duplicate column name values.\r\n");
                }

                cnn.Close();

                sb.Remove(sb.Length - 3, 3).Append("\r\n\t\t\t]\r\n\t\t}\r\n\t]\r\n}");

                txtJsonContent.Text = sb.ToString();
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

            if (errorMessage.Length > 0)
            {
                errorMessage = "The JSON document is invalid for the following reason(s):\r\n\r\n" + errorMessage;

                MessageBox.Show(errorMessage.Remove(errorMessage.Length - 2, 2), "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            this.Cursor = Cursors.Default;
        }

        private void txtJsonContent_TextChanged(object sender, EventArgs e)
        {
            btnOK.Enabled = (txtJsonContent.Text.Length > 0);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            // Create the dataset
            Dataset dataset = PowerBI.CreateDataset(mainForm.selectedGroup, txtJsonContent.Text, cboDefaultRetentionPolicy.Text);

            // If created successfully, update the application treeview
            if (dataset != null)
            {
                // Add the dataset node to main form treeview control
                TreeNode nodeDataset = new TreeNode(dataset.name);
                nodeDataset.ImageIndex = nodeDataset.SelectedImageIndex = 2;
                nodeDataset.Tag = dataset;
                nodeDataset.ToolTipText = String.Format("DATASET\r\nid: {0}\r\nname: {1}", dataset.id, dataset.name);
                nodeDatasets.Nodes.Add(nodeDataset);

                // Add Tables node
                TreeNode nodeTables = new TreeNode("Tables");
                nodeTables.ImageIndex = nodeTables.SelectedImageIndex = 1;
                nodeTables.ContextMenuStrip = mainForm.cmsTables;
                nodeDataset.Nodes.Add(nodeTables);

                nodeDataset.ExpandAll();

                mainForm.treeView1.SelectedNode = nodeDataset;

                btnClose_Click(null, null);
            }
            else
            {
                MessageBox.Show("Dataset could not be created.\r\n\r\nEnsure that the JSON document is valid.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            this.Cursor = Cursors.Default;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}