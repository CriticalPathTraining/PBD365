import * as d3 from 'd3';
import { BaseType, ScaleLinear } from 'd3';

type d3Element = d3.Selection<SVGElement, {}, HTMLElement, any>;
type d3RectElement = d3.Selection<SVGRectElement, {}, SVGElement, any>;
type d3TextElement = d3.Selection<SVGTextElement, {}, BaseType, any>;
type d3LineElement = d3.Selection<SVGLineElement, {}, SVGElement, any>;

export interface IViewPort {
  width: number;
  height: number;
}

export interface ICustomVisual {
  name: string;
  icon: string;
  load(container: HTMLElement): void;
  update(viewport: IViewPort): void;
}

export class Viz01 implements ICustomVisual {

  public name = "Hello D3";
  public icon = "fa-paper-plane-o";

  private svgRoot: d3Element;
  private ellipse: d3Element;
  private text: d3Element;
  private padding: number = 20;

  load(container: HTMLElement) {

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
  }

  public update(viewport: IViewPort) {

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
      .attr("ry", (plot.height * 0.5))

    var fontSizeForWidth = plot.width * .20;
    var fontSizeForHeight = plot.height * .35;
    var fontSize = d3.min([fontSizeForWidth, fontSizeForHeight]);

    this.text
      .attr("x", plot.xOffset + (plot.width * 0.5))
      .attr("y", plot.yOffset + (plot.height * 0.5))
      .attr("width", plot.width)
      .attr("height", plot.height)
      .attr("font-size", fontSize);
  }

}

export class Viz02 implements ICustomVisual {

  name = "Barchart1";
  icon = "fa-bar-chart";

  private dataset = [440, 290, 340, 330, 400, 512, 368];

  private svgRoot: d3Element;
  private bars: d3RectElement;
  private padding: number = 12;

  load(container: HTMLElement) {
    this.svgRoot = d3.select(container).append("svg");
    this.bars = this.svgRoot
      .selectAll("rect")
      .data(this.dataset)
      .enter()
      .append("rect");
  }

  update(viewport: IViewPort) {

    this.svgRoot
      .attr("width", viewport.width)
      .attr("height", viewport.height);

    var plot = {
      xOffset: this.padding,
      yOffset: this.padding,
      width: viewport.width - (this.padding * 2),
      height: viewport.height - (this.padding * 2),
    };

    var datasetSize = this.dataset.length;
    var xScaleFactor = plot.width / datasetSize;
    var yScaleFactor = plot.height / d3.max(this.dataset);
    var barWidth = (plot.width / datasetSize) * 0.92;

    this.bars
      .attr("x", (d, i) => {
        return plot.xOffset + (i * (xScaleFactor));
      })
      .attr("y", (d, i) => {
        return plot.yOffset + plot.height - (Number(d) * yScaleFactor);
      })
      .attr("width", (d, i) => {
        return barWidth;
      })
      .attr("height", (d, i) => {
        return (Number(d) * yScaleFactor);
      })
      .attr("fill", "teal");
  }

}

export class Viz03 implements ICustomVisual {

  name = "Barchart 2";
  icon = "fa-bar-chart";

  private dataset = [440, 290, 340, 330, 400, 512, 368];

  private svgRoot: d3Element;
  private bars: d3RectElement;
  private labels: d3TextElement;
  private padding: number = 12;

  load(container: HTMLElement) {
    this.svgRoot = d3.select(container).append("svg");

    this.bars = this.svgRoot
      .selectAll("rect")
      .data(this.dataset)
      .enter()
      .append("rect");

    this.labels = d3.select("svg").selectAll("text")
      .data(this.dataset)
      .enter()
      .append("text");
  }

