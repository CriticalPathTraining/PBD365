using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;

namespace PowerBiApiExplorer.Forms
{
    public partial class About : Form
    {
        Main mainForm;

        public About()
        {
            InitializeComponent();
        }

        private void About_Load(object sender, EventArgs e)
        {
            mainForm = (Main)this.Owner;

            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);

            txtName.Text = ((AssemblyTitleAttribute)assembly.GetCustomAttributes(typeof(AssemblyTitleAttribute), false)[0]).Title;
            lblVersion.Text = String.Format("Version {0}", fvi.FileVersion);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
