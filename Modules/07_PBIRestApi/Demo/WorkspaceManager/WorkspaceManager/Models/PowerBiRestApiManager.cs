using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace WorkspaceManager.Models {
  public class PowerBiRestApiManager {

    #region "private implemntation details"

    private static string aadInstance = "https://login.microsoftonline.com/";
    private static string resourceUrlPowerBi = "https://analysis.windows.net/powerbi/api";
    private static string urlPowerBiRestApiRoot = "https://api.powerbi.com/";

    private static string clientId = ConfigurationManager.AppSettings["client-id"];
    private static string clientSecret = ConfigurationManager.AppSettings["client-secret"];
    private static string redirectUrl = ConfigurationManager.AppSettings["reply-url"];

    #endregion

    #region "Private Utility Functions"

    private static async Task<string> GetAccessTokenAsync() {

      // determine authorization URL for current tenant
      string tenantID = ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid").Value;
      string tenantAuthority = aadInstance + tenantID;

      // create ADAL cache object
      ApplicationDbContext db = new ApplicationDbContext();
      string signedInUserID = ClaimsPrincipal.Current.FindFirst(ClaimTypes.NameIdentifier).Value;
      ADALTokenCache userTokenCache = new ADALTokenCache(signedInUserID);

      // create authentication context
      AuthenticationContext authenticationContext = new AuthenticationContext(tenantAuthority, userTokenCache);

      // create client credential object using client ID and client Secret"];
      ClientCredential clientCredential = new ClientCredential(clientId, clientSecret);

      // create user identifier object for logged on user
      string objectIdentifierId = "http://schemas.microsoft.com/identity/claims/objectidentifier";
      string userObjectID = ClaimsPrincipal.Current.FindFirst(objectIdentifierId).Value;
      UserIdentifier userIdentifier = new UserIdentifier(userObjectID, UserIdentifierType.UniqueId);

      // get access token for Power BI Service API from AAD
      AuthenticationResult authenticationResult =
        await authenticationContext.AcquireTokenSilentAsync(
            resourceUrlPowerBi,
            clientCredential,
            userIdentifier);

      // return access token back to user
      return authenticationResult.AccessToken;

    }

    private static async Task<string> ExecuteGetRequest(string urlRestEndpoint) {

      string accessToken = await GetAccessTokenAsync();

      HttpClient client = new HttpClient();
      HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, urlRestEndpoint);
      request.Headers.Add("Authorization", "Bearer " + accessToken);
      request.Headers.Add("Accept", "application/json;odata.metadata=minimal");

      HttpResponseMessage response = await client.SendAsync(request);

      if (response.StatusCode != HttpStatusCode.OK) {
        throw new ApplicationException("Error!!!!!");
      }

      return await response.Content.ReadAsStringAsync();
    }

    #endregion

    public static async Task<string> HelloWorld() {
      string restUrl = urlPowerBiRestApiRoot + "v1.0/myorg/groups/";
      string jsonResult = await ExecuteGetRequest(restUrl);
      return jsonResult;
    }


  }
}