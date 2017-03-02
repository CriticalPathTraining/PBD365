/// <reference path="C:\Demo\PowerBiContentViewer\PowerBiContentViewer\Scripts/adal-angular.js" />


'use strict';

(function () {

  var app = angular.module('PowerBIContentViewer');

  app.factory("PowerBiService", ['$http', 'adalAuthenticationService', createServiceObject]);

  function createServiceObject($http, adalAuthenticationService) {

    // create service object
    var service = {};

    var apiRoot = "https://api.powerbi.com/v1.0/";
    var apiRootBeta = "https://api.powerbi.com/beta/";
    var tenancyId = adalAuthenticationService.userInfo.profile.tid;
    $http.defaults.useXDomain = true;

    // set default headers for $http service
    var config = {
      headers: {
        'Accept': 'application/json; odata.metadata=none',
      }
    };

    service.getAccessToken = function () {
      return adalAuthenticationService.acquireToken("https://analysis.windows.net/powerbi/api");
    }

    service.getWorkspaces = function () {
      var restUrl = apiRoot + tenancyId + "/groups/" + queryString;
      return $http.get(restUrl);
    };

    
    service.getDatasets = function () {
      var restUrl = apiRoot + "myOrg/Datasets/";
      return $http.get(restUrl);
    };

    service.getDashboards = function () {
      var restUrl = apiRootBeta + "myOrg/Dashboards/";
      return $http.get(restUrl);
    };

    service.getDashboardTiles = function (dashboardId) {
      var restUrl = apiRootBeta + "myOrg/Dashboards/" + dashboardId + "/tiles/";
      return $http.get(restUrl);
    };


    service.getReports = function () {
      var restUrl = apiRootBeta + "myOrg/Reports/";
      return $http.get(restUrl);
    };

    // return service object to angular framework
    return service;
  }

})();

