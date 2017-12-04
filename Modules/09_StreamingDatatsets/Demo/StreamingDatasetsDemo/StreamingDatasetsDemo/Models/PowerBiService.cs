using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;

using Newtonsoft.Json;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace StreamingDatasetsDemo {

  class PowerBiServiceWrapper {

    public const string AzureAuthorizationEndpoint = "https://login.microsoftonline.com/common";
    public const string PowerBiServiceResourceUri = "https://analysis.windows.net/powerbi/api";
    public const string PowerBiServiceRootUrl = "https://api.powerbi.com/v1.0/myorg/";

    public const string ClientID = "bbc4bbc1-ca57-4bb0-a996-4228f49fd09b";
    public const string RedirectUri = "https://localhost/app1234";

    #region "Authentication Details"

    protected string AccessToken = string.Empty;

    protected void GetAccessToken() {

      // create new authentication context 
      var authenticationContext = new AuthenticationContext(AzureAuthorizationEndpoint);

      //// use authentication context to trigger user sign-in and return access token 
      //var userAuthnResult = authenticationContext.AcquireTokenAsync(PowerBiServiceResourceUri,
      //                                                         ClientID,
      //                                                         new Uri(RedirectUri),
      //                                                         new PlatformParameters(PromptBehavior.Auto)).Result;

      // use authentication context to trigger user sign-in and return access token 
      UserPasswordCredential creds = new UserPasswordCredential("Student@cpt0828.onMicrosoft.com", "Pa$$word!");
      var userAuthnResult = authenticationContext.AcquireTokenAsync(PowerBiServiceResourceUri,
                                                               ClientID,
                                                               creds).Result;


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

    public PowerBiServiceWrapper() {
      GetAccessToken();
    }

    private string GetDatasetId(string DatasetName) {
      string restUrlDatasets = PowerBiServiceRootUrl + "datasets/";
      string json = ExecuteGetRequest(restUrlDatasets);
      DatasetCollection datasets = JsonConvert.DeserializeObject<DatasetCollection>(json);
      foreach (var ds in datasets.value) {
        if (ds.name.Equals(DatasetName)) {
          return ds.id;
        }
      }
      return string.Empty;
    }

    public void CreateDemoPushDataset(string DatasetName) {
      string restUrlDatasets = PowerBiServiceRootUrl + "datasets";

      // check to see if dataset exists
      string DatasetId = GetDatasetId(DatasetName);
      if (string.IsNullOrEmpty(DatasetId)) {
        Console.WriteLine("Creating new Push Dataset named " + DatasetName + " ...");
        string jsonNewDataset = Properties.Resources.DemoPushDataset.Replace("@DatasetName", DatasetName);
        // execute REST call to create new dataset
        string json = ExecutePostRequest(restUrlDatasets, jsonNewDataset);
        // retrieve Guid to track dataset ID
        Dataset dataset = JsonConvert.DeserializeObject<Dataset>(json);
        DatasetId = GetDatasetId(DatasetName);
      }
      else { 
        // if dataset exists, delete all existing table rows
        string restUrlExpensesTableRows = string.Format("{0}/{1}/tables/Expenses/rows", restUrlDatasets, DatasetId);
        ExecuteDeleteRequest(restUrlExpensesTableRows);
      }

      // add rows
      ExpenseTableRows expensesData = SampleData.GetExpense();
      Console.WriteLine();
      Console.Write("Adding rows");
      foreach (var expenseRow in expensesData.rows) {
        Console.Write(".");
        expenseRow.Time = DateTime.Now.AddHours(-4);
        expenseRow.TimeWindow = DateTime.Now.Hour.ToString("00") + ":" +
                                DateTime.Now.Minute.ToString("00") + ":" +
                                ((DateTime.Now.Second / 10) * 10).ToString("00");

        ExpenseRow[] Expenses = { expenseRow };
        ExpenseTableRows expenseRows = new ExpenseTableRows { rows = Expenses };
        string jsonExpenseRows = JsonConvert.SerializeObject(expenseRows);
        string restUrlExpenseTableRows = string.Format("{0}/{1}/tables/Expenses/rows", restUrlDatasets, DatasetId);
        string json = ExecutePostRequest(restUrlExpenseTableRows, jsonExpenseRows);

        Thread.Sleep(500);
      }

    }

    public void CreateDemoStreamingDataset(string DatasetName) {
      string restUrlDatasets = PowerBiServiceRootUrl + "datasets";

      // check to see if dataset exists
      string DatasetId = GetDatasetId(DatasetName);
      if (string.IsNullOrEmpty(DatasetId)) {
        Console.WriteLine("Creating new datset named " + DatasetName);
        // create dataet if it does not exist
        string jsonNewDataset = Properties.Resources.DemoStreamingDataset.Replace("@DatasetName", DatasetName);
        // execute REST call to create new dataset
        string json = ExecutePostRequest(restUrlDatasets, jsonNewDataset);
        // retrieve Guid to track dataset ID
        Dataset dataset = JsonConvert.DeserializeObject<Dataset>(json);
        Console.WriteLine("Creating new Streaming Dataset named " + DatasetName + "...");
        // get Dataset ID once it has been created
        DatasetId = GetDatasetId(DatasetName);
      }

      Console.WriteLine();
      Console.Write("Adding rows");
      double temperatureBatchA = 100;
      double temperatureBatchB = 100;
      double temperatureBatchC = 100;
      Random rand = new Random(714);

      while (true) {

        Console.Write(".");

        double bumpA = rand.Next(-50, 240) / (double)100;
        temperatureBatchA += bumpA;
        if (temperatureBatchA < 212) {
          temperatureBatchA = 212;
        }
        TemperatureReadingsRow rowA = new TemperatureReadingsRow {
          Temperature = temperatureBatchA,
          TargetTemperature = 212,
          MaxTemperature = 250,
          Batch = "Batch A",
          Time = DateTime.Now.AddHours(-4)
        };


        double bumpB = rand.Next(-80, 280) / (double)100;
        temperatureBatchB += bumpB;
        if (temperatureBatchB < 212) {
          temperatureBatchB = 212;
        }
        TemperatureReadingsRow rowB = new TemperatureReadingsRow {
          Temperature = temperatureBatchB,
          TargetTemperature = 212,
          MaxTemperature = 250,
          Batch = "Batch B",
          Time = DateTime.Now.AddHours(-4)
        };


        double bumpC = rand.Next(-20, 350) / (double)100;
        temperatureBatchC += bumpC;
        if (temperatureBatchC < 212) {
          temperatureBatchC = 212;
        }
        TemperatureReadingsRow rowC = new TemperatureReadingsRow {
          Temperature = temperatureBatchC,
          TargetTemperature = 212,
          MaxTemperature = 250,
          Batch = "Batch C",
          Time = DateTime.Now.AddHours(-4)
        };



        TemperatureReadingsRow[] rows = { rowA, rowB, rowC };
        TemperatureReadingsRows temperatureReadingsRows = new TemperatureReadingsRows { rows = rows };
        string jsonNewRows = JsonConvert.SerializeObject(temperatureReadingsRows);
        string restUrlTargetTableRows = string.Format("{0}/{1}/tables/TemperatureReadings/rows", restUrlDatasets, DatasetId);
        string jsonResultAddExpenseRows = ExecutePostRequest(restUrlTargetTableRows, jsonNewRows);

        Thread.Sleep(500);
      }


    }

    public void CreateDemoHybridDataset(string DatasetName) {
      string restUrlDatasets = PowerBiServiceRootUrl + "datasets";

      // check to see if dataset exists
      string DatasetId = GetDatasetId(DatasetName);
      if (string.IsNullOrEmpty(DatasetId)) {
        Console.WriteLine("Creating new hybrid datset named " + DatasetName);
        // create dataet if it does not exist
        string jsonNewDataset = Properties.Resources.DemoHybridDataset.Replace("@DatasetName", DatasetName);
        // execute REST call to create new dataset
        string json = ExecutePostRequest(restUrlDatasets, jsonNewDataset);
        // retrieve Guid to track dataset ID
        Dataset dataset = JsonConvert.DeserializeObject<Dataset>(json);
        Console.WriteLine("Creating new Hybrid Dataset named " + DatasetName + "...");
        // get Dataset ID once it has been created
        DatasetId = GetDatasetId(DatasetName);
      }

      Console.WriteLine();
      Console.Write("Adding rows");
      double temperatureBatchA = 100;
      double temperatureBatchB = 100;
      double temperatureBatchC = 100;
      Random rand = new Random(714);

      while (true) {

        Console.Write(".");

        double bumpA = rand.Next(-50, 240) / (double)100;
        temperatureBatchA += bumpA;
        if (temperatureBatchA < 212) {
          temperatureBatchA = 212;
        }
        TemperatureReadingsRow rowA = new TemperatureReadingsRow {
          Temperature = temperatureBatchA,
          TargetTemperature = 212,
          MaxTemperature = 250,
          Batch = "Batch A",
          Time = DateTime.Now.AddHours(-4)
        };


        double bumpB = rand.Next(-80, 280) / (double)100;
        temperatureBatchB += bumpB;
        if (temperatureBatchB < 212) {
          temperatureBatchB = 212;
        }
        TemperatureReadingsRow rowB = new TemperatureReadingsRow {
          Temperature = temperatureBatchB,
          TargetTemperature = 212,
          MaxTemperature = 250,
          Batch = "Batch B",
          Time = DateTime.Now.AddHours(-4)
        };


        double bumpC = rand.Next(-20, 350) / (double)100;
        temperatureBatchC += bumpC;
        if (temperatureBatchC < 212) {
          temperatureBatchC = 212;
        }
        TemperatureReadingsRow rowC = new TemperatureReadingsRow {
          Temperature = temperatureBatchC,
          TargetTemperature = 212,
          MaxTemperature = 250,
          Batch = "Batch C",
          Time = DateTime.Now.AddHours(-4)
        };



        TemperatureReadingsRow[] rows = { rowA, rowB, rowC };
        TemperatureReadingsRows temperatureReadingsRows = new TemperatureReadingsRows { rows = rows };
        string jsonNewRows = JsonConvert.SerializeObject(temperatureReadingsRows);
        string restUrlTargetTableRows = string.Format("{0}/{1}/tables/TemperatureReadings/rows", restUrlDatasets, DatasetId);
        string jsonResultAddExpenseRows = ExecutePostRequest(restUrlTargetTableRows, jsonNewRows);

        Thread.Sleep(500);
      }


    }

  }

  public class Dataset {
    public string id { get; set; }
    public string name { get; set; }
  }

  public class DatasetCollection {
    public List<Dataset> value { get; set; }
  }

  public class ExpenseRow {
    public string Expense { get; set; }
    public string Category { get; set; }
    public double Amount { get; set; }
    public DateTime Time { get; set; }
    public string TimeWindow { get; set; }
  }

  class ExpenseTableRows {
    public ExpenseRow[] rows { get; set; }
  }

  public class TemperatureReadingsRow {
    public double Temperature { get; set; }
    public double TargetTemperature { get; set; }
    public double MaxTemperature { get; set; }
    public string Batch { get; set; }
    public DateTime Time { get; set; }
  }

  class TemperatureReadingsRows {
    public TemperatureReadingsRow[] rows { get; set; }
  }

  class SampleData {
    public static ExpenseTableRows GetExpense() {
      ExpenseRow[] Expenses = {
        new ExpenseRow { Expense="Pens and staples", Category="Office Supplies", Amount=32.23 },
        new ExpenseRow { Expense="Google Add Words", Category="Marketing", Amount=221.23 },
        new ExpenseRow { Expense="Bing Add Words", Category="Marketing", Amount=100 },
        new ExpenseRow { Expense="New Chair", Category="Office Supplies", Amount=429 },
        new ExpenseRow { Expense="Electricity Bill", Category="Operations", Amount=215.21 },
        new ExpenseRow { Expense="Google Add Words", Category="Marketing", Amount=215 },
        new ExpenseRow { Expense="Print Ads", Category="Marketing", Amount=500 },
        new ExpenseRow { Expense="Photos", Category="Marketing", Amount=5.34 },
        new ExpenseRow { Expense="Google Add Words", Category="Marketing", Amount=400 },
        new ExpenseRow { Expense="Electricity Bill", Category="Operations", Amount=234 },
        new ExpenseRow { Expense="New Printer", Category="Office Supplies", Amount=121 },
        new ExpenseRow { Expense="Print Ads", Category="Marketing", Amount=120 },
        new ExpenseRow { Expense="Swag", Category="Marketing", Amount=120 },
        new ExpenseRow { Expense="Cleaning Supplies", Category="Office Supplies", Amount=32 },
        new ExpenseRow { Expense="Cleaning Service", Category="Operations", Amount=234 },
        new ExpenseRow { Expense="Pencils", Category="Office Supplies", Amount=5.21 },
        new ExpenseRow { Expense="Google Add Words", Category="Marketing", Amount=5.21 },
        new ExpenseRow { Expense="Electricity Bill", Category="Operations", Amount=520 },
        new ExpenseRow { Expense="Google Ad Words", Category="Marketing", Amount=120 },
        new ExpenseRow { Expense="Print Ads", Category="Marketing", Amount=200 },
        new ExpenseRow { Expense="Print Ads", Category="Marketing", Amount=121 },
        new ExpenseRow { Expense="Bing Ad Words", Category="Marketing", Amount=200 },
        new ExpenseRow { Expense="Electricity Bill", Category="Operations", Amount=210 },
        new ExpenseRow { Expense="Google Add Words", Category="Marketing", Amount=200 },
        new ExpenseRow { Expense="New Server", Category="Operations", Amount=2300 }
      };
      return new ExpenseTableRows { rows = Expenses };

    }
  }
}