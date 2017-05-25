using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AzureGraphAdalWebClient.Controllers {
  public class AccountController : Controller {

    public ActionResult SignIn() {
      return Redirect(DemoConstants.urlAuthorizationEndpoint);
    }

    public ActionResult SignOut() {
      CustomAuthenticationManager.ClearTokenCache();
      return RedirectToAction("Index", "Home");
    }

    public ActionResult RefreshAccessToken() {

      string refreshToken = CustomAuthenticationManager.GetRefreshToken();

      ClientCredential credential =
        new ClientCredential(DemoConstants.ClientId, DemoConstants.ClientSecret);

      string resource = DemoConstants.TargetResource;
   
      AuthenticationContext authenticationContext = 
        new AuthenticationContext(DemoConstants.urlAuthorizationEndpoint);

      AuthenticationResult authenticationResult =
        authenticationContext.AcquireTokenByRefreshToken(refreshToken, credential, resource);

      CustomAuthenticationManager.RefreshAccessToken(authenticationResult);

      return RedirectToAction("AccessToken", "TokenViewer");
    }
  }
}