  update(viewport: IViewPort) {

    this.svgRoot
      .attr("width", viewport.width)
      .attr("height", viewport.height);

    var plot = {
      xOffset: this.padding,
      yOffset: this.padding,
      width: viewport.width - (this.padding * 2),
      height: viewport.height - (this.padding * 2),
    };

    var datasetSize = this.dataset.length;
    var xScaleFactor = plot.width / datasetSize;
    var yScaleFactor = plot.height / d3.max(this.dataset);
    var barWidth = (plot.width / datasetSize) * 0.92;

    this.bars
      .attr("x", (d, i) => { return plot.xOffset + (i * (xScaleFactor)); })
      .attr("y", (d, i) => { return plot.yOffset + plot.height - (Number(d) * yScaleFactor); })
      .attr("width", (d, i) => { return barWidth; })
      .attr("height", (d, i) => { return (Number(d) * yScaleFactor); })
      .attr("fill", "teal");

    var yTextOffset = (d3.min(this.dataset) * yScaleFactor) * 0.2;
    var textSize = (barWidth * 0.3) + "px";

    this.labels.text((d, i) => { return "$" + d; })
      .attr("x", (d, i) => { return plot.xOffset + (i * (xScaleFactor)) + (barWidth / 2); })
      .attr("y", (d, i) => { return plot.yOffset + plot.height - yTextOffset; })
      .attr("fill", "white")
      .attr("font-size", textSize)
      .attr("text-anchor", "middle")
      .attr("alignment-baseline", "middle");

  }
}

export class Viz04 implements ICustomVisual {

  name = "Barchart 3";
  icon = "fa-bar-chart"

  private dataset = [440, 290, 340, 330, 400, 512, 368];

  private svgRoot: d3Element;
  private plotArea: d3Element;
  private axisGroup: d3Element;
  private bars: d3RectElement;
  private labels: d3TextElement;
  private padding: number = 12;
  private xAxisOffset: number = 50;
  private yScale: d3.ScaleLinear<number, number>;
  private yAxis: d3.Axis<number | { valueOf(): number; }>;

  load(container: HTMLElement) {

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

    this.labels = this.svgRoot
      .selectAll("text")
      .data(this.dataset)
      .enter()
      .append("text");

    this.axisGroup = this.svgRoot.append("g");
    this.yScale = d3.scaleLinear();
    this.yAxis = d3.axisLeft(this.yScale);
  }

  update(viewport: IViewPort) {

    this.svgRoot
      .attr("width", viewport.width)
      .attr("height", viewport.height);

    var plot = {
      xOffset: this.padding + this.xAxisOffset,
      yOffset: this.padding,
      width: viewport.width - this.xAxisOffset - (this.padding * 2),
      height: viewport.height - (this.padding * 2),
    };

    var yDomainStart: number = d3.max(this.dataset) * 1.05;
    var yDomainStop: number = 0;
    var yRangeStart: number = 0;
    var yRangeStop: number = plot.height;

    this.yScale
      .domain([yDomainStart, yDomainStop])
      .range([yRangeStart, yRangeStop]);

    var datasetSize = this.dataset.length;
    var xScaleFactor = plot.width / datasetSize;
    var barXStart = (plot.width / datasetSize) * 0.05
    var barWidth = (plot.width / datasetSize) * 0.92;
    var yScaleFactor = plot.height / d3.max(this.dataset);

    this.plotArea
      .attr("x", plot.xOffset)
      .attr("y", plot.yOffset)
      .attr("width", plot.width)
      .attr("height", plot.height);

    this.bars
      .attr("x", (d, i) => { return plot.xOffset + barXStart + (i * (xScaleFactor)); })
      .attr("y", (d, i) => { return plot.yOffset + this.yScale(Number(d)); })
      .attr("width", (d, i) => { return barWidth; })
      .attr("height", (d, i) => { return (plot.height - this.yScale(Number(d))); })
      .attr("fill", "teal");

    var yTextOffset = this.yScale(d3.min(this.dataset)) * 0.5;
    var textSize = (barWidth * 0.3) + "px";

    this.labels
      .text((d, i) => { return "$" + d; })
      .attr("x", (d, i) => { return plot.xOffset + (i * (xScaleFactor)) + (barWidth / 2); })
      .attr("y", (d, i) => { return plot.yOffset + plot.height - yTextOffset; })
      .attr("fill", "white")
      .attr("font-size", textSize)
      .attr("text-anchor", "middle")
      .attr("alignment-baseline", "middle");


    this.yAxis.scale(this.yScale).ticks(10);
    var transform = "translate(" + (this.padding + this.xAxisOffset) + "," + this.padding + ")";
    this.axisGroup
      .attr("class", "axis")
      .attr("transform", transform)
      .call(this.yAxis);


  }
}

