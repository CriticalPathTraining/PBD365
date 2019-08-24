using System;
using System.Configuration;
using System.Threading.Tasks;
using Microsoft.Identity.Client;
using Microsoft.PowerBI.Api.V2;
using Microsoft.PowerBI.Api.V2.Models;
using Microsoft.Rest;

namespace AppOwnsDataApp.Models {

  public class PbiEmbeddingManager {

    private static readonly string clientId = ConfigurationManager.AppSettings["client-id"];
    private static readonly string clientSecret = ConfigurationManager.AppSettings["client-secret"];
    private static readonly string tenantName = ConfigurationManager.AppSettings["tenant-name"];

    private static readonly string workspaceId = ConfigurationManager.AppSettings["app-workspace-id"];
    private static readonly string datasetId = ConfigurationManager.AppSettings["dataset-id"];
    private static readonly string reportId = ConfigurationManager.AppSettings["report-id"];
    private static readonly string dashboardId = ConfigurationManager.AppSettings["dashboard-id"];

    // endpoint for tenant-specific authority 
    private static readonly string tenantAuthority = "https://login.microsoftonline.com/" + tenantName;

    // Power BI Service API Root URL
    const string urlPowerBiRestApiRoot = "https://api.powerbi.com/";

    static string GetAppOnlyAccessToken() {

      var appConfidential = ConfidentialClientApplicationBuilder.Create(clientId)
                              .WithClientSecret(clientSecret)
                              .WithAuthority(tenantAuthority)
                              .Build();

      string[] scopesDefault = new string[] { "https://analysis.windows.net/powerbi/api/.default" };
      var authResult = appConfidential.AcquireTokenForClient(scopesDefault).ExecuteAsync().Result;
      return authResult.AccessToken;
    }

    private static PowerBIClient GetPowerBiClient() {
      var tokenCredentials = new TokenCredentials(GetAppOnlyAccessToken(), "Bearer");
      return new PowerBIClient(new Uri(urlPowerBiRestApiRoot), tokenCredentials);
    }

    public static async Task<ReportEmbeddingData> GetReportEmbeddingData() {

      PowerBIClient pbiClient = GetPowerBiClient();

      var report = await pbiClient.Reports.GetReportInGroupAsync(workspaceId, reportId);
      var embedUrl = report.EmbedUrl;
      var reportName = report.Name;

      GenerateTokenRequest generateTokenRequestParameters = new GenerateTokenRequest(accessLevel: "edit");
      string embedToken =
            (await pbiClient.Reports.GenerateTokenInGroupAsync(workspaceId,
                                                               report.Id,
                                                               generateTokenRequestParameters)).Token;

      return new ReportEmbeddingData {
        reportId = reportId,
        reportName = reportName,
        embedUrl = embedUrl,
        accessToken = embedToken
      };

    }



  }

}
