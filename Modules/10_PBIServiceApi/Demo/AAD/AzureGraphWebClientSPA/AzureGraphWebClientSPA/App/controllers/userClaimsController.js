'use strict';

var app = angular.module('AzureGraphWebClientSPA')

app.controller('userClaimsController', ['$scope', 'adalAuthenticationService', '$location', userClaimsController]);

function userClaimsController($scope, adalAuthenticationService, $location) {

  // create array to track claims for logged-on user
  $scope.claims = [];

  // add claims for id_token into claims array
  for (var property in adalAuthenticationService.userInfo.profile) {
    if (adalAuthenticationService.userInfo.profile.hasOwnProperty(property)) {
      $scope.claims.push({
        key: property,
        value: adalAuthenticationService.userInfo.profile[property],
      });
    }
  }

}