export class Viz05 implements ICustomVisual {

  name = "Barchart 4";
  icon = "fa-bar-chart";

  private dataset = [440, 290, 340, 330, 400, 512, 368];

  private svgRoot: d3Element;
  private plotArea: d3Element;
  private axisGroup: d3Element;
  private bars: d3RectElement;
  private labels: d3TextElement;
  private padding: number = 12;
  private xAxisOffset: number = 50;
  private yScale: d3.ScaleLinear<number, number>;
  private yAxis: d3.Axis<number | { valueOf(): number; }>;

  load(container: HTMLElement) {

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

    this.labels = this.svgRoot
      .selectAll("text")
      .data(this.dataset)
      .enter()
      .append("text");

    this.axisGroup = this.svgRoot.append("g");
    this.yScale = d3.scaleLinear();
    this.yAxis = d3.axisLeft(this.yScale);
  }

  update(viewport: IViewPort) {

    this.svgRoot
      .attr("width", viewport.width)
      .attr("height", viewport.height);

    var plot = {
      xOffset: this.padding + this.xAxisOffset,
      yOffset: this.padding,
      width: viewport.width - this.xAxisOffset - (this.padding * 2),
      height: viewport.height - (this.padding * 2),
    };

    var yDomainStart: number = d3.max(this.dataset) * 1.05;
    var yDomainStop: number = 0;
    var yRangeStart: number = 0;
    var yRangeStop: number = plot.height;

    this.yScale
      .domain([yDomainStart, yDomainStop])
      .range([yRangeStart, yRangeStop]);

    var datasetSize = this.dataset.length;
    var xScaleFactor = plot.width / datasetSize;
    var barXStart = (plot.width / datasetSize) * 0.05
    var barWidth = (plot.width / datasetSize) * 0.92;
    var yScaleFactor = plot.height / d3.max(this.dataset);

    this.plotArea
      .attr("x", plot.xOffset)
      .attr("y", plot.yOffset)
      .attr("width", plot.width)
      .attr("height", plot.height);

    this.bars
      .attr("x", (d, i) => { return plot.xOffset + barXStart + (i * (xScaleFactor)); })
      .attr("y", (d, i) => { return plot.yOffset + this.yScale(Number(d)); })
      .attr("width", (d, i) => { return barWidth; })
      .attr("height", (d, i) => { return (plot.height - this.yScale(Number(d))); })
      .attr("fill", "teal")
      .on("mouseover", function () { d3.select(this).attr("fill", "black") })
      .on("mouseout", function () { d3.select(this).attr("fill", "teal") });

    var yTextOffset = this.yScale(d3.min(this.dataset)) * 0.5;
    var textSize = (barWidth * 0.3) + "px";

    this.labels
      .text((d, i) => { return "$" + d; })
      .attr("x", (d, i) => { return plot.xOffset + (i * (xScaleFactor)) + (barWidth / 2); })
      .attr("y", (d, i) => { return plot.yOffset + plot.height - yTextOffset; })
      .attr("fill", "white")
      .attr("font-size", textSize)
      .attr("text-anchor", "middle")
      .attr("alignment-baseline", "middle");


    this.yAxis.scale(this.yScale).ticks(10);
    var transform = "translate(" + (this.padding + this.xAxisOffset) + "," + this.padding + ")";
    this.axisGroup
      .attr("class", "axis")
      .attr("transform", transform)
      .call(this.yAxis);

  }
}

export class Viz06 implements ICustomVisual {

  name = "Transitions";
  icon = "fa-bar-chart";

  private dataset = [440, 290, 340, 330, 400, 512, 368];

