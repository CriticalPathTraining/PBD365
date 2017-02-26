/// <reference path="../scripts/typings/d3/d3.d.ts" />

module myApp.visuals {

  export interface ViewPort {
    width: number;
    height: number;
  }

  export interface CustomVisual {
    create(containerId: string);
    init(container: d3.Selection<any>)
    update(viewport: ViewPort);
  }

  export class CustomVisualBase implements CustomVisual {
    protected container: d3.Selection<any>;
    create(containerId: string) {
      this.container = d3.select(containerId);
      this.init(this.container);

      var windowPaddingBottom = 4;
      var viewport: ViewPort = {
        width: $(window).width(),
        height: $(window).height() - windowPaddingBottom
      };

      this.update(viewport);

      $(window).resize(() => {
        var windowPaddingBottom = 4;
        var viewport: ViewPort = {
          width: $(window).width(),
          height: $(window).height() - windowPaddingBottom
        };

        this.update(viewport);
      });

    }

    init(container: d3.Selection<any>) {

    }

    update(viewport: ViewPort) { }
  }

  export class Viz01 extends CustomVisualBase {

    dataset = [2, 7, 6, 8, 5, 8];

    svgRoot: d3.Selection<any>;

    init(container: d3.Selection<any>) {
      this.svgRoot = container.append("svg");;
    }

    update(viewport: ViewPort) {

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

      var yScale = (yInput) => { return (plot.height * yInput) / d3.max(this.dataset) };

      var rects = this.svgRoot.selectAll("rect");

      if (rects.empty()) {
        rects = this.svgRoot.selectAll("rect").data(this.dataset).enter().append("rect");
      }

      rects.transition().duration(500).delay(200)
        .attr({
        x: (d, i) => { return padding + barSpacing + (i * (plot.width / this.dataset.length)); },
        y: (d, i) => {
          return padding + plot.height - (yScale(d));
        },
        width: barWidth,
        height: (d, i) => { return yScale(d) },
        fill: "teal"
      });

    }

  }

  export class Viz02 extends CustomVisualBase {

    dataset = [32, 31, 29, 30, 32, 33, 35, 57, 40, 41, 38, 36, 22, 24, 26, 28, 28, 29, 27, 26, 23, 23, 21];

    svgRoot: d3.Selection<any>;

    init(container: d3.Selection<any>) {
      this.svgRoot = container.append("svg");;
    }

    update(viewport: ViewPort) {

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
        .range([0, plot.height - (paddingInner*2)]);

      var xScale = d3.scale.linear()
        .domain([0, this.dataset.length-1])
        .range([0, plot.width - (paddingInner * 2)]);

      var line = d3.svg.line()
        .x(function (d, i) { return paddingInner + xScale(i); })
        .y(function (d, i) { return paddingInner + plot.height - yScale(d); });

      gPlot.append("path")
        .attr("d", line(this.dataset));

    }

  }

  export class Viz03 extends CustomVisualBase {

    dataset = [32, 31, 29, 30, 32, 33, 35, 57, 40, 41, 38, 36, 22, 24, 26, 28, 28, 29, 27, 26, 23, 23, 21];

    svgRoot: d3.Selection<any>;

    init(container: d3.Selection<any>) {
      this.svgRoot = container.append("svg");;
    }

    update(viewport: ViewPort) {

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



    }

  }

  export class Viz04 extends CustomVisualBase {

    dataset = [32, 31, 29, 30, 32, 33, 35, 57, 40, 41, 38, 36, 22, 24, 26, 28, 28, 29, 27, 26, 23, 23, 21];

    svgRoot: D3.Selection;

    init(container: D3.Selection) {
      this.svgRoot = container.append("svg");;
    }

    update(viewport: ViewPort) {

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
        .attr("transform", "translate( " + yAxisOffset + ", " + (paddingOuter + paddingInner )+ ")")
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



    }

  }

  export class Viz05 extends CustomVisualBase {

    dataset = [21, 26, 16, 32];

    svgRoot: D3.Selection;

    init(container: D3.Selection) {
      this.svgRoot = container.append("svg");;
    }

    update(viewport: ViewPort) {

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
      
      var outerRadius = d3.min([plot.width / 2, plot.height /2]);
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
        .attr("transform", "translate(" + (outerRadius + padding ) + ", " + (outerRadius + padding) + ")");

      var color = ["red", "green", "blue", "purple", "Yellow"];

      arcs.append("path")
        .attr("fill", (d, i) => { return color[i]; })
        .attr("d", arc);

    }

  }


}
