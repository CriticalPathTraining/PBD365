using System;
using System.Configuration;
using System.Threading.Tasks;
using Microsoft.Rest;
using Microsoft.PowerBI.Api.V2;
using Microsoft.PowerBI.Api.V2.Models;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Collections.Generic;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Web;
using System.Security.Cryptography.X509Certificates;

namespace ThirdPartyEmbeddingDemo.Models {

  public class PbiEmbeddedManager {

    private static string resourceUriPowerBi = "https://analysis.windows.net/powerbi/api";
    private static string urlPowerBiRestApiRoot = "https://api.powerbi.com/";

    const string aadRootAuthorizationEndpoint = "https://login.windows.net/";
    static readonly string tenantId = ConfigurationManager.AppSettings["tenant-id"];
    static readonly string aadTenantAuthorizationEndpoint = aadRootAuthorizationEndpoint +
                                                            tenantId + "/";


    private static string applicationId = ConfigurationManager.AppSettings["application-id"];
    private static string applicationSecret = ConfigurationManager.AppSettings["application-secret"];

    private static string workspaceId = ConfigurationManager.AppSettings["app-workspace-id"];
    private static string reportId = ConfigurationManager.AppSettings["report-id"];

    static string GetAccessToken() {
      var authContext = new AuthenticationContext(aadTenantAuthorizationEndpoint);
      var clientCredential = new ClientCredential(applicationId, applicationSecret);
      return authContext.AcquireTokenAsync(resourceUriPowerBi, clientCredential).Result.AccessToken;
    }    


    static string GetAccessTokenUsingKey() {

      string PfxFilePassword = "pass@word1";
      string PfxFilePath = HttpContext.Current.Server.MapPath("~/Certificates/AppCert1.pfx");
      X509Certificate2 privateKeyCert = new X509Certificate2(PfxFilePath, PfxFilePassword);
      ClientAssertionCertificate cert = new ClientAssertionCertificate(applicationId, privateKeyCert);

      var authContext = new AuthenticationContext(aadTenantAuthorizationEndpoint);
      return authContext.AcquireTokenAsync(resourceUriPowerBi, cert).Result.AccessToken;

    }

    private static PowerBIClient GetPowerBiClient() {
      var tokenCredentials = new TokenCredentials(GetAccessToken(), "Bearer");
      return new PowerBIClient(new Uri(urlPowerBiRestApiRoot), tokenCredentials);
    }

    public static async Task<ReportEmbeddingData> GetReportEmbeddingData() {

      string currentUserName = HttpContext.Current.User.Identity.GetUserName();
      ApplicationDbContext context = new ApplicationDbContext();
      var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
      ApplicationUser currentUser = userManager.FindByName(currentUserName);

      var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

      List<string> roles = new List<string>();

      foreach (var role in currentUser.Roles) {
        roles.Add(roleManager.FindById(role.RoleId).Name);
      }

      string accessLevel = HttpContext.Current.User.IsInRole("Admin") ? "edit" : "view";

      PowerBIClient pbiClient = GetPowerBiClient();

      var report = await pbiClient.Reports.GetReportInGroupAsync(workspaceId, reportId);
      var embedUrl = report.EmbedUrl;
      var reportName = report.Name;

      GenerateTokenRequest generateTokenRequestParameters = new GenerateTokenRequest(accessLevel: accessLevel);
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