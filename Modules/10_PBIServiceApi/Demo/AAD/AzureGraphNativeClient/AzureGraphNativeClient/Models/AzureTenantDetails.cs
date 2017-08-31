using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureGraphNativeClient.Models {
  public class AzureTenantDetails {
    public string objectType { get; set; }
    public string objectId { get; set; }
    public string displayName { get; set; }
    public object city { get; set; }
    public string telephoneNumber { get; set; }
  }
}
