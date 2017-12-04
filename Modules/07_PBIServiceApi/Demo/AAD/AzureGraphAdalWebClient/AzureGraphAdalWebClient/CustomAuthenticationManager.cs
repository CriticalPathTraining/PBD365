using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Web.SessionState;

public class CustomAuthenticationManager {

  static HttpSessionState session = System.Web.HttpContext.Current.Session;

  public static void CacheAuthenticationCode(string AuthenticationCode) {
    session["authentication_code"] = AuthenticationCode;

  }

  public static string GetAuthenticationCode() {
    return session["authentication_code"].ToString();
  }


  public static void CacheAuthenticationResult(AuthenticationResult authenticationResult) {
    session["UserHasAuthenticated"] = "true";
    // cache user authentication info
    session["tenant_id"] = authenticationResult.TenantId;
    session["user_id"] = authenticationResult.UserInfo.UniqueId;
    session["user_name"] = authenticationResult.UserInfo.GivenName + " " +
                           authenticationResult.UserInfo.FamilyName;
    // cache security tokens
    session["id_token"] = authenticationResult.IdToken;
    session["access_token"] = authenticationResult.AccessToken;
    session["refresh_token"] = authenticationResult.RefreshToken;

  }

  public static void RefreshAccessToken(AuthenticationResult authenticationResult) {
    session["access_token"] = authenticationResult.AccessToken;
    session["refresh_token"] = authenticationResult.RefreshToken;

  }

  public static bool UserHasAuthentiated() {
    return (session["UserHasAuthenticated"] != null) &&
           (session["UserHasAuthenticated"].Equals("true"));
  }

  public static string GetTenantID() {
    return session["tenant_id"].ToString();
  }

  public static string GetUserID() {
    return session["user_id"].ToString();
  }

  public static string GetUserName() {
    return session["user_name"].ToString();
  }

  public static string GetIdToken() {
    return session["id_token"].ToString();
  }

  public static string GetAccessToken() {
    return session["access_token"].ToString();
  }

  public static string GetRefreshToken() {
    return session["refresh_token"].ToString();
  }

  public static void ClearTokenCache() {
    //session.Abandon();
    session.Clear();
  }

  }