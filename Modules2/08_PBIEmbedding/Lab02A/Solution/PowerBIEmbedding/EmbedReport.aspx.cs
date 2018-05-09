using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Rest;
using Microsoft.PowerBI.Api.V2;
using Microsoft.PowerBI.Api.V2.Models;

namespace PowerBIEmbedding
{
    public partial class EmbedReport : System.Web.UI.Page
    {
        private static readonly string AuthorityUrl = ConfigurationManager.AppSettings["authorityUrl"];
        private static readonly string ResourceUrl = ConfigurationManager.AppSettings["resourceUrl"];
        private static readonly string ApiUrl = ConfigurationManager.AppSettings["apiUrl"];
        private static readonly string AppWorkspaceId = ConfigurationManager.AppSettings["appWorkspaceId"];
        private static readonly string ClientId = ConfigurationManager.AppSettings["clientId"];

        private static readonly string Username = ConfigurationManager.AppSettings["pbiUsername"];
        private static readonly string Password = ConfigurationManager.AppSettings["pbiPassword"];

        public string embedToken;
        public string embedUrl;
        public string reportId;

        protected void Page_Load(object sender, EventArgs e)
        {
            var credential = new UserPasswordCredential(Username, Password);

            // Authenticate using app settings credentials
            var authenticationContext = new AuthenticationContext(AuthorityUrl);
            var authenticationResult = authenticationContext.AcquireTokenAsync(ResourceUrl, ClientId, credential).Result;

            var tokenCredentials = new TokenCredentials(authenticationResult.AccessToken, "Bearer");

            // Populate the dropdown list
            if (!IsPostBack)
            {
                using (var client = new PowerBIClient(new Uri(ApiUrl), tokenCredentials))
                {
                    // Get a list of reports
                    var reports = client.Reports.GetReportsInGroup(AppWorkspaceId);

                    // Populate dropdown list
                    foreach (Report item in reports.Value)
                    {
                        ddlReport.Items.Add(new ListItem(item.Name, item.Id));
                    }

                    // Select first item
                    ddlReport.SelectedIndex = 0;
                }
            }
            
            // Generate an embed token and populate embed variables
            using (var client = new PowerBIClient(new Uri(ApiUrl), tokenCredentials))
            {
                // Retrieve the selected report
                var report = client.Reports.GetReportInGroup(AppWorkspaceId, ddlReport.SelectedValue);

                // Generate an embed token to view
                var generateTokenRequestParameters = new GenerateTokenRequest(accessLevel: "view");
                var tokenResponse = client.Reports.GenerateTokenInGroup(AppWorkspaceId, report.Id, generateTokenRequestParameters);

                // Populate embed variables (to be passed client-side)
                embedToken = tokenResponse.Token;
                embedUrl = report.EmbedUrl;
                reportId = report.Id;
            }
        }
    }
}