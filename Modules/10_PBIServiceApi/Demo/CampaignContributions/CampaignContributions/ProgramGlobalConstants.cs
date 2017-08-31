
using System.Configuration;

namespace CampaignContributions {
  class ProgramGlobalConstants {

    public const string AzureAuthorizationEndpoint = "https://login.microsoftonline.com/common";
    public const string PowerBiServiceResourceUri = "https://analysis.windows.net/powerbi/api";
    public const string PowerBiServiceRootUrl = "https://api.powerbi.com/v1.0/myorg/";

    public static readonly string ClientID = ConfigurationManager.AppSettings["ClientID"];
    public static readonly string RedirectUri = ConfigurationManager.AppSettings["RedirectUri"];
  }
}


