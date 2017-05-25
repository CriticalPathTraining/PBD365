using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IdentityModel.Tokens;
using ADAL = Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace AzureGraphAdalWebClient.Controllers {
  public class TokenViewerController : Controller {

    public ActionResult IdToken() {
      string idToken = CustomAuthenticationManager.GetIdToken();
      JwtSecurityToken jwtIdToken = new JwtSecurityToken(idToken);
      return View(jwtIdToken);

    }

    public ActionResult AccessToken() {
      string accessToken = CustomAuthenticationManager.GetAccessToken(); 
      JwtSecurityToken jwtAccessToken = new JwtSecurityToken(accessToken); 
      return View(jwtAccessToken);
    }

    public ActionResult RefreshToken() {
      string refreshToken = CustomAuthenticationManager.GetRefreshToken();
      JwtSecurityToken jwtRefreshToken = new JwtSecurityToken(refreshToken);
      return View(jwtRefreshToken);
    }

    public ActionResult AppOnlyAccessToken() {

      string urlTokenEndpointForTenant = 
        string.Format("https://login.windows.net/{0}/oauth2/token",
                      CustomAuthenticationManager.GetTenantID());

      ADAL.AuthenticationContext authenticationContext 
        = new ADAL.AuthenticationContext(urlTokenEndpointForTenant);

      string resource = DemoConstants.TargetResource;
      ADAL.ClientAssertionCertificate clientAssertionCertificate = 
        DemoConstants.GetClientAssertionCertificate();

      ADAL.AuthenticationResult authenticationResult =
        authenticationContext.AcquireToken(resource, clientAssertionCertificate);

      string appOnlyAccessToken = authenticationResult.AccessToken;
      JwtSecurityToken jwtAppOnlyAccessToken = new JwtSecurityToken(appOnlyAccessToken);
      return View(jwtAppOnlyAccessToken);
      
    }

  }
}