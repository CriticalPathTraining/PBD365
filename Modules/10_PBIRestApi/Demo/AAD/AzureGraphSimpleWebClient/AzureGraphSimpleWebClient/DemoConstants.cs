using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AzureGraphSimpleWebClient {

  public class DemoConstants {
    
    public const string ClientId = "86e8abf3-d9b1-422c-8f7c-ba58d153c8a2";

    public const string ClientSecret = "apR1t33pcD0PbYrP8fYpl7obstr6ZLYy5W5RuMvM//E=";
    public static readonly string ClientSecretEncoded = HttpUtility.UrlEncode(ClientSecret);

    public const string TargetResource = "https://graph.windows.net";

    public const string DebugSiteUrl = "https://localhost:44300/";
    public const string DebugSiteRedirectUrl = "https://localhost:44300/ReplyUrl/";

    public const string AADAuthUrl = "https://login.windows.net/common/oauth2/authorize" +
                                      "?resource=" + TargetResource +
                                      "&client_id=" + ClientId +
                                      "&redirect_uri=" + DebugSiteRedirectUrl +
                                      "&response_type=code";

    public const string AccessTokenRequesrUrl = "https://login.windows.net/common/oauth2/token" +
                                         "";

    public static string AccessTokenRequestBody = "grant_type=authorization_code" +
                                                   "&resource=" + TargetResource +
                                                   "&redirect_uri=" + DebugSiteRedirectUrl +
                                                   "&client_id=" + ClientId +
                                                   "&client_secret=" + ClientSecretEncoded +
                                                   "&code=";
  }
}