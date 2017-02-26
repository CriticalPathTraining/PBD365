var myApp;
(function (myApp) {
    var visuals = myApp.visuals;
    $(function () {
        var viz = new visuals.Viz01();
        viz.create("#viz");
    });
})(myApp || (myApp = {}));
