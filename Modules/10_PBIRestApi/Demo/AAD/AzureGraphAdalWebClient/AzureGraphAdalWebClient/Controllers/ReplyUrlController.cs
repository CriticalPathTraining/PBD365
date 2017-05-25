using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AzureGraphAdalWebClient.Controllers {

  
  public class ReplyUrlController : Controller {

    public ActionResult Index(string code) {

      CustomAuthenticationManager.CacheAuthenticationCode(code);
      
      ClientCredential credential = 
        new ClientCredential(DemoConstants.ClientId, DemoConstants.ClientSecret);

      string resource = DemoConstants.TargetResource;
      Uri uriReplyUrl = new Uri(DemoConstants.ClientReplyUrl);

      AuthenticationContext authenticationContext = new AuthenticationContext(DemoConstants.urlAuthorizationEndpoint);

      AuthenticationResult authenticationResult =
        authenticationContext.AcquireTokenByAuthorizationCode(
                      code,
                      uriReplyUrl,
                      credential, 
                      resource);

      CustomAuthenticationManager.CacheAuthenticationResult(authenticationResult);

      ViewBag.AuthenticationCode = code;

      return View(authenticationResult);

    }
  }
}