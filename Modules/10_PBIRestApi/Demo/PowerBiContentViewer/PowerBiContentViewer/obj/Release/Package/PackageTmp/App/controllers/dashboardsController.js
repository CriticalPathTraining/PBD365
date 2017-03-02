'use strict';

var app = angular.module('PowerBIContentViewer');

app.controller('dashboardsController', ['$scope', '$sce', 'PowerBiService', dashboardsController]);

function dashboardsController($scope, $sce, PowerBiService) {

  PowerBiService.getDashboards().success(function (data) {
    $scope.dashboards = data.value;
    $scope.displayTiles = function (dashboardId) {
      PowerBiService.getDashboardTiles(dashboardId).success(function (data) {
        var tiles = data.value;
        for (var index = 0; index < tiles.length-1 ; index++) {
          tiles[index].embedUrl = $sce.trustAsResourceUrl(tiles[index].embedUrl)
        }
        $scope.tiles = tiles;
      });
    };

    $scope.PowerBiDashboardTileOnLoad = function () {

      var tileId = this.tile.id;

      PowerBiService.getAccessToken().then(function (data) {

        var accessToken = data;

        var message = JSON.stringify({
          action: "loadTile",
          accessToken: accessToken,
          width: 420,
          height: 240
        });

        $("#" + tileId).get(0).contentWindow.postMessage(message, "*");

      });

    }


  }).
  error(function(data, status, headers, config) {
    alert("Error getting dashboards");
  });

}