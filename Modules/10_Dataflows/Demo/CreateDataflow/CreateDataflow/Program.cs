using Microsoft.Identity.Client;
using Microsoft.PowerBI.Api;
using Microsoft.PowerBI.Api.Models;
using Microsoft.Rest;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace CreateDataflow {
  
  class Program {

    // update the following four constants with the values from your envirionment
    public static readonly Guid appWorkspaceId = new Guid("");
    const string clientId = "";
    const string tenantName = "";
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
        "https://analysis.windows.net/powerbi/api/Dataflow.Read.All",
        "https://analysis.windows.net/powerbi/api/Dataset.Read.All",
        "https://analysis.windows.net/powerbi/api/Report.Read.All"
  };

    static string[] scopesManageWorkspaceAssets = new string[] {
        "https://analysis.windows.net/powerbi/api/Content.Create",
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

      string username = "tedp@pbdbc2020.onMicrosoft.com";
      string userPassword = "Pa$$word!";
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

     // CreateDataflow(appWorkspaceId, @"c:\temp\model.json");

      DisplayAppWorkspaceAssets();

    }



    static void CreateDataflow(Guid appWorkspaceId, string modelJsonPath) {

      string AccessToken = GetAccessTokenWithUserPassword(scopesManageWorkspaceAssets);
      
      var pbiClient = new PowerBIClient(new Uri(urlPowerBiRestApiRoot),
                                        new TokenCredentials(AccessToken, "Bearer"));

      MemoryStream stream = new MemoryStream(Properties.Resources.model_json);

      var import = pbiClient.Imports.PostImportWithFileInGroup(appWorkspaceId, stream, "model.json");

      Console.WriteLine("Dataflow creation completed");

    }



    static void DisplayAppWorkspaceAssets() {

      string AccessToken = GetAccessTokenWithUserPassword(scopesManageWorkspaceAssets);
      var pbiClient = new PowerBIClient(new Uri(urlPowerBiRestApiRoot),
                                        new TokenCredentials(AccessToken, "Bearer"));

      var dataflows = pbiClient.Dataflows.GetDataflows(appWorkspaceId).Value;

      foreach (var dataflow in dataflows) {

        Console.WriteLine(" - " + dataflow.Name + " [" + dataflow.ObjectId + "]");

        RefreshRequest refreshRequest = new RefreshRequest(NotifyOption.MailOnCompletion);
        
        pbiClient.Dataflows.RefreshDataflowAsync(appWorkspaceId, dataflow.ObjectId, refreshRequest);
      

      }

      Console.ReadLine();

    }
  }
}
