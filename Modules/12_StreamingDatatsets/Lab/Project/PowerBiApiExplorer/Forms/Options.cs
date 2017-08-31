using System;
using System.Configuration;
using System.Windows.Forms;

namespace PowerBiApiExplorer.Forms
{
    public partial class Options : Form
    {
        private Main mainForm;

        public Options()
        {
            InitializeComponent();
        }

        private void Options_Load(object sender, EventArgs e)
        {
            mainForm = (Main)this.Owner;

            // Retrieve session values
            txtClientId.Text = PowerBI.clientId;
            txtRedirectUri.Text = PowerBI.redirectUri;

            // Retrieve fixed Uri values from App.config
            txtResourceUri.Text = ConfigurationManager.AppSettings["ResourceUri"];
            txtAuthorityUri.Text = ConfigurationManager.AppSettings["AuthorityUri"];
            txtPowerBiApiUri.Text = ConfigurationManager.AppSettings["PowerBiApiUri"];

            // Set help tooltip text
            ToolTip tt = new ToolTip();
            tt.SetToolTip(pbxClientId, "Registered Client ID must be retrieved from the Azure Active Directory application registration");
            tt.SetToolTip(pbxRedirectUri, "Registered Redirect URI must be retrieved from the Azure Active Directory application registration");
            tt.SetToolTip(pbxResourceUri, "Power BI resource URI is a fixed URI to connect to the Power BI service");
            tt.SetToolTip(pbxAuthorityUri, "OAuth2 authority URI is a fixed URI to authenticate the Azure Active Directory user");
            tt.SetToolTip(pbxPowerBiApiUri, "Power BI API URI is a fixed URI to automate Power BI content");
        }

        private void Options_Activated(object sender, EventArgs e)
        {
            // Disable controls if application is connected
            txtClientId.Enabled = !mainForm.isConnected;
            txtRedirectUri.Enabled = !mainForm.isConnected;
            btnOK.Enabled = !mainForm.isConnected;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            // Persist updateable setting values
            PowerBI.clientId = txtClientId.Text.Trim();
            PowerBI.redirectUri = txtRedirectUri.Text.Trim();

            this.btnClose_Click(null, null);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
