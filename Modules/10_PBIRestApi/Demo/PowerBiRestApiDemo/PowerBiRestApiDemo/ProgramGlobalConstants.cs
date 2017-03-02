using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerBiRestApiDemo {

  class ProgramGlobalConstants {

    public const string AzureAuthorizationEndpoint = "https://login.microsoftonline.com/common";
    public const string PowerBiServiceResourceUri = "https://analysis.windows.net/powerbi/api";
    public const string PowerBiServiceRootUrl = "https://api.powerbi.com/v1.0/myorg/";

    public const string ClientID = "bc6b8f66-390b-4ad5-9dc6-9637f7f9841f";
    public const string RedirectUri = "https://localhost/PowerBiRestApiDemo";

    public const string DatasetName = "My Custom Dataset";

  }
}

