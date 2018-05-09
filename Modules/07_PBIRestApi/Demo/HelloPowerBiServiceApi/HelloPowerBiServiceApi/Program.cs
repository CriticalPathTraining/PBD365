using System;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.PowerBI.Api.V2;
using Microsoft.Rest;

class Program {
  static string aadAuthorizationEndpoint = "https://login.windows.net/common/oauth2/authorize";
  static string resourceUriPowerBi = "https://analysis.windows.net/powerbi/api";
  static string urlPowerBiRestApiRoot = "https://api.powerbi.com/";

  static string clientId = "5b07ebad-92d7-4a2e-a92d-c36382f6c3a8";
  static string rediectUrl = "https://localhost/app1234";

  static string GetAccessToken() {

    // create new authentication context 
    var authenticationContext =
      new AuthenticationContext(aadAuthorizationEndpoint);

    // use authentication context to trigger user sign-in and return access token 
    var userAuthnResult =
      authenticationContext.AcquireTokenAsync(resourceUriPowerBi,
                                              clientId,
                                              new Uri(rediectUrl),
                                              new PlatformParameters(PromptBehavior.Auto)).Result;

    // return access token to caller
    return userAuthnResult.AccessToken;

  }

  static PowerBIClient GetPowerBiClient() {
    var tokenCredentials = new TokenCredentials(GetAccessToken(), "Bearer");
    return new PowerBIClient(new Uri(urlPowerBiRestApiRoot), tokenCredentials);
  }

  static void Main() {
    PowerBIClient pbiClient = GetPowerBiClient();
    var reports = pbiClient.Reports.GetReports().Value;
    foreach (var report in reports) {
      Console.WriteLine(report.Name);
    }
  }
}
