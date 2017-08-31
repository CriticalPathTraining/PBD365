'use strict';

(function () {

  var app = angular.module('AzureGraphWebClientSPA');

  app.factory("azureGraphApiService", ['$http', 'adalAuthenticationService', createServiceObject]);

  function createServiceObject($http, adalAuthenticationService) {

    // create service object
    var service = {};

    var apiRoot = "https://graph.windows.net/";
    var queryString = "?api-version=1.5";
    var tenancyId = adalAuthenticationService.userInfo.profile.tid;

    $http.defaults.useXDomain = true;

    // set default headers for $http service
    var config = {
      headers: {
        'Accept': 'application/json; odata.metadata=none',
      }
    };

    
    service.getTenantDetails = function () {
      var restUrl = apiRoot + tenancyId + "/tenantDetails/" + queryString;
      return $http.get(restUrl);
    };

    service.getUsers = function () {
      var restUrl = apiRoot + tenancyId + "/users/" + queryString;
      return $http.get(restUrl);
    };

    service.getGroups = function () {
      var restUrl = apiRoot + tenancyId + "/groups/" + queryString;
      return $http.get(restUrl);
    };
   
    // return service object to angular framework
    return service;
  }

})();

