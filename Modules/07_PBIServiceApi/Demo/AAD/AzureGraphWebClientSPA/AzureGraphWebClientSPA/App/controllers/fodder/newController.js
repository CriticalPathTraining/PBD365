'use strict';

(function () {

  var app = angular.module('AzureGraphWebClientSPA');

  app.controller('newController', newController);

  function newController($scope, $location, wingtipCrmService) {

    $scope.customer = {};
    $scope.customer.FirstName = "";
    $scope.customer.Title = "";
    $scope.customer.WorkPhone = "";
    $scope.customer.HomePhone = "";
    $scope.customer.Email = "";

    $scope.addCustomer = function () {
      var firstName = $scope.customer.FirstName;
      var lastName = $scope.customer.Title;
      var workPhone = $scope.customer.WorkPhone;
      var homePhone = $scope.customer.HomePhone;
      var email = $scope.customer.Email;
      wingtipCrmService.addCustomer(firstName, lastName, workPhone, homePhone, email)
        .success(function (data) {
          $location.path("/");
        });
    }
  }

})();