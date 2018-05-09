'use strict';

// !!! BEFORE YOU RUN THIS SAMPLE APP !!!
// --------------------------------------

// update the GUID for the client_id variable 
// in the initializeADALSettings function
var client_id = "12d9addd-a8a5-48cf-a3c2-205d8ce5b418";


(function () {

  var app = angular.module("PowerBIContentViewer", ['ngRoute', 'AdalAngular']);

  app.config(['$routeProvider', '$httpProvider', 'adalAuthenticationServiceProvider', initializeApp]);

  function initializeApp($routeProvider, $httpProvider, adalProvider) {
    initializeADALSettings($httpProvider, adalProvider);
    initializeRouteMap($routeProvider);
  }

  function initializeADALSettings($httpProvider, adalProvider) {

    var endpoints = {
      "https://graph.windows.net/": "https://graph.windows.net/",
      "https://api.powerbi.com/v1.0/": "https://analysis.windows.net/powerbi/api",
      "https://api.powerbi.com/beta/": "https://analysis.windows.net/powerbi/api",
    };

    var adalProviderSettings = {
      tenant: 'common',
      clientId: client_id,
      extraQueryParameter: 'nux=1',
      endpoints: endpoints,
      cacheLocation: 'localStorage' // enable this for IE, as sessionStorage does not work for localhost.
    };

    adalProvider.init(adalProviderSettings, $httpProvider);
   
  }

  function initializeRouteMap($routeProvider) {

    // config app's route map
    $routeProvider
      .when("/",
           { templateUrl: 'App/views/home.html', controller: "homeController" })
      .when("/datasets",
           { templateUrl: 'App/views/datasets.html', controller: "datasetsController", requireADLogin: true })
      .when("/dashboards",
           { templateUrl: 'App/views/dashboards.html', controller: "dashboardsController", requireADLogin: true })
      .when("/reports",
           { templateUrl: 'App/views/reports.html', controller: "reportsController", requireADLogin: true })
      .when("/userClaims",
           { templateUrl: 'App/views/userClaims.html', controller: "userClaimsController", requireADLogin: true })
      .otherwise({ redirectTo: "/" });
  }

})();