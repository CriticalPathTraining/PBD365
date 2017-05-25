using AzureGraphSimpleWebClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AzureGraphSimpleWebClient.Controllers {

  public class ReplyUrlController : Controller {

    public async Task<ActionResult> Index() {

      var client = new HttpClient();
      client.BaseAddress = new Uri(DemoConstants.AccessTokenRequesrUrl);

      string auth_code = Request.QueryString["code"];

      var content = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string, string>("grant_type", "authorization_code"),
                new KeyValuePair<string, string>("resource", DemoConstants.TargetResource),
                new KeyValuePair<string, string>("redirect_uri", DemoConstants.DebugSiteRedirectUrl),
                new KeyValuePair<string, string>("client_id", DemoConstants.ClientId),
                new KeyValuePair<string, string>("client_secret", DemoConstants.ClientSecret),
                new KeyValuePair<string, string>("code", auth_code)
            });

      var result = await client.PostAsync(DemoConstants.AccessTokenRequesrUrl, content);

      string responseText = result.Content.ReadAsStringAsync().Result;

      JsonWebToken jwt = JsonWebToken.Deserialize(responseText);

      return View(jwt);
    }
  }
}