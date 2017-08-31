/// <reference path="C:\Demo\PowerBiContentViewer\PowerBiContentViewer\Scripts/angular.js" />

'use strict';

var app = angular.module('PowerBIContentViewer');




app.directive('powerBiReportLoader', ['$scope', 'PowerBiService', dashboardsController]);

function dashboardsController($scope, PowerBiService) {

  PowerBiService.getDashboards().success(function (data) {
    $scope.dashboards = data.value;
    $scope.displayTiles = function (dashboardId) {
      PowerBiService.getDashboardTiles(dashboardId).success(function (data) {
        $scope.tiles = data.value;
      });
    };

  }).
  error(function (data, status, headers, config) {
    alert("Error getting dashboards");
  });

}