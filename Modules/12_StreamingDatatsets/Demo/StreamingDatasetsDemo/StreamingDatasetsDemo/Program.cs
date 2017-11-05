using System;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.PowerBI.Api.V2;
using Microsoft.PowerBI.Api.V2.Models;
using Microsoft.Rest;

namespace StreamingDatasetsDemo {
  class Program {

    static PowerBiServiceWrapper pbiService = new PowerBiServiceWrapper();

    static void Main() {


      //pbiService.CreateDemoPushDataset("Demo Push Dataset");
      //pbiService.CreateDemoStreamingDataset("Demo Streaming Dataset");
      pbiService.CreateDemoHybridDataset("Demo Hybrid Dataset");

    }
  }

}
