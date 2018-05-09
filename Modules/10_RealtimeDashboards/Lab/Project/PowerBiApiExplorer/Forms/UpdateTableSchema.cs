using System;
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
    public partial class UpdateTableSchema : Form
    {
        private Main mainForm;

        public UpdateTableSchema()
        {
            InitializeComponent();
        }

        private void UpdateTableSchema_Load(object sender, EventArgs e)
        {
            mainForm = (Main)this.Owner;

            // Set textbox tab spacing
            txtJsonContent.SelectionTabs = new int[] { 20, 40, 60, 80, 100, 120, 140, 160, 180, 200 };
        }

        private void txtJsonContent_TextChanged(object sender, EventArgs e)
        {
            btnOK.Enabled = (txtJsonContent.Text.Length > 0);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (txtJsonContent.Text.Length == 0)
            {
                MessageBox.Show("You must enter the New Table Schema JSON document.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            this.Cursor = Cursors.WaitCursor;

            if (PowerBI.UpdateTableSchema(mainForm.selectedGroup, mainForm.selectedDataset, mainForm.selectedTable, txtJsonContent.Text))
            {
                btnClose_Click(null, null);
            }
            else
            {
                MessageBox.Show("Table schema could not be updated.\r\nEnsure that the JSON document is valid.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            this.Cursor = Cursors.Default;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}