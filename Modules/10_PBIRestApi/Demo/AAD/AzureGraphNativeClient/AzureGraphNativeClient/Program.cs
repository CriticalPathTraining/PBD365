using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.IdentityModel.Clients.ActiveDirectory;

using Microsoft.Azure.ActiveDirectory.GraphClient;
using System.Net.Http;

using AzureGraphNativeClient.Models;
using Newtonsoft.Json.Linq;

namespace AzureGraphNativeClient {

  class Program {

    protected static string AccessToken = string.Empty;

    protected static void GetAccessToken() {

      // shared login authority for all Office 365 tenants
      string authority = "https://login.microsoftonline.com/common";

      // create new authentication context 
      var authenticationContext = new AuthenticationContext(authority);

      // create URI for target resource
      string urlAzureGraphApi = "https://graph.windows.net/";
      string tenantDomain = "CptLabs.onMicrosoft.com";
      Uri uriAzureGraphApiResource = new Uri(urlAzureGraphApi + tenantDomain);

      // 
      string clientID = "19f2d875-25b2-4ce1-89fc-8215f6de6dba";
      string redirectUri = "https://localhost/AzureGraphNativeClient";


      // use authentication context to trigger user sign-in and return access token 
      var userAuthnResult = authenticationContext.AcquireToken(urlAzureGraphApi,
                                                               clientID,
                                                               new Uri(redirectUri),
                                                               PromptBehavior.Auto);
      // cache access token in AccessToken field
      AccessToken = userAuthnResult.AccessToken;


    }

    static void Main() {

      GetAccessToken();
      CallToGraphApiWithREST();
      CallToGraphApiWithADClient();

      Console.WriteLine();
      Console.WriteLine("Press ENTER to end");
      Console.ReadLine();
    }

    static void CallToGraphApiWithREST() {
      Console.WriteLine();
      Console.WriteLine("Calling To Graph API using raw REST calls");

      // create URI for target resource
      string urlAzureGraphApi = "https://graph.windows.net/";
      string tenantDomain = "CptLabs.onMicrosoft.com/";
      string restTargetObject = "tenantDetails";
      string queryString = "?api-version=1.6";
      Uri restUri = new Uri(urlAzureGraphApi + tenantDomain + restTargetObject + queryString);


      HttpClient client = new HttpClient();
      client.DefaultRequestHeaders.Add("Authorization", "Bearer " + AccessToken);
      client.DefaultRequestHeaders.Add("Accept", "application/json");

      HttpResponseMessage response = client.GetAsync(restUri).Result;

      if (response.IsSuccessStatusCode) {
        string json = response.Content.ReadAsStringAsync().Result;
        var TenantDetailsList = JObject.Parse(json).SelectToken("value").ToObject<List<AzureTenantDetails>>();
        var TenantDetails = TenantDetailsList.FirstOrDefault();
        Console.WriteLine(" - Display Name: " + TenantDetails.displayName);
        Console.WriteLine(" - Tenant City : " + TenantDetails.city);
        Console.WriteLine(" - Tenant Phone: " + TenantDetails.telephoneNumber);
      }
      else {
        Console.WriteLine("ERROR - REST call failed");
      }

    }

    private static Task<string> AccessTokenFunc() {
      // return access token to caller 
      return Task.FromResult(AccessToken);
    }

    static void CallToGraphApiWithADClient() {
      Console.WriteLine();
      Console.WriteLine("Calling To Graph API using .NET Active Directory Client");


      // create URI for target resource
      string urlAzureGraphApi = "https://graph.windows.net/";
      string tenantDomain = "CptLabs.onMicrosoft.com";
      Uri uriAzureGraphApiResource = new Uri(urlAzureGraphApi + tenantDomain);


      ActiveDirectoryClient activeDirectoryClient =
      new ActiveDirectoryClient(
        uriAzureGraphApiResource,
        async () => { return await AccessTokenFunc(); }
      );

      var TenantDetails = activeDirectoryClient.TenantDetails.ExecuteAsync().Result.CurrentPage.First();
      Console.WriteLine(" - Display Name: " + TenantDetails.DisplayName);
      Console.WriteLine(" - Tenant City : " + TenantDetails.City);
      Console.WriteLine(" - Tenant Phone: " + TenantDetails.TelephoneNumber);
    }
  }
}
