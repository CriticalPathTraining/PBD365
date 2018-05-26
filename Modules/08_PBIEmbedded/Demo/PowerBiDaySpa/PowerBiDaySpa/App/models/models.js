var myApp;
(function (myApp) {
    // data required for embedding a report
    var Report = (function () {
        function Report() {
        }
        return Report;
    }());
    myApp.Report = Report;
    // data required for embedding a new report
    var NewReport = (function () {
        function NewReport() {
        }
        return NewReport;
    }());
    myApp.NewReport = NewReport;
    // data required for embedding a dashboard
    var Dashboard = (function () {
        function Dashboard() {
        }
        return Dashboard;
    }());
    myApp.Dashboard = Dashboard;
    var Dataset = (function () {
        function Dataset() {
        }
        return Dataset;
    }());
    myApp.Dataset = Dataset;
    // data required for embedding a dashboard
    var Qna = (function () {
        function Qna() {
        }
        return Qna;
    }());
    myApp.Qna = Qna;
})(myApp || (myApp = {}));
//# sourceMappingURL=models.js.map