  private svgRoot: d3Element;
  private plotArea: d3Element;
  private axisGroup: d3Element;
  private bars: d3RectElement;
  private labels: d3TextElement;
  private padding: number = 12;
  private xAxisOffset: number = 50;
  private yScale: d3.ScaleLinear<number, number>;
  private yAxis: d3.Axis<number | { valueOf(): number; }>;

  load(container: HTMLElement) {

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

    this.labels = this.svgRoot
      .selectAll("text")
      .data(this.dataset)
      .enter()
      .append("text");

    this.axisGroup = this.svgRoot.append("g");
    this.yScale = d3.scaleLinear();
    this.yAxis = d3.axisLeft(this.yScale);
  }

  update(viewport: IViewPort) {

    this.svgRoot
      .attr("width", viewport.width)
      .attr("height", viewport.height);

    var plot = {
      xOffset: this.padding + this.xAxisOffset,
      yOffset: this.padding,
      width: viewport.width - this.xAxisOffset - (this.padding * 2),
      height: viewport.height - (this.padding * 2),
    };

    var yDomainStart: number = d3.max(this.dataset) * 1.05;
    var yDomainStop: number = 0;
    var yRangeStart: number = 0;
    var yRangeStop: number = plot.height;

    this.yScale
      .domain([yDomainStart, yDomainStop])
      .range([yRangeStart, yRangeStop]);

    var datasetSize = this.dataset.length;
    var xScaleFactor = plot.width / datasetSize;
    var barXStart = (plot.width / datasetSize) * 0.05
    var barWidth = (plot.width / datasetSize) * 0.92;
    var yScaleFactor = plot.height / d3.max(this.dataset);

    this.plotArea
      .attr("x", plot.xOffset)
      .attr("y", plot.yOffset)
      .attr("width", plot.width)
      .attr("height", plot.height);

    this.bars
      .attr("x", (d, i) => { return plot.xOffset + barXStart + (i * (xScaleFactor)); })
      .attr("y", (d, i) => { return plot.yOffset + this.yScale(Number(d)); })
      .attr("width", (d, i) => { return barWidth; })
      .attr("height", (d, i) => { return (plot.height - this.yScale(Number(d))); })
      .attr("fill", "teal")
      .on("mouseover", function () { d3.select(this).attr("fill", "black") })
      .on("mouseout", function () { d3.select(this).attr("fill", "teal") })
      .on("click", function (d, i) {
        // get reference to current bar
        var currentBar = d3.select(this);
        // determine current bar Y position and height
        var currentY: number = parseInt(currentBar.attr("y"));
        var currentHeight: number = parseInt(currentBar.attr("height"));
        // transition bar to height of zero
        currentBar.transition().duration(1000).ease(d3.easeBounce)
          .attr("y", currentY + (currentHeight))
          .attr("height", 0)
          .on("end", () => {
            // transition bar back to previoys height
            currentBar.transition().duration(500).ease(d3.easeBounce).delay(100)
              .attr("y", currentY)
              .attr("height", currentHeight);
          });
      });

    var yTextOffset = this.yScale(d3.min(this.dataset)) * 0.5;
    var textSize = (barWidth * 0.3) + "px";

    this.labels
      .text((d, i) => { return "$" + d; })
      .attr("x", (d, i) => { return plot.xOffset + (i * (xScaleFactor)) + (barWidth / 2); })
      .attr("y", (d, i) => { return plot.yOffset + plot.height - yTextOffset; })
      .attr("fill", "white")
      .attr("font-size", textSize)
      .attr("text-anchor", "middle")
      .attr("alignment-baseline", "middle");


    this.yAxis.scale(this.yScale).ticks(10);
    var transform = "translate(" + (this.padding + this.xAxisOffset) + "," + this.padding + ")";
    this.axisGroup
      .attr("class", "axis")
      .call(this.yAxis)
      .attr("transform", transform);

  }
}

export class Viz07 implements ICustomVisual {

  name = "Line Chart";
  icon = "fa-line-chart"

