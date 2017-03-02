﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerBiRestApiDemo {
  class Program {
    static void Main() {

      PowerBiWorkspaceManager workspace = new PowerBiWorkspaceManager();

      workspace.CreateDataset();

      workspace.AddCountryRows();

      workspace.AddStateRows();


    }
  }
}
