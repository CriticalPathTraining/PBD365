﻿/// <reference path="jquery-3.1.1.js" />
/// <reference path="powerbi.js" />

$(function () {

  var reportConfig = {
    settings: {
      filterPaneEnabled: false,
      navContentPaneEnabled: false
    }
  };

  var reportContainer = document.getElementById("pbi-report");
  var report = powerbi.embed(reportContainer, reportConfig);

  var pages = [];
  var pageIndex = 0;
  var currentPage = null;

  report.on('loaded', onReportLoaded);
  report.on('pageChanged', onReportPageChanged);

  function onReportLoaded() {
    report.getPages().then(
        function (reportPages) {
          // assign report pages collection to pages array
          pages = reportPages;
          // initialize UI to radio button for each report page
          var pageNavigation = $("#page-navigation");
          pageNavigation.empty();
          for (var index = 0; index < pages.length; index++) {
            pageNavigation.append(
              $("<div>")
                .append($('<input type="radio" name="selected-page">')
                .val(pages[index].displayName))
                .click(function (e) {
                  pages.find(function (page) { return page.displayName === e.target.value }).setActive();
                })
            .append(pages[index].displayName));
          }
        })
  }

  function onReportPageChanged(e) {
    currentPage = e.detail.newPage;
    var selectedPage = $("input[name=selected-page]");
    selectedPage.val([e.detail.newPage.displayName]);

    if (pages.length === 0) {
      return;
    }

    pageIndex = pages.findIndex(function (el) {
      return el.name === e.detail.newPage.name;
    });
  }

  $('#pbi-prev-page').on('click', function () { changePage(-1); });
  $('#pbi-next-page').on('click', function () { changePage(1); });

  function changePage(direction) {
    var nextPageIndex = pageIndex + direction;
    if (nextPageIndex < 0) nextPageIndex = pages.length - 1;
    if (nextPageIndex >= pages.length) nextPageIndex = 0;
    pages[nextPageIndex].setActive();
  }
  
  $('#setting-shownav').on('change', function (e) { updateSetting(e, 'navContentPaneEnabled'); });
  $('#setting-showfilterpane').on('change', function (e) { updateSetting(e, 'filterPaneEnabled'); });

  function updateSetting(e, settingName) {
    var settings = {};
    settings[settingName] = e.target.checked;
    report.updateSettings(settings);
  }

});