'use strict';

(function () {

  var app = angular.module('AzureGraphWebClientSPA');

  app.controller('navbarController', ['$scope', 'adalAuthenticationService', '$location', navbarController]);

  function navbarController($scope, adalAuthenticationService, $location) {
   
    $scope.login = function () {
      adalAuthenticationService.login();
    };

    $scope.logout = function () {
      adalAuthenticationService.logOut();
    };

    $scope.$on("adal:loginSuccess", function () {
      // add code here to respond to successful user login event
    });

  }

})();
