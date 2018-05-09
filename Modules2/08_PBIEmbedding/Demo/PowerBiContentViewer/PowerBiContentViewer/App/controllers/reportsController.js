'use strict';

var app = angular.module('PowerBIContentViewer');

app.controller('reportsController', ['$scope', '$sce', 'PowerBiService', reportsController]);

function reportsController($scope, $sce, PowerBiService) {

  PowerBiService.getReports().success(function (data) {
    $scope.reports = data.value;
  
    $scope.setEmbedUrl = function (url) {
        $scope.embedUrl = $sce.trustAsResourceUrl(url);
    }

    $scope.PowerBiReportOnLoad = function () {

      PowerBiService.getAccessToken().then(function (data) {

        var accessToken = data;

        var message = JSON.stringify({
          action: "loadReport",
          accessToken: accessToken,
          height: 600,
          width: 600
        });

        $("#reportFrame").get(0).contentWindow.postMessage(message, "*");

      });
            
    }

  }).
  error(function (data, status, headers, config) {
    alert("Error getting reports");
  });

}