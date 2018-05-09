
using System;
using System.Configuration;
using System.Text;

using AzureStreamingAnalyticsDemo.Models;
using Microsoft.Azure.EventHubs;

namespace AzureStreamingAnalyticsDemo {

  class Program {

    private static EventHubClient eventHubClient;
    private static readonly string EventHubConnectionString = ConfigurationManager.AppSettings["EventHubConnectionString"];
    private static readonly string EventHubEntityPath = ConfigurationManager.AppSettings["EventHubName"];

    static void Main() {

      var connectionStringBuilder = 
        new EventHubsConnectionStringBuilder(EventHubConnectionString) {
          EntityPath = EventHubEntityPath
        };

      eventHubClient = EventHubClient.CreateFromConnectionString(connectionStringBuilder.ToString());

      var boiler1 = new Thermometer {
        DeviceName = "Boiler 01"
      };

      while (true) {
        var eventDataJson = boiler1.GetTemperatureMessage();
        Console.WriteLine("Sending message: " + eventDataJson);
        var eventData = new EventData(Encoding.UTF8.GetBytes(eventDataJson));
        eventHubClient.SendAsync(eventData).Wait();
        System.Threading.Thread.Sleep(500);
      }

    }
  }
}
