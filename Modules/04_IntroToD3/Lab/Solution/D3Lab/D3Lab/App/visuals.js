var myApp;
(function (myApp) {
    var Viz01 = (function () {
        function Viz01() {
            this.name = "Visual 1: Hello jQuery";
        }
        Viz01.prototype.load = function (container) {
            this.container = $(container);
            this.message = $("<div>")
                .text("Hello jQuery")
                .css({
                "display": "table-cell",
                "text-align": "center",
                "vertical-align": "middle",
                "text-wrap": "none",
                "background-color": "yellow"
            });
            this.container.append(this.message);
        };
        Viz01.prototype.update = function (viewport) {
            var paddingX = 2;
            var paddingY = 2;
            var fontSizeMultiplierX = viewport.width * 0.15;
            var fontSizeMultiplierY = viewport.height * 0.4;
            var fontSizeMultiplier = Math.min.apply(Math, [fontSizeMultiplierX, fontSizeMultiplierY]);
            this.message.css({
                "width": viewport.width - paddingX,
                "height": viewport.height - paddingY,
                "font-size": fontSizeMultiplier
            });
        };
        return Viz01;
    }());
    myApp.Viz01 = Viz01;
    var Viz02 = (function () {
        function Viz02() {
            this.name = "Visual 2: Hello D3";
            this.padding = 20;
        }
        Viz02.prototype.load = function (container) {
            this.svgRoot = d3.select(container).append("svg");
            this.ellipse = this.svgRoot.append("ellipse")
                .style("fill", "rgba(255, 255, 0, 0.5)")
                .style("stroke", "rgba(0, 0, 0, 1.0)")
                .style("stroke-width", "4");
            this.text = this.svgRoot.append("text")
                .text("Hello D3")
                .attr("text-anchor", "middle")
                .attr("dominant-baseline", "central")
                .style("fill", "rgba(255, 0, 0, 1.0)")
                .style("stroke", "rgba(0, 0, 0, 1.0)")
                .style("stroke-width", "2");
        };
        Viz02.prototype.update = function (viewport) {
            this.svgRoot
                .attr("width", viewport.width)
                .attr("height", viewport.height);
            var plot = {
                xOffset: this.padding,
                yOffset: this.padding,
                width: viewport.width - (this.padding * 2),
                height: viewport.height - (this.padding * 2),
            };
            this.ellipse
                .attr("cx", plot.xOffset + (plot.width * 0.5))
                .attr("cy", plot.yOffset + (plot.height * 0.5))
                .attr("rx", (plot.width * 0.5))
                .attr("ry", (plot.height * 0.5));
            var fontSizeForWidth = plot.width * .20;
            var fontSizeForHeight = plot.height * .35;
            var fontSize = d3.min([fontSizeForWidth, fontSizeForHeight]);
            this.text
                .attr("x", plot.xOffset + (plot.width / 2))
                .attr("y", plot.yOffset + (plot.height / 2))
                .attr("width", plot.width)
                .attr("height", plot.height)
                .attr("font-size", fontSize);
        };
        return Viz02;
    }());
    myApp.Viz02 = Viz02;
    var Viz03 = (function () {
        function Viz03() {
            this.name = "Visual 3 - Bar Chart";
            this.dataset = [440, 290, 340, 330, 400, 512, 368, 412];
            this.padding = 12;
            this.xAxisOffset = 50;
        }
        Viz03.prototype.load = function (container) {
            this.svgRoot = d3.select(container).append("svg");
            this.plotArea = this.svgRoot.append("rect")
                .attr("fill", "lightyellow")
                .attr("stroke", "black")
                .attr("stroke-width", 1);
            this.bars = this.svgRoot.append("g")
                .selectAll("rect")
                .data(this.dataset)
                .enter()
                .append("rect");
            this.labels = d3.select("svg").selectAll("text")
                .data(this.dataset)
                .enter()
                .append("text");
            this.axisGroup = this.svgRoot.append("g");
            this.yScale = d3.scale.linear();
            this.yAxis = d3.svg.axis();
        };
        Viz03.prototype.update = function (viewport) {
            var _this = this;
            this.svgRoot
                .attr("width", viewport.width)
                .attr("height", viewport.height);
            var plot = {
                xOffset: this.padding + this.xAxisOffset,
                yOffset: this.padding,
                width: viewport.width - this.xAxisOffset - (this.padding * 2),
                height: viewport.height - (this.padding * 2),
            };
            var datasetSize = this.dataset.length;
            var xScaleFactor = plot.width / datasetSize;
            // var yScaleFactor = plot.height / d3.max(this.dataset);
            var barWidth = (plot.width / datasetSize) * 0.92;
            var barXStart = (plot.width / datasetSize) * 0.04;
            // initialize D3 scale object
            var yDomainStart = d3.max(this.dataset) * 1.05;
            var yDomainStop = 0;
            var yRangeStart = 0;
            var yRangeStop = plot.height;
            this.yScale
                .domain([yDomainStart, yDomainStop])
                .range([yRangeStart, yRangeStop]);
            // add shaded rect in background of bar chart
            this.plotArea
                .attr("x", plot.xOffset)
                .attr("y", plot.yOffset)
                .attr("width", plot.width)
                .attr("height", plot.height);
            this.bars
                .attr("x", function (d, i) { return plot.xOffset + barXStart + (i * (xScaleFactor)); })
                .attr("y", function (d, i) { return plot.yOffset + _this.yScale(Number(d)); })
                .attr("width", function (d, i) { return barWidth; })
                .attr("height", function (d, i) { return (plot.height - _this.yScale(Number(d))); })
                .attr("fill", "teal");
            //var yTextOffset = (d3.min(this.dataset) * yScaleFactor) * 0.2;
            var yTextOffset = this.yScale(d3.min(this.dataset)) * 0.5;
            var textSize = (barWidth * 0.3) + "px";
            this.labels.text(function (d, i) { return "$" + d; })
                .attr("x", function (d, i) { return plot.xOffset + (i * (xScaleFactor)) + (barWidth / 2); })
                .attr("y", function (d, i) { return plot.yOffset + plot.height - yTextOffset; })
                .attr("fill", "white")
                .attr("font-size", textSize)
                .attr("text-anchor", "middle")
                .attr("alignment-baseline", "middle");
            this
                .yAxis
                .scale(this.yScale)
                .orient('left').ticks(10);
            // determine x and y posiiton of y axis
            var xPosition = this.padding + this.xAxisOffset;
            var yPosition = this.padding;
            // create transform string
            var transform = "translate(" + xPosition + "," + yPosition + ")";
            // add axis into SVG group
            this.axisGroup
                .attr("class", "axis")
                .call(this.yAxis)
                .attr({ 'transform': transform });
        };
        return Viz03;
    }());
    myApp.Viz03 = Viz03;
    var Viz04 = (function () {
        function Viz04() {
            this.name = "Visual 04 - Doughnut Chart";
            this.dataset = [21, 26, 16, 32];
            this.padding = 12;
        }
        Viz04.prototype.load = function (container) {
            this.svgRoot = d3.select(container).append("svg");
            this.arc = d3.svg.arc();
            this.pie = d3.layout.pie();
            this.arcSelection = this.svgRoot.selectAll("arc")
                .data(this.pie(this.dataset))
                .enter()
                .append("g")
                .attr("class", "arc");
            this.arcSelectionPath = this.arcSelection.append("path");
        };
        Viz04.prototype.update = function (viewport) {
            this.svgRoot
                .attr("width", viewport.width)
                .attr("height", viewport.height);
            var plot = {
                xOffset: this.padding,
                yOffset: this.padding,
                width: viewport.width - (this.padding * 2),
                height: viewport.height - (this.padding * 2),
            };
            var outerRadius = d3.min([plot.width / 2, plot.height / 2]);
            var innerRadius = outerRadius / 3;
            this.arc
                .innerRadius(innerRadius)
                .outerRadius(outerRadius);
            this.arcSelection
                .attr("transform", "translate(" + (outerRadius + this.padding) + ", " + (outerRadius + this.padding) + ")");
            var color = ["red", "green", "blue", "purple", "Yellow"];
            this.arcSelectionPath
                .attr("fill", function (d, i) { return color[i]; })
                .attr("d", this.arc);
        };
        return Viz04;
    }());
    myApp.Viz04 = Viz04;
})(myApp || (myApp = {}));
