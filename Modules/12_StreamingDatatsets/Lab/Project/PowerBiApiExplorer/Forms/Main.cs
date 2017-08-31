using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PowerBiApiExplorer.Forms
{
    public partial class Main : Form
    {
        internal bool isConnected = false;

        internal TreeNode selectedNode = null;
        internal Group selectedGroup = null;
        internal Dataset selectedDataset = null;
        internal Table selectedTable = null;
        internal Dashboard selectedDashboard = null;
        internal Tile selectedTile = null;

        public Main()
        {
            InitializeComponent();

            // Redirect all Console output to txtConsole
            this.SetOutToTextBox = txtConsole;
            Console.WriteLine("> Started");
        }

        internal TextBox SetOutToTextBox
        {
            set
            {
                Console.SetOut(new TextBoxStreamWriter(value));
            }
        }

        private void mnuConnect_Click(object sender, EventArgs e)
        {
            if ((PowerBI.clientId == String.Empty) || (PowerBI.redirectUri == String.Empty))
            {
                MessageBox.Show("You must use Options to enter the Client ID and Redirect URI.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            this.Cursor = Cursors.WaitCursor;

            string token = PowerBI.GetAccessToken();

            if (token != null)
            {
                isConnected = true;

                mnuConnect.Enabled = false;
                tslConnection.Text = "Connected";
                tslConnection.ForeColor = Color.Green;

                TreeNode nodePowerBIService = new TreeNode("Power BI Service");
                nodePowerBIService.ImageIndex = nodePowerBIService.SelectedImageIndex = 0;
                nodePowerBIService.ContextMenuStrip = cmsService;
                treeView1.Nodes.Add(nodePowerBIService);

                TreeNode nodeUserWorkspace = new TreeNode("User Workspace");
                nodeUserWorkspace.ImageIndex = nodeUserWorkspace.SelectedImageIndex = 5;
                nodePowerBIService.Nodes.Add(nodeUserWorkspace);

                TreeNode nodeDatasets = new TreeNode("Datasets");
                nodeDatasets.ImageIndex = nodeDatasets.SelectedImageIndex = 1;
                nodeDatasets.ContextMenuStrip = cmsDatasets;
                nodeUserWorkspace.Nodes.Add(nodeDatasets);

                TreeNode nodeDashboards = new TreeNode("Dashboards");
                nodeDashboards.ImageIndex = nodeDashboards.SelectedImageIndex = 1;
                nodeDashboards.ContextMenuStrip = cmsDashboards;
                nodeUserWorkspace.Nodes.Add(nodeDashboards);

                TreeNode nodeGroups = new TreeNode("App Workspaces");
                nodeGroups.ImageIndex = nodeGroups.SelectedImageIndex = 1;
                nodeGroups.ContextMenuStrip = cmsGroups;
                nodePowerBIService.Nodes.Add(nodeGroups);

                nodePowerBIService.Expand();
            }

            Console.WriteLine("> ");

            this.Cursor = Cursors.Default;
        }

        private void mnuOptions_Click(object sender, EventArgs e)
        {
            Options optionsForm = new Options();
            this.AddOwnedForm(optionsForm);
            optionsForm.StartPosition = FormStartPosition.CenterParent;

            optionsForm.ShowDialog();
        }

        private void mnuAbout_Click(object sender, EventArgs e)
        {
            About aboutForm = new About();
            this.AddOwnedForm(aboutForm);
            aboutForm.StartPosition = FormStartPosition.CenterParent;

            aboutForm.ShowDialog();
        }

        private void mnuClearConsole_Click(object sender, EventArgs e)
        {
            txtConsole.Text = String.Empty;
        }

        private void mnuExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Keep state of selected objects in the Power BI hierarchy
        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                selectedNode = e.Node;

                selectedGroup = null;
                selectedDataset = null;
                selectedTable = null;

                IList<TreeNode> ancestorList = TreeViewHelper.GetAncestors(selectedNode, x => x.Parent).ToList();

                foreach (TreeNode ancestorNode in ancestorList)
                {
                    if (ancestorNode.Tag is Group)
                    {
                        selectedGroup = ((Group)ancestorNode.Tag);
                    }
                    else if (ancestorNode.Tag is Dataset)
                    {
                        selectedDataset = ((Dataset)ancestorNode.Tag);
                    }
                    else if (ancestorNode.Tag is Table)
                    {
                        selectedTable = ((Table)ancestorNode.Tag);
                    }
                    else if (ancestorNode.Tag is Dashboard)
                    {
                        selectedDashboard = ((Dashboard)ancestorNode.Tag);
                    }
                    else if (ancestorNode.Tag is Tile)
                    {
                        selectedTile = ((Tile)ancestorNode.Tag);
                    }
                }
            }
        }

        private void cmnuGetGroups_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            TreeNode nodeGroups = selectedNode;

            Group[] result = PowerBI.GetGroups();

            if (result != null)
            {
                nodeGroups.Nodes.Clear();

                TreeNode nodeGroup;

                foreach (Group group in result)
                {
                    nodeGroup = new TreeNode(group.name);
                    nodeGroup.ImageIndex = nodeGroup.SelectedImageIndex = 4;
                    nodeGroup.Tag = group;
                    nodeGroup.ToolTipText = String.Format("APP WORKSPACE\r\nid: {0}\r\nname: {1}", group.id, group.name);
                    nodeGroups.Nodes.Add(nodeGroup);

                    TreeNode nodeDatasets = new TreeNode("Datasets");
                    nodeDatasets.ImageIndex = nodeDatasets.SelectedImageIndex = 1;
                    nodeDatasets.ContextMenuStrip = cmsDatasets;
                    nodeGroup.Nodes.Add(nodeDatasets);

                    TreeNode nodeDashboards = new TreeNode("Dashboards");
                    nodeDashboards.ImageIndex = nodeDashboards.SelectedImageIndex = 1;
                    nodeDashboards.ContextMenuStrip = cmsDashboards;
                    nodeGroup.Nodes.Add(nodeDashboards);
                }

                nodeGroups.Expand();
            }

            this.Cursor = Cursors.Default;
        }

        private void cmnuDisconnect_Click(object sender, EventArgs e)
        {
            PowerBI.Disconnect();

            isConnected = false;

            mnuConnect.Enabled = true;

            treeView1.Nodes.Clear();

            tslConnection.Text = "Not Connected";
            tslConnection.ForeColor = Color.Black;
        }

        private void cmnuCreateDataset_Click(object sender, EventArgs e)
        {
            CreateDataset createDatasetForm = new CreateDataset();
            this.AddOwnedForm(createDatasetForm);
            createDatasetForm.nodeDatasets = selectedNode;
            createDatasetForm.StartPosition = FormStartPosition.CenterParent;

            createDatasetForm.ShowDialog();
        }

        private void cmnuGetDatasets_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            TreeNode nodeDatasets = selectedNode;

            Dataset[] result = PowerBI.GetDatasets(selectedGroup);

            if (result != null)
            {
                nodeDatasets.Nodes.Clear();

                TreeNode nodeDataset;
                TreeNode nodeTables;

                foreach (Dataset dataset in result)
                {
                    nodeDataset = new TreeNode(dataset.name);
                    nodeDataset.ImageIndex = nodeDataset.SelectedImageIndex = 2;
                    nodeDataset.Tag = dataset;
                    nodeDataset.ToolTipText = String.Format("DATASET\r\nid: {0}\r\nname: {1}", dataset.id, dataset.name);
                    nodeDatasets.Nodes.Add(nodeDataset);

                    nodeTables = new TreeNode("Tables");
                    nodeTables.ImageIndex = nodeTables.SelectedImageIndex = 1;
                    nodeTables.ContextMenuStrip = cmsTables;
                    nodeDataset.Nodes.Add(nodeTables);
                }

                nodeDatasets.Expand();
            }

            this.Cursor = Cursors.Default;
        }

        private void cmnuGetTables_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            TreeNode nodeTables = selectedNode;

            Table[] result = PowerBI.GetTables(selectedGroup, selectedDataset);

            if (result != null)
            {
                nodeTables.Nodes.Clear();

                TreeNode nodeTable;

                foreach (Table table in result)
                {
                    nodeTable = new TreeNode(table.name);
                    nodeTable.ImageIndex = nodeTable.SelectedImageIndex = 3;
                    nodeTable.Tag = table;
                    nodeTable.ToolTipText = String.Format("TABLE\r\nname: {0}", table.name);
                    nodeTable.ContextMenuStrip = cmsTable;
                    nodeTables.Nodes.Add(nodeTable);
                }

                nodeTables.Expand();
            }

            this.Cursor = Cursors.Default;
        }

        private void cmnuUpdateSchema_Click(object sender, EventArgs e)
        {
            UpdateTableSchema updateTableSchemaForm = new UpdateTableSchema();
            this.AddOwnedForm(updateTableSchemaForm);
            updateTableSchemaForm.StartPosition = FormStartPosition.CenterParent;

            updateTableSchemaForm.ShowDialog();
        }

        private void cmnuPushData_JSON_Click(object sender, EventArgs e)
        {
            PushData_JSON pushDataJSONForm = new PushData_JSON();
            this.AddOwnedForm(pushDataJSONForm);
            pushDataJSONForm.StartPosition = FormStartPosition.CenterParent;

            pushDataJSONForm.ShowDialog();
        }

        private void cmnuPushData_EventStream_Click(object sender, EventArgs e)
        {
            PushData_EventStream pushDataEventStreamForm = new PushData_EventStream();
            this.AddOwnedForm(pushDataEventStreamForm);
            pushDataEventStreamForm.StartPosition = FormStartPosition.CenterParent;

            pushDataEventStreamForm.ShowDialog();
        }

        private void cmnuClearAllRows_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            TreeNode nodeTable = selectedNode;
            string datasetId = ((Dataset)nodeTable.Parent.Parent.Tag).id;
            string tableName = nodeTable.Text;

            PowerBI.ClearTableRows(selectedGroup, selectedDataset, selectedTable);

            this.Cursor = Cursors.Default;
        }

        private void cmnuGetDashboards_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            TreeNode nodeDashboards = selectedNode;

            Dashboard[] result = PowerBI.GetDashboards(selectedGroup);

            if (result != null)
            {
                nodeDashboards.Nodes.Clear();

                TreeNode nodeDashboard;
                TreeNode nodeTiles;

                foreach (Dashboard dashboard in result)
                {
                    nodeDashboard = new TreeNode(dashboard.displayName);
                    nodeDashboard.ImageIndex = nodeDashboard.SelectedImageIndex = 6;
                    nodeDashboard.Tag = dashboard;
                    nodeDashboard.ToolTipText = String.Format("DASHBOARD\r\nid: {0}\r\ndisplayName: {1}\r\nisReadOnly: {2}", dashboard.id, dashboard.displayName, dashboard.isReadOnly);
                    nodeDashboards.Nodes.Add(nodeDashboard);

                    nodeTiles = new TreeNode("Tiles");
                    nodeTiles.ImageIndex = nodeTiles.SelectedImageIndex = 1;
                    nodeTiles.ContextMenuStrip = cmsTiles;
                    nodeDashboard.Nodes.Add(nodeTiles);
                }

                nodeDashboards.Expand();
            }

            this.Cursor = Cursors.Default;
        }

        private void cmnuGetTiles_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            TreeNode nodeTiles = selectedNode;

            Tile[] result = PowerBI.GetTiles(selectedGroup, selectedDashboard);

            if (result != null)
            {
                nodeTiles.Nodes.Clear();

                TreeNode nodeTile;

                foreach (Tile tile in result)
                {
                    nodeTile = new TreeNode((string.IsNullOrEmpty(tile.title)) ? "[No title]" : tile.title);
                    nodeTile.ImageIndex = nodeTile.SelectedImageIndex = 7;
                    nodeTile.Tag = tile;
                    nodeTile.ToolTipText = String.Format("TILE\r\nid: {0}\r\ntitle: {1}\r\nsubTitle: {2}\r\nembedUrl: {3}", tile.id, tile.title, tile.subTitle, tile.embedUrl);
                    nodeTile.ContextMenuStrip = cmsTile;
                    nodeTiles.Nodes.Add(nodeTile);
                }

                nodeTiles.Expand();
            }

            this.Cursor = Cursors.Default;
        }

        private void cmnuCopyEmbedUrl_Click(object sender, EventArgs e)
        {
            TreeNode nodeTile = selectedNode;

            Clipboard.SetText(((Tile)nodeTile.Tag).embedUrl);
        }
    }

    #region Console Redirection

    public class TextBoxStreamWriter : System.IO.TextWriter
    {
        TextBox _output = null;

        public TextBoxStreamWriter(TextBox output)
        {
            _output = output;
        }

        public override void Write(char value)
        {
            base.Write(value);

            try
            {
                if (_output != null)
                {
                    _output.AppendText(value.ToString());
                }
            }
            catch { }
        }

        public override Encoding Encoding
        {
            get { return System.Text.Encoding.UTF8; }
        }
    } 
    #endregion

    #region TreeView Helper

    public static class TreeViewHelper
    {
        public static IEnumerable<TItem> GetAncestors<TItem>(TItem item, Func<TItem, TItem> getParentFunc)
        {
            if (ReferenceEquals(item, null)) yield break;

            for (TItem curItem = item; !ReferenceEquals(curItem, null); curItem = getParentFunc(curItem))
            {
                yield return curItem;
            }
        }
    }
    #endregion
}