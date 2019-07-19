using Microsoft.Identity.Client;
using Microsoft.PowerBI.Api.V2;
using Microsoft.Rest;
using System;
using System.Diagnostics;
using System.Security;
using System.Threading.Tasks;

class Program {

  // update the following four constants with the values from your envirionment
  const string appWorkspaceId = "";
  const string clientId = "";
  const string tenantName = "MY_TENANT.onMicrosoft.com";
  const string redirectUri = "https://localhost/app1234";

  // generic v2 endpoint references "organizations" instead of "common"
  const string tenantCommonAuthority = "https://login.microsoftonline.com/organizations";
  const string tenantSpecificAuthority = "https://login.microsoftonline.com/" + tenantName;

  // Power BI Service API Root URL  
  const string urlPowerBiRestApiRoot = "https://api.powerbi.com/";

  static string[] scopesDefault = new string[] {
        "https://analysis.windows.net/powerbi/api/.default"
  };

  static string[] scopesReadWorkspaceAssets = new string[] {
        "https://analysis.windows.net/powerbi/api/Dashboard.Read.All",
        "https://analysis.windows.net/powerbi/api/Dataset.Read.All",
        "https://analysis.windows.net/powerbi/api/Report.Read.All"
  };

  static string[] scopesReadUserApps = new string[] {
        "https://analysis.windows.net/powerbi/api/App.Read.All"
  };

  static string[] scopesManageWorkspaceAssets = new string[] {
        "https://analysis.windows.net/powerbi/api/Content.Create",
        "https://analysis.windows.net/powerbi/api/Dashboard.ReadWrite.All",
        "https://analysis.windows.net/powerbi/api/Dataset.ReadWrite.All",
        "https://analysis.windows.net/powerbi/api/Group.Read.All",
        "https://analysis.windows.net/powerbi/api/Report.ReadWrite.All",
        "https://analysis.windows.net/powerbi/api/Workspace.ReadWrite.All"
  };

  static string[] scopesKitchenSink = new string[] {
        "https://analysis.windows.net/powerbi/api/Tenant.ReadWrite.All", // requires admin
        "https://analysis.windows.net/powerbi/api/App.Read.All",
        "https://analysis.windows.net/powerbi/api/Capacity.ReadWrite.All",
        "https://analysis.windows.net/powerbi/api/Content.Create",
        "https://analysis.windows.net/powerbi/api/Dashboard.Read.All",
        "https://analysis.windows.net/powerbi/api/Dashboard.ReadWrite.All",
        "https://analysis.windows.net/powerbi/api/Data.Alter_Any",
        "https://analysis.windows.net/powerbi/api/Dataflow.ReadWrite.All",
        "https://analysis.windows.net/powerbi/api/Dataset.ReadWrite.All",
        "https://analysis.windows.net/powerbi/api/Gateway.ReadWrite.All",
        "https://analysis.windows.net/powerbi/api/Group.Read.All",
        "https://analysis.windows.net/powerbi/api/Metadata.View_Any",
        "https://analysis.windows.net/powerbi/api/Report.ReadWrite.All",
        "https://analysis.windows.net/powerbi/api/StorageAccount.ReadWrite.All",
        "https://analysis.windows.net/powerbi/api/Workspace.ReadWrite.All"
  };

  static string GetAccessTokenInteractive(string[] scopes) {

    PublicClientApplicationOptions options = new PublicClientApplicationOptions();

    var appPublic = PublicClientApplicationBuilder.Create(clientId)
                     .WithAuthority(tenantCommonAuthority)
                     .WithRedirectUri(redirectUri)
                     .Build();

    var authResult = appPublic.AcquireTokenInteractive(scopes)
                              .WithPrompt(Prompt.SelectAccount)
                              .ExecuteAsync().Result;

    return authResult.AccessToken;
  }

