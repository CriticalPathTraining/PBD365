'use strict';

(function () {

  var app = angular.module('AzureGraphWebClientSPA');

  app.controller('viewController', viewController);

  function viewController($scope, $routeParams, wingtipCrmService) {
    var id = $routeParams.id;
    wingtipCrmService.getCustomer(id).success(function (data) {
      $scope.customer = data.d;
    });
  }

})();