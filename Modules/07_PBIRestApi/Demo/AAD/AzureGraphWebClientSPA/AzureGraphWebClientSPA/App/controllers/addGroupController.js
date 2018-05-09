'use strict';




var app = angular.module('AzureGraphWebClientSPA');

app.controller('addGroupController', ['$scope', '$location', 'azureGraphApiService', addGroupController]);

function addGroupController($scope, $location, unifiedApiService) {

  $scope.group = {
    GroupName: ""
  };

  $scope.actions = {
    addGroup: function () {
      unifiedApiService.addGroup($scope.group.GroupName)
        .success(function (data) {
          $location.path("/groups");
        })
        .error(function (data, status, headers, config) {
          alert("Error add group");
        });
    }
  }

}