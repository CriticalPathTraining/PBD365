using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;

using Newtonsoft.Json;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace PowerBiRestApiDemo {

  class PowerBiWorkspaceManager {

    #region "Authentication Details"

    protected string AccessToken = string.Empty;

    protected void GetAccessToken() {

      // create new authentication context 
      var authenticationContext = new AuthenticationContext(ProgramGlobalConstants.AzureAuthorizationEndpoint);

      // use authentication context to trigger user sign-in and return access token 
      var userAuthnResult = authenticationContext.AcquireToken(ProgramGlobalConstants.PowerBiServiceResourceUri,
                                                               ProgramGlobalConstants.ClientID,
                                                               new Uri(ProgramGlobalConstants.RedirectUri),
                                                               PromptBehavior.Auto);
      // cache access token in AccessToken field
      AccessToken = userAuthnResult.AccessToken;

    }

    #endregion

    #region "REST operation utility methods"

    private string ExecuteGetRequest(string restUri) {

      HttpClient client = new HttpClient();
      client.DefaultRequestHeaders.Add("Authorization", "Bearer " + AccessToken);
      client.DefaultRequestHeaders.Add("Accept", "application/json");

      HttpResponseMessage response = client.GetAsync(restUri).Result;

      if (response.IsSuccessStatusCode) {
        return response.Content.ReadAsStringAsync().Result;
      }
      else {
        Console.WriteLine();
        Console.WriteLine("OUCH! - error occurred during GET REST call");
        Console.WriteLine();
        return string.Empty;
      }
    }

    private string ExecutePostRequest(string restUri, string postBody) {

      try {
        HttpContent body = new StringContent(postBody);
        body.Headers.ContentType = new MediaTypeWithQualityHeaderValue("application/json");
        HttpClient client = new HttpClient();
        client.DefaultRequestHeaders.Add("Accept", "application/json");
        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + AccessToken);
        HttpResponseMessage response = client.PostAsync(restUri, body).Result;

        if (response.IsSuccessStatusCode) {
          return response.Content.ReadAsStringAsync().Result;
        }
        else {
          Console.WriteLine();
          Console.WriteLine("OUCH! - error occurred during POST REST call");
          Console.WriteLine();
          return string.Empty;
        }
      }
      catch {
        Console.WriteLine();
        Console.WriteLine("OUCH! - error occurred during POST REST call");
        Console.WriteLine();
        return string.Empty;
      }
    }

    private string ExecuteDeleteRequest(string restUri) {
      HttpClient client = new HttpClient();
      client.DefaultRequestHeaders.Add("Accept", "application/json");
      client.DefaultRequestHeaders.Add("Authorization", "Bearer " + AccessToken);
      HttpResponseMessage response = client.DeleteAsync(restUri).Result;

      if (response.IsSuccessStatusCode) {
        return response.Content.ReadAsStringAsync().Result;
      }
      else {
        Console.WriteLine();
        Console.WriteLine("OUCH! - error occurred during Delete REST call");
        Console.WriteLine();
        return string.Empty;
      }
    }

    #endregion

    public PowerBiWorkspaceManager() {
      GetAccessToken();
    }

    private bool DatasetExist(string datasetName) {
      string restUrlDatasets = ProgramGlobalConstants.PowerBiServiceRootUrl + "datasets/";
      string json = ExecuteGetRequest(restUrlDatasets);
      DatasetCollection datasets = JsonConvert.DeserializeObject<DatasetCollection>(json);
      foreach (var ds in datasets.value) {
        if (ds.name.Equals(datasetName)) {
          CustomDatasetId = ds.id;
          return true;
        }
      }
      return false;
    }

    private string CustomDatasetId = string.Empty;

    public void CreateDataset() {
      // check to see if dataset exists
      string datasetName = ProgramGlobalConstants.DatasetName;
      if (DatasetExist(datasetName)) {
        // if dataset exists, delete all existing table rows
        DeleteRows();
        Console.WriteLine("Connected to " + datasetName + " dataset");
        return;
      }
      // prepare call to create new dataset
      string restUrlDatasets = ProgramGlobalConstants.PowerBiServiceRootUrl + "datasets";
      string jsonNewDataset = Properties.Resources.NewDataset_json;
      // execute REST call to create new dataset
      string json = ExecutePostRequest(restUrlDatasets, jsonNewDataset);
      // retrieve Guid to track dataset ID
      Dataset dataset = JsonConvert.DeserializeObject<Dataset>(json);
      CustomDatasetId = dataset.id;
      Console.WriteLine("Creating new Power BI Dataset named " + datasetName + "...");
    }

    public void DeleteRows() {
      string restUrlDatasets = ProgramGlobalConstants.PowerBiServiceRootUrl + "datasets/";

      string restUrlCountryTableRows = string.Format("{0}/{1}/tables/Countries/rows", restUrlDatasets, CustomDatasetId);
      ExecuteDeleteRequest(restUrlCountryTableRows);

      string restUrlStateTableRows = string.Format("{0}/{1}/tables/States/rows", restUrlDatasets, CustomDatasetId);
      ExecuteDeleteRequest(restUrlStateTableRows);
    }

    public void AddCountryRows() {
      string restUrlDatasets = ProgramGlobalConstants.PowerBiServiceRootUrl + "datasets/";
      CountryTableRows countryRows = SampleData.GetCountries();
      string jsonCountryRows = JsonConvert.SerializeObject(countryRows);
      string restUrlCountryTableRows = string.Format("{0}/{1}/tables/Countries/rows", restUrlDatasets, CustomDatasetId);
      string json = ExecutePostRequest(restUrlCountryTableRows, jsonCountryRows);
    }

    public void AddStateRows() {

      string restUrlDatasets = ProgramGlobalConstants.PowerBiServiceRootUrl + "datasets/";

      StateTableRows stateRows = SampleData.GetStates();

      foreach (StateRow state in stateRows.rows) {
        StateRow[] rows = { state };
        StateTableRows singleRowBatch = new StateTableRows { rows = rows };
        string jsonStateRows = JsonConvert.SerializeObject(singleRowBatch);
        string restUrlStateTableRows = string.Format("{0}/{1}/tables/States/rows", restUrlDatasets, CustomDatasetId);
        string json = ExecutePostRequest(restUrlStateTableRows, jsonStateRows);

        // wait 1 second before adding next state
        System.Threading.Thread.Sleep(1000);
      }


    }
  }
}
