'use strict';

var app = angular.module('AzureGraphWebClientSPA');

app.controller('tenancyController', ['$scope', 'azureGraphApiService', tenancyController]);

function tenancyController($scope, azureGraphApiService) {

  azureGraphApiService.getTenantDetails().success(function (data) {
    $scope.tenantDetails = data.value[0];
  }).
  error(function (data, status, headers, config) {
    alert("Error getting tenant details");
  });


}