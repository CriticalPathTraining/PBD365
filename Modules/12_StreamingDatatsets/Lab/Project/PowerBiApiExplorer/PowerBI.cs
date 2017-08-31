using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;
using System.Net;

// Available with the Active Directory Authentication Library NuGet package
// See: https://msdn.microsoft.com/en-us/library/dn877545.aspx#Library
using Microsoft.IdentityModel.Clients.ActiveDirectory;

using Newtonsoft.Json;

namespace PowerBiApiExplorer
{
    class PowerBI
    {
        // Set by the user in the Settings form
        internal static string clientId = ConfigurationManager.AppSettings["ClientId"];
        internal static string redirectUri = ConfigurationManager.AppSettings["RedirectUri"];

        // Application settings
        internal static string resourceUri = ConfigurationManager.AppSettings["ResourceUri"];
        internal static string authorityUri = ConfigurationManager.AppSettings["AuthorityUri"];
        internal static string powerBiApiUri = ConfigurationManager.AppSettings["PowerBiApiUri"];

        // authenticationContext represents the token issuing authority for Azure AD resources
        private static AuthenticationContext authenticationContext = null;

        // Outputs a token string to be used by all Power BI API requests
        // Token string is cached for reuse within the session
        public static string GetAccessToken()
        {

            // Validate application settings have been entered
            if ((clientId == String.Empty) || (redirectUri == String.Empty))
            {
                throw new ArgumentException("Client ID and Redirect URI must be set");
            } 
            
            Console.WriteLine("> Calling GetAccessToken()");
            Console.Write("> Getting access token... ");

            string token = null;

            try
            {
                if (authenticationContext == null)
                {
                    // Create an instance of TokenCache to cache the access token
                    TokenCache tokenCache = new TokenCache();

                    // Create an instance of AuthenticationContext to acquire an Azure access token
                    authenticationContext = new AuthenticationContext(authorityUri, tokenCache);

                    // Call AcquireToken to get an Azure token from Azure Active Directory token issuance endpoint
                    token = authenticationContext.AcquireToken(resourceUri, clientId, new Uri(redirectUri)).AccessToken;

                    Console.WriteLine("New token acquired and cached");
                }
                else
                {
                    // Retrieve the token from the cache
                    token = authenticationContext.AcquireTokenSilent(resourceUri, clientId).AccessToken;

                    Console.WriteLine("Existing token retrieved from cache");
                }

                return token;
            }
            catch (Exception ex)
            {
                Console.WriteLine("> Error: " + ex.Message);
                Console.WriteLine(">");

                return null;
            }
        }

        public static void Disconnect()
        {
            Console.WriteLine("> Calling Disconnect()");

            authenticationContext.TokenCache.Clear();
            authenticationContext = null;

            Console.WriteLine("> Token cache cleared");
            Console.WriteLine(">");
        }

        // Sends GET method to https://api.powerbi.com/v1.0/myorg/groups
        // Outputs a JSON response describing all groups
        public static Group[] GetGroups()
        {
            Console.WriteLine("> Calling GetGroups()");

            try
            {
                // Create a GET web request
                HttpWebRequest request = CreateRequest(GetGroupsUri(), "GET", GetAccessToken());

                // Get HttpWebResponse from GET request
                string responseContent = GetResponse(request);

                // Deserialize JSON response into objects
                GetGroupsResponse response = JsonConvert.DeserializeObject<GetGroupsResponse>(responseContent);

                Console.WriteLine("> Succeeded\r\n>");

                return response.value;
            }
            catch (Exception ex)
            {
                Console.WriteLine("> Error: " + ex.Message);
                Console.WriteLine(">");

                return null;
            }
        }

        // If group is null, then retrieve datasets from the user's workspace
        // Outputs a JSON response describing all datasets
        public static Dataset[] GetDatasets(Group group)
        {
            Console.WriteLine("> Calling GetDatasets()");

            try
            {
                // Create a GET web request
                HttpWebRequest request = CreateRequest(GetDatasetsUri(group), "GET", GetAccessToken());

                // Get HttpWebResponse from GET request
                string responseContent = GetResponse(request);

                // Deserialize JSON response into objects
                GetDatasetsResponse response = JsonConvert.DeserializeObject<GetDatasetsResponse>(responseContent);

                Console.WriteLine("> Succeeded\r\n>");

                return response.value;
            }
            catch (Exception ex)
            {
                Console.WriteLine("> Error: " + ex.Message);
                Console.WriteLine(">");

                return null;
            }
        }