  dataset: [number, number][] = [[0, 0], [0.5, 4], [1.0, 8], [1.6, 16], [2.1, 14], [3.0, 21]];

  private svgRoot: d3Element;
  private plotArea: d3Element;
  private line: any;
  private path: d3Element;
  private padding: number = 12;

  private yAxisGroup: d3Element;
  private yScale: d3.ScaleLinear<number, number>;
  private yAxis: d3.Axis<number | { valueOf(): number; }>;
  private yAxisOffset: number = 20;

  private xAxisGroup: d3Element;
  private xScale: d3.ScaleLinear<number, number>;
  private xAxis: d3.Axis<number | { valueOf(): number; }>;
  private xAxisOffset: number = 20;

  load(container: HTMLElement) {

    this.svgRoot = d3.select(container).append("svg");

    this.plotArea = this.svgRoot.append("rect")
      .attr("fill", "lightyellow")
      .attr("stroke", "black")
      .attr("stroke-width", 1);


    this.yAxisGroup = this.svgRoot.append("g");
    this.yScale = d3.scaleLinear();
    this.yAxis = d3.axisLeft(this.yScale);

    this.xAxisGroup = this.svgRoot.append("g");
    this.xScale = d3.scaleLinear();
    this.xAxis = d3.axisBottom(this.xScale);

    this.line = d3.line();

    this.path = this.svgRoot.append("path")
      .datum(this.dataset)
      .attr("fill", "none")
      .attr("stroke", "steelblue")
      .attr("stroke-linejoin", "round")
      .attr("stroke-linecap", "round")
      .attr("stroke-width", 1.5);


  }

  update(viewport: IViewPort) {


    this.svgRoot
      .attr("width", viewport.width)
      .attr("height", viewport.height);

    var plot = {
      xOffset: this.padding + this.xAxisOffset,
      yOffset: this.padding,
      width: viewport.width - this.xAxisOffset - (this.padding * 2),
      height: viewport.height - this.yAxisOffset - (this.padding * 2),
    };

    var maxValueY: number = d3.max(this.dataset, (value) => { return value[1]; });
    var yDomainStart: number = maxValueY * 1.05;
    var yDomainStop: number = 0;
    var yRangeStart: number = 0;
    var yRangeStop: number = plot.height;

    var maxValueX: number = d3.max(this.dataset, (value) => { return value[0]; });
    var xDomainStart: number = 0;
    var xDomainStop: number = maxValueX * 1.05;
    var xRangeStart: number = 0;
    var xRangeStop: number = plot.width;

    this.yScale
      .domain([yDomainStart, yDomainStop])
      .range([yRangeStart, yRangeStop]);

    this.xScale
      .domain([xDomainStart, xDomainStop])
      .range([xRangeStart, xRangeStop]);

    var datasetSize = this.dataset.length;

    this.plotArea
      .attr("x", plot.xOffset)
      .attr("y", plot.yOffset)
      .attr("width", plot.width)
      .attr("height", plot.height);

    this.yAxis.scale(this.yScale).ticks(10);
    var transform = "translate(" + (this.padding + this.xAxisOffset) + "," + this.padding + ")";
    this.yAxisGroup.attr("class", "axis").call(this.yAxis).attr("transform", transform);

    this.xAxis.scale(this.xScale).ticks(10);
    var transform = "translate(" + (this.padding + this.yAxisOffset) + "," + (plot.height + this.padding) + ")";
    this.xAxisGroup.attr("class", "axis").call(this.xAxis).attr("transform", transform);

    this.line
      .x((d, i) => { return this.padding + this.xAxisOffset + this.xScale(d[0]); })
      .y((d, i) => { return this.padding + this.yScale(d[1]); });

    this.path.attr("d", this.line);

  }

}

//  fa-calendar fa-diamond fa-paper-plane-o fa-line-chart fa-coffee fa-money

export class VisualGallery {
  static Visuals: ICustomVisual[] = [
    new Viz01(),
    new Viz02(),
    new Viz03(),
    new Viz04(),
    new Viz05(),
    new Viz06(),
    new Viz07()
  ];
}