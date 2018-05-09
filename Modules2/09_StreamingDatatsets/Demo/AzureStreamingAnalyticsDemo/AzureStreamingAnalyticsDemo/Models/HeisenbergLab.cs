using Newtonsoft.Json;
using System;

namespace AzureStreamingAnalyticsDemo.Models {

  class TemperatureReading {
    public string DeviceName;
    public DateTime TimeStamp;
    public double Temperature;
    public double MinTempterature;
    public double MaxTempterature;
    public double TargetTempterature;
  }

  class Thermometer {
    public string DeviceName;
    public double CurrentTemperaure = 100;
    public double MinTemperature = 100;
    public double MaxTemperature = 250;
    public double TargetTemperature = 212;
    public bool TempOnTheRise = true;
    public bool TempInTransition = false;
    public int TransitionCounter = 0;

    private Random random = new Random();

    private double GetNextTemperature() {

      if (TempInTransition) {
        TransitionCounter += 1;
        int transitionCountMax = TempOnTheRise ? 15 : 3;
        if (TransitionCounter >= transitionCountMax) {
          TempInTransition = false;
          TransitionCounter = 0;
        }
      }
      else {
        if (TempOnTheRise) {
          CurrentTemperaure += random.Next(-40, 380) / (double)100;
          if (CurrentTemperaure > TargetTemperature) {
            CurrentTemperaure = TargetTemperature;
          }
          if (CurrentTemperaure == TargetTemperature) {
            TempOnTheRise = false;
            TempInTransition = true;
          }
        }
        else {
          CurrentTemperaure -= random.Next(0, 1020) / (double)100;
          if (CurrentTemperaure < MinTemperature) {
            CurrentTemperaure = MinTemperature;
          }
          if(CurrentTemperaure == MinTemperature) {
            TempOnTheRise = true;
            TransitionCounter = 0;

          }
        }
      }

      return CurrentTemperaure;
    }

    public string GetTemperatureMessage() {
      return JsonConvert.SerializeObject(
        new TemperatureReading {
          DeviceName = this.DeviceName,
          TimeStamp = DateTime.UtcNow,
          Temperature = GetNextTemperature(),
          MinTempterature = this.MinTemperature,
          MaxTempterature = this.MaxTemperature,
          TargetTempterature = this.TargetTemperature
        });
    }
  }

  class HeisenbergLab {
  }


}
