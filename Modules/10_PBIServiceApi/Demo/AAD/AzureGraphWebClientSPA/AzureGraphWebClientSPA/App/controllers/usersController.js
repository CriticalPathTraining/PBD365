'use strict';

var app = angular.module('AzureGraphWebClientSPA');

app.controller('usersController', ['$scope', 'azureGraphApiService', usersController]);

function usersController($scope, azureGraphApiService) {

  azureGraphApiService.getUsers().success(function (data) {
    $scope.users = data.value;
  }).
  error(function(data, status, headers, config) {
    alert("Error getting users");
  });


}