  static string GetAccessTokenWithUserPassword(string[] scopes) {

    var appPublic = PublicClientApplicationBuilder.Create(clientId)
                      .WithAuthority(tenantCommonAuthority)
                      .Build();

    string username = "user1@MY_TENANT.onMicrosoft.com";
    string userPassword = "";
    SecureString userPasswordSecure = new SecureString();
    foreach (char c in userPassword) {
      userPasswordSecure.AppendChar(c);
    }

    var authResult = appPublic.AcquireTokenByUsernamePassword(scopes, username, userPasswordSecure)
                              .ExecuteAsync().Result;

    return authResult.AccessToken;
  }

static string GetAccessTokenWithDeviceCode(string[] scopes) {

  // device code authentication requires tenant-specific authority URL
  var appPublic = PublicClientApplicationBuilder.Create(clientId)
                    .WithAuthority(tenantSpecificAuthority)
                    .Build();

  // this method call will block until you have logged in using the generated device code
  var authResult = appPublic.AcquireTokenWithDeviceCode(scopes, deviceCodeCallbackParams => {
    // retrieve device code and verification URL from deviceCodeCallbackParams 
    string deviceCode = deviceCodeCallbackParams.UserCode;
    string verificationUrl = deviceCodeCallbackParams.VerificationUrl;
    Console.WriteLine();
    Console.WriteLine("When prompted by the browser, copy-and-paste the following device code: " + deviceCode);
    Console.WriteLine();
    Console.WriteLine("Opening Browser at " + verificationUrl);
    Process.Start("chrome.exe", verificationUrl);
    Console.WriteLine();
    Console.WriteLine("This console app will now block until you enter the device code and log in");
    // return task result
    return Task.FromResult(0);
  }).ExecuteAsync().Result;

  Console.WriteLine();
  Console.WriteLine("The call to AcquireTokenWithDeviceCode has completed and returned an access token");
  Console.WriteLine();

  return authResult.AccessToken;

}


  static void Main() {

    DisplayAppWorkspaceAssets();

    // DisplayAllWorkspacesInTenant();
  }

static void DisplayAppWorkspaceAssets() {

  string AccessToken = GetAccessTokenWithDeviceCode(scopesReadWorkspaceAssets);
  var pbiClient = new PowerBIClient(new Uri(urlPowerBiRestApiRoot),
                                          new TokenCredentials(AccessToken, "Bearer"));

    Console.WriteLine();
    Console.WriteLine("Dashboards:");
    var dashboards = pbiClient.Dashboards.GetDashboardsInGroup(appWorkspaceId).Value;
    foreach (var dashboard in dashboards) {
      Console.WriteLine(" - " + dashboard.DisplayName + " [" + dashboard.Id + "]");
    }

    Console.WriteLine();
    Console.WriteLine("Reports:");
    var reports = pbiClient.Reports.GetReportsInGroup(appWorkspaceId).Value;
    foreach (var report in reports) {
      Console.WriteLine(" - " + report.Name + " [" + report.Id + "]");
    }

    Console.WriteLine();
    Console.WriteLine("Datasets:");
    var datasets = pbiClient.Datasets.GetDatasetsInGroup(appWorkspaceId).Value;
    foreach (var dataset in datasets) {
      Console.WriteLine(" - " + dataset.Name + " [" + dataset.Id + "]");
    }

    Console.WriteLine();
  }

  static void DisplayAllWorkspacesInTenant() {

    string AccessToken = GetAccessTokenInteractive(scopesKitchenSink);
    var pbiClient = new PowerBIClient(new Uri(urlPowerBiRestApiRoot),
                                           new TokenCredentials(AccessToken, "Bearer"));

    Console.WriteLine();
    Console.WriteLine("All Workpaces in Tenant:");
    var workspaces = pbiClient.Groups.GetGroupsAsAdmin(top: 100).Value;
    foreach (var workspace in workspaces) {
      Console.WriteLine("- " + workspace.Type + ": " + workspace.Name + " [" + workspace.Id + "] ");
    }
    Console.WriteLine();
  }

}