        // If group is null, then retrieve datasets from the user's workspace
        // Outputs a JSON response describing all datasets
        public static Dataset CreateDataset(Group group, string jsonContent, string defaultRetentionPolicy)
        {
            Console.WriteLine("> Calling CreateDataset()");

            try
            {
                // Validate defaultRetentionPolicy argument
                if (!((defaultRetentionPolicy == "None") || (defaultRetentionPolicy == "basicFIFO")))
                {
                    throw new ArgumentException("Invalid defaultRetentionPolicy value [" + defaultRetentionPolicy + "]");
                }

                // Create a POST web request
                string uri = GetDatasetsUri(group) + String.Format("?defaultRetentionPolicy={0}", defaultRetentionPolicy);
                HttpWebRequest request = CreateRequest(uri, "POST", GetAccessToken(), jsonContent);

                // Get HttpWebResponse from the POST request
                string responseContent = GetResponse(request);

                // Deserialize JSON response into objects
                Dataset response = JsonConvert.DeserializeObject<Dataset>(responseContent);

                Console.WriteLine("> Succeeded\r\n>");

                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine("> Error: " + ex.Message);
                Console.WriteLine(">");

                return null;
            }
        }

        // If group is null, then retrieve datasets from the user's workspace
        // Outputs a JSON response describing all tables for a given dataset
        public static Table[] GetTables(Group group, Dataset dataset)
        {
            Console.WriteLine("> Calling GetTables()");

            try
            {
                // Create a GET web request
                HttpWebRequest request = CreateRequest(GetTablesUri(group, dataset), "GET", GetAccessToken());

                // Get HttpWebResponse from the GET request
                string responseContent = GetResponse(request);

                // Deserialize JSON response into objects
                GetTablesResponse response = JsonConvert.DeserializeObject<GetTablesResponse>(responseContent);

                Console.WriteLine("> Succeeded\r\n>");

                return response.value;
            }
            catch (Exception ex)
            {
                Console.WriteLine("> Error: " + ex.Message);
                Console.WriteLine(">");

                return null;
            }
        }

        // If group is null, then retrieve datasets from the user's workspace
        // Outputs status
        public static bool UpdateTableSchema(Group group, Dataset dataset, Table table, string jsonContent)
        {
            Console.WriteLine("> Calling UpdateTableSchema()");

            try
            {
                // Create a PUT web request
                HttpWebRequest request = CreateRequest(GetTableUri(group, dataset, table), "PUT", GetAccessToken(), jsonContent);

                // Get HttpWebResponse from the PUT request
                string responseContent = GetResponse(request);

                Console.WriteLine("> Succeeded\r\n>");

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("> Error: " + ex.Message);
                Console.WriteLine(">");

                return false;
            }
        }

        // If group is null, then retrieve datasets from the user's workspace
        // Outputs status
        public static bool AddTableRows(Group group, Dataset dataset, Table table, string jsonContent)
        {
            Console.WriteLine("> Calling AddTableRows()");

            try
            {
                // Create a POST web request
                HttpWebRequest request = CreateRequest(GetTableRowsUri(group, dataset, table), "POST", GetAccessToken(), jsonContent);

                // Get HttpWebResponse from the POST request
                string responseContent = GetResponse(request);

                Console.WriteLine("> Succeeded\r\n>");

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("> Error: " + ex.Message);
                Console.WriteLine(">");

                return false;
            }
        }

        // If group is null, then retrieve datasets from the user's workspace
        // Outputs a JSON response describing the new table name
        public static bool ClearTableRows(Group group, Dataset dataset, Table table)
        {
            Console.WriteLine("> Calling ClearTableRows()");

            try
            {
                // Create a DELETE web request
                HttpWebRequest request = CreateRequest(GetTableRowsUri(group, dataset, table), "DELETE", GetAccessToken());

                // Get HttpWebResponse from the DELETE request
                string responseContent = GetResponse(request);

                Console.WriteLine("> Succeeded\r\n>");

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("> Error: " + ex.Message);
                Console.WriteLine(">");

                return false;
            }
        }

        // If group is null, then retrieve dasboards from the user's workspace
        // Outputs a JSON response describing all dasboards
        public static Dashboard[] GetDashboards(Group group)
        {
            Console.WriteLine("> Calling GetDashboards()");

            try
            {
                // Create a GET web request
                HttpWebRequest request = CreateRequest(GetDashboardsUri(group), "GET", GetAccessToken());

                // Get HttpWebResponse from GET request
                string responseContent = GetResponse(request);

                // Deserialize JSON response into objects
                GetDashboardsResponse response = JsonConvert.DeserializeObject<GetDashboardsResponse>(responseContent);

                Console.WriteLine("> Succeeded\r\n>");

                return response.value;
            }
            catch (Exception ex)
            {
                Console.WriteLine("> Error: " + ex.Message);
                Console.WriteLine(">");

                return null;
            }
        }

