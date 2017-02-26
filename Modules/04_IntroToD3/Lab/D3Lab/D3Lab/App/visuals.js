/// <reference path="../scripts/typings/d3/d3.d.ts" />
var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var myApp;
(function (myApp) {
    var visuals;
    (function (visuals) {
        var CustomVisualBase = (function () {
            function CustomVisualBase() {
            }
            CustomVisualBase.prototype.create = function (containerId) {
                var _this = this;
                this.container = d3.select(containerId);
                this.init(this.container);
                var windowPaddingBottom = 4;
                var viewport = {
                    width: $(window).width(),
                    height: $(window).height() - windowPaddingBottom
                };
                this.update(viewport);
                $(window).resize(function () {
                    var windowPaddingBottom = 4;
                    var viewport = {
                        width: $(window).width(),
                        height: $(window).height() - windowPaddingBottom
                    };
                    _this.update(viewport);
                });
            };
            CustomVisualBase.prototype.init = function (container) {
            };
            CustomVisualBase.prototype.update = function (viewport) { };
            return CustomVisualBase;
        }());
        visuals.CustomVisualBase = CustomVisualBase;
        var Viz01 = (function (_super) {
            __extends(Viz01, _super);
            function Viz01() {
                var _this = _super !== null && _super.apply(this, arguments) || this;
                _this.dataset = [2, 7, 6, 8, 5, 8];
                return _this;
            }
            Viz01.prototype.init = function (container) {
                this.svgRoot = container.append("svg");
                ;
            };
            Viz01.prototype.update = function (viewport) {
                var _this = this;
                this.svgRoot.attr({
                    "width": viewport.width,
                    "height": viewport.height
                });
                var padding = 8;
                var plot = {
                    width: viewport.width - (padding * 2),
                    height: viewport.height - (padding * 2)
                };
                var barSpacing = 4;
                var barWidth = ((plot.width - barSpacing) / this.dataset.length) - barSpacing;
                var yScale = function (yInput) { return (plot.height * yInput) / d3.max(_this.dataset); };
                var rects = this.svgRoot.selectAll("rect");
                if (rects.empty()) {
                    rects = this.svgRoot.selectAll("rect").data(this.dataset).enter().append("rect");
                }
                rects.transition().duration(500).delay(200)
                    .attr({
                    x: function (d, i) { return padding + barSpacing + (i * (plot.width / _this.dataset.length)); },
                    y: function (d, i) {
                        return padding + plot.height - (yScale(d));
                    },
                    width: barWidth,
                    height: function (d, i) { return yScale(d); },
                    fill: "teal"
                });
            };
            return Viz01;
        }(CustomVisualBase));
        visuals.Viz01 = Viz01;
        var Viz02 = (function (_super) {
            __extends(Viz02, _super);
            function Viz02() {
                var _this = _super !== null && _super.apply(this, arguments) || this;
                _this.dataset = [32, 31, 29, 30, 32, 33, 35, 57, 40, 41, 38, 36, 22, 24, 26, 28, 28, 29, 27, 26, 23, 23, 21];
                return _this;
            }
            Viz02.prototype.init = function (container) {
                this.svgRoot = container.append("svg");
                ;
            };
            Viz02.prototype.update = function (viewport) {
                this.svgRoot.selectAll("*").remove();
                this.svgRoot.attr({
                    "width": viewport.width,
                    "height": viewport.height
                });
                var paddingOuter = 8;
                var paddingInner = 4;
                var plot = {
                    width: viewport.width - (paddingOuter * 2),
                    height: viewport.height - (paddingOuter * 2)
                };
                var gPlot = this.svgRoot.append("g")
                    .attr("transform", "translate(" + paddingOuter + ", " + paddingOuter + ")");
                gPlot.append("rect")
                    .attr("width", plot.width)
                    .attr("height", plot.height)
                    .classed("plot-area", true);
                var yScale = d3.scale.linear()
                    .domain([0, d3.max(this.dataset)])
                    .range([0, plot.height - (paddingInner * 2)]);
                var xScale = d3.scale.linear()
                    .domain([0, this.dataset.length - 1])
                    .range([0, plot.width - (paddingInner * 2)]);
                var line = d3.svg.line()
                    .x(function (d, i) { return paddingInner + xScale(i); })
                    .y(function (d, i) { return paddingInner + plot.height - yScale(d); });
                gPlot.append("path")
                    .attr("d", line(this.dataset));
            };
            return Viz02;
        }(CustomVisualBase));
        visuals.Viz02 = Viz02;
        var Viz03 = (function (_super) {
            __extends(Viz03, _super);
            function Viz03() {
                var _this = _super !== null && _super.apply(this, arguments) || this;
                _this.dataset = [32, 31, 29, 30, 32, 33, 35, 57, 40, 41, 38, 36, 22, 24, 26, 28, 28, 29, 27, 26, 23, 23, 21];
                return _this;
            }
            Viz03.prototype.init = function (container) {
                this.svgRoot = container.append("svg");
                ;
            };
            Viz03.prototype.update = function (viewport) {
                this.svgRoot.selectAll("*").remove();
                this.svgRoot.attr({
                    "width": viewport.width,
                    "height": viewport.height
                });
                var paddingOuter = 8;
                var paddingInner = 4;
                var plot = {
                    width: viewport.width - (paddingOuter * 2),
                    height: viewport.height - (paddingOuter * 2)
                };
                var gPlot = this.svgRoot.append("g")
                    .attr("transform", "translate(" + paddingOuter + ", " + paddingOuter + ")");
                gPlot.append("rect")
                    .attr("width", plot.width)
                    .attr("height", plot.height)
                    .classed("plot-area", true);
                var yScale = d3.scale.linear()
                    .domain([0, d3.max(this.dataset)])
                    .range([0, plot.height - (paddingInner * 2)]);
                var xScale = d3.scale.linear()
                    .domain([0, this.dataset.length - 1])
                    .range([0, plot.width - (paddingInner * 2)]);
                var area = d3.svg.area()
                    .x(function (d, i) { return paddingInner + xScale(i); })
                    .y1(function (d, i) { return paddingInner + plot.height - yScale(d); })
                    .y0(plot.height - paddingInner);
                gPlot.append("path")
                    .attr("d", area(this.dataset))
                    .classed("area", true);
            };
            return Viz03;
        }(CustomVisualBase));
        visuals.Viz03 = Viz03;
        var Viz04 = (function (_super) {
            __extends(Viz04, _super);
            function Viz04() {
                var _this = _super !== null && _super.apply(this, arguments) || this;
                _this.dataset = [32, 31, 29, 30, 32, 33, 35, 57, 40, 41, 38, 36, 22, 24, 26, 28, 28, 29, 27, 26, 23, 23, 21];
                return _this;
            }
            Viz04.prototype.init = function (container) {
                this.svgRoot = container.append("svg");
                ;
            };
            Viz04.prototype.update = function (viewport) {
                this.svgRoot.selectAll("*").remove();
                this.svgRoot.attr({
                    "width": viewport.width,
                    "height": viewport.height
                });
                var yAxisOffset = 28;
                var paddingOuter = 8;
                var paddingInner = 4;
                var plot = {
                    width: viewport.width - paddingOuter - yAxisOffset,
                    height: viewport.height - (paddingOuter * 2)
                };
                var gPlot = this.svgRoot.append("g")
                    .attr("transform", "translate(" + yAxisOffset + ", " + paddingOuter + ")");
                gPlot.append("rect")
                    .attr("width", plot.width)
                    .attr("height", plot.height)
                    .classed("plot-area", true);
                var yScale = d3.scale.linear()
                    .domain([0, d3.max(this.dataset)])
                    .range([plot.height - (paddingInner * 2), 0]);
                var yAxis = d3.svg.axis()
                    .scale(yScale)
                    .orient("left")
                    .ticks(5);
                this.svgRoot.append("g")
                    .attr("class", "axis")
                    .attr("transform", "translate( " + yAxisOffset + ", " + (paddingOuter + paddingInner) + ")")
                    .call(yAxis);
                var xScale = d3.scale.linear()
                    .domain([0, this.dataset.length - 1])
                    .range([0, plot.width - (paddingInner * 2)]);
                var area = d3.svg.area()
                    .x(function (d, i) { return paddingInner + xScale(i); })
                    .y1(function (d, i) { return paddingInner + yScale(d); })
                    .y0(plot.height - paddingInner);
                gPlot.append("path")
                    .attr("d", area(this.dataset))
                    .classed("area", true);
            };
            return Viz04;
        }(CustomVisualBase));
        visuals.Viz04 = Viz04;
        var Viz05 = (function (_super) {
            __extends(Viz05, _super);
            function Viz05() {
                var _this = _super !== null && _super.apply(this, arguments) || this;
                _this.dataset = [21, 26, 16, 32];
                return _this;
            }
            Viz05.prototype.init = function (container) {
                this.svgRoot = container.append("svg");
                ;
            };
            Viz05.prototype.update = function (viewport) {
                this.svgRoot.selectAll("*").remove();
                this.svgRoot.attr({
                    "width": viewport.width,
                    "height": viewport.height
                });
                var padding = 8;
                var plot = {
                    width: viewport.width - (padding * 2),
                    height: viewport.height - (padding * 2)
                };
                var outerRadius = d3.min([plot.width / 2, plot.height / 2]);
                var innerRadius = outerRadius / 3;
                var arc = d3.svg.arc()
                    .innerRadius(innerRadius)
                    .outerRadius(outerRadius);
                var pie = d3.layout.pie();
                var arcs = this.svgRoot.selectAll("arc")
                    .data(pie(this.dataset))
                    .enter()
                    .append("g")
                    .attr("class", "arc")
                    .attr("transform", "translate(" + (outerRadius + padding) + ", " + (outerRadius + padding) + ")");
                var color = ["red", "green", "blue", "purple", "Yellow"];
                arcs.append("path")
                    .attr("fill", function (d, i) { return color[i]; })
                    .attr("d", arc);
            };
            return Viz05;
        }(CustomVisualBase));
        visuals.Viz05 = Viz05;
    })(visuals = myApp.visuals || (myApp.visuals = {}));
})(myApp || (myApp = {}));
