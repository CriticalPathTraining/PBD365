using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;

using AzureGraphAdalWebClient.Models;
using Newtonsoft.Json.Linq;

namespace AzureGraphAdalWebClient.Controllers {
  public class UserInfoController : Controller {

    public ActionResult Index() {

      string tenantId = CustomAuthenticationManager.GetTenantID();
      string userId = CustomAuthenticationManager.GetUserID();

      string urlRestTemplate = "https://graph.windows.net/{0}/users/{1}?api-version=1.5";
      string urlRest = string.Format(urlRestTemplate, tenantId, userId);
      Uri uriRest = new Uri(urlRest);

      string accessToken = CustomAuthenticationManager.GetAccessToken();

      HttpClient client = new HttpClient();
      client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
      client.DefaultRequestHeaders.Add("Accept", "application/json");

      HttpResponseMessage response = client.GetAsync(uriRest).Result;

      if (response.IsSuccessStatusCode) {
        string json = response.Content.ReadAsStringAsync().Result;
        ADUser userInfo = JObject.Parse(json).ToObject<ADUser>();
        return View(userInfo);
      }
      else {
        throw new ApplicationException("Whoops");
      }
      
    }
  }
}