        // If group is null, then retrieve tiles from the user's workspace
        // Outputs a JSON response describing all tiles for a given dashboard
        public static Tile[] GetTiles(Group group, Dashboard dashboard)
        {
            Console.WriteLine("> Calling GetTiles()");

            try
            {
                // Create a GET web request
                HttpWebRequest request = CreateRequest(GetTilesUri(group, dashboard), "GET", GetAccessToken());

                // Get HttpWebResponse from the GET request
                string responseContent = GetResponse(request);

                // Deserialize JSON response into objects
                GetTilesResponse response = JsonConvert.DeserializeObject<GetTilesResponse>(responseContent);

                Console.WriteLine("> Succeeded\r\n>");

                return response.value;
            }
            catch (Exception ex)
            {
                Console.WriteLine("> Error: " + ex.Message);
                Console.WriteLine(">");

                return null;
            }
        }

        private static string GetGroupsUri()
        {
            return powerBiApiUri + "/groups";
        }

        private static string GetDatasetsUri(Group group)
        {
            return powerBiApiUri + ((group == null) ? String.Empty : String.Format("/groups/{0}", group.id)) + "/datasets";
        }

        private static string GetTablesUri(Group group, Dataset dataset)
        {
            return GetDatasetsUri(group) + String.Format("/{0}/tables", dataset.id);
        }

        private static string GetTableUri(Group group, Dataset dataset, Table table)
        {
            return GetTablesUri(group, dataset) + String.Format("/{0}", table.name);
        }

        private static string GetTableRowsUri(Group group, Dataset dataset, Table table)
        {
            return GetTableUri(group, dataset, table) + "/rows";
        }

        private static string GetDashboardsUri(Group group)
        {
            // TODO: Remove the beta reference when method is available in production
            Console.WriteLine("> WARNING: This method is currently in beta");
            return powerBiApiUri.Replace("v1.0", "beta") + ((group == null) ? String.Empty : String.Format("/groups/{0}", group.id)) + "/dashboards";
        }

        private static string GetTilesUri(Group group, Dashboard dashboard)
        {
            return GetDashboardsUri(group) + String.Format("/{0}/tiles", dashboard.id);
        }

        private static HttpWebRequest CreateRequest(string uri, string method, string accessToken)
        {
            HttpWebRequest request = WebRequest.Create(uri) as HttpWebRequest;

            request.KeepAlive = true;
            request.Method = method;
            request.ContentLength = 0;
            request.ContentType = "application/json";

            // Add access token to the header
            request.Headers.Add("Authorization", String.Format("Bearer {0}", accessToken));

            return request;
        }

        private static HttpWebRequest CreateRequest(string uri, string method, string accessToken, string jsonContent)
        {
            HttpWebRequest request = CreateRequest(uri, method, accessToken);

            byte[] byteArray = Encoding.UTF8.GetBytes(jsonContent);
            request.ContentLength = byteArray.Length;

            // Write JSON byte[] into a stream
            using (Stream writer = request.GetRequestStream())
            {
                writer.Write(byteArray, 0, byteArray.Length);
            }

            return request;
        }

        private static string GetResponse(HttpWebRequest request)
        {
            string response = string.Empty;

            Console.WriteLine(String.Format("> {0} request sent to {1}", request.Method.ToUpper(), request.RequestUri));

            if (request.ContentLength > 0)
            {
                Console.WriteLine("> JSON document added to request body");
            }

            using (HttpWebResponse httpResponse = request.GetResponse() as System.Net.HttpWebResponse)
            {
                // Retrieve a StreamReader that holds the response stream
                using (StreamReader reader = new System.IO.StreamReader(httpResponse.GetResponseStream()))
                {
                    response = reader.ReadToEnd();
                }
            }

            return response;
        }
    }

    #region Supporting Classes
    public class GetDatasetsResponse
    {
        public string odatacontext { get; set; }
        public Dataset[] value { get; set; }
    }

    public class Dataset
    {
        public string id { get; set; }
        public string name { get; set; }
        //public string defaultRetentionPolicy { get; set; }
    }

    public class CreateDatasetResponse
    {
        public string odatacontext { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string defaultRetentionPolicy { get; set; }
    }

    public class GetTablesResponse
    {
        public string odatacontext { get; set; }
        public Table[] value { get; set; }
    }

    public class Table
    {
        public string name { get; set; }
    }

    public class GetGroupsResponse
    {
        public string odatacontext { get; set; }
        public Group[] value { get; set; }
    }

    public class Group
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    public class GetDashboardsResponse
    {
        public string odatacontext { get; set; }
        public Dashboard[] value { get; set; }
    }

    public class Dashboard
    {
        public string id { get; set; }
        public string displayName { get; set; }
        public bool isReadOnly { get; set; }
    }

    public class GetTilesResponse
    {
        public string odatacontext { get; set; }
        public Tile[] value { get; set; }
    }

    public class Tile
    {
        public string id { get; set; }
        public string title { get; set; }
        public string subTitle { get; set; }
        public string embedUrl { get; set; }
    }
    #endregion
}
