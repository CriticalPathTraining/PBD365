
module myApp {

  export interface IViewPort {
    width: number;
    height: number;
  }

  export interface ICustomVisual {
    name: string;
    load(container: HTMLElement): void;
    update(viewport: IViewPort): void;
  }

  export class Viz01 implements ICustomVisual {

    public name: string = "Visual 1: Hello jQuery";
    private container: JQuery;
    private message: JQuery;

    load(container: HTMLElement) {

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
    }

    public update(viewport: IViewPort) {

      let paddingX: number = 2;
      let paddingY: number = 2;
      let fontSizeMultiplierX: number = viewport.width * 0.15;
      let fontSizeMultiplierY: number = viewport.height * 0.4;
      let fontSizeMultiplier: number = Math.min(...[fontSizeMultiplierX, fontSizeMultiplierY]);

      this.message.css({
        "width": viewport.width - paddingX,
        "height": viewport.height - paddingY,
        "font-size": fontSizeMultiplier
      });

    }
  }

  export class Viz02 implements ICustomVisual {

    name = "Visual 2: Hello D3";

    private svgRoot: d3.Selection<SVGElementInstance>;
    private ellipse: d3.Selection<SVGElementInstance>;
    private text: d3.Selection<SVGElementInstance>;
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

      this.ellipse
        .attr("cx", plot.xOffset + (plot.width * 0.5))
        .attr("cy", plot.yOffset + (plot.height * 0.5))
        .attr("rx", (plot.width * 0.5))
        .attr("ry", (plot.height * 0.5))

      var fontSizeForWidth: number = plot.width * .20;
      var fontSizeForHeight: number = plot.height * .35;
      var fontSize: number = d3.min([fontSizeForWidth, fontSizeForHeight]);

      this.text
        .attr("x", plot.xOffset + (plot.width / 2))
        .attr("y", plot.yOffset + (plot.height / 2))
        .attr("width", plot.width)
        .attr("height", plot.height)
        .attr("font-size", fontSize);

    }

  }

  export class Viz03 implements ICustomVisual {

    name = "Visual 3 - Bar Chart";

    private dataset: number[] = [440, 290, 340, 330, 400, 512, 368, 412];

    private svgRoot: d3.Selection<SVGElementInstance>;
    private bars: d3.Selection<number>;
    private labels: d3.Selection<number>;
    private padding: number = 12;


    private plotArea: d3.Selection<SVGElementInstance>;
    private axisGroup: d3.Selection<SVGElementInstance>;
    private xAxisOffset: number = 50;
    private yScale: d3.scale.Linear<number, number>;
    private yAxis: d3.svg.Axis;

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

      this.labels = d3.select("svg").selectAll("text")
        .data(this.dataset)
        .enter()
        .append("text")

      this.axisGroup = this.svgRoot.append("g");
      this.yScale = d3.scale.linear();
      this.yAxis = d3.svg.axis();

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

      var datasetSize = this.dataset.length;
      var xScaleFactor = plot.width / datasetSize;
      // var yScaleFactor = plot.height / d3.max(this.dataset);
      var barWidth = (plot.width / datasetSize) * 0.92;
      var barXStart = (plot.width / datasetSize) * 0.04

      // initialize D3 scale object
      var yDomainStart: number = d3.max(this.dataset) * 1.05;
      var yDomainStop: number = 0;
      var yRangeStart: number = 0;
      var yRangeStop: number = plot.height;

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
        .attr("x", (d, i) => { return plot.xOffset + barXStart + (i * (xScaleFactor)); })
        .attr("y", (d, i) => { return plot.yOffset + this.yScale(Number(d)); })
        .attr("width", (d, i) => { return barWidth; })
        .attr("height", (d, i) => { return (plot.height - this.yScale(Number(d))); })
        .attr("fill", "teal");

      //var yTextOffset = (d3.min(this.dataset) * yScaleFactor) * 0.2;
      var yTextOffset = this.yScale(d3.min(this.dataset)) * 0.5;

      var textSize = (barWidth * 0.3) + "px";

      this.labels.text((d, i) => { return "$" + d; })
        .attr("x", (d, i) => { return plot.xOffset + (i * (xScaleFactor)) + (barWidth / 2); })
        .attr("y", (d, i) => { return plot.yOffset + plot.height - yTextOffset; })
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

    }
  }

  export class Viz04 implements ICustomVisual {

    name = "Visual 04 - Doughnut Chart";

    dataset = [21, 26, 16, 32];

    private svgRoot: d3.Selection<SVGElementInstance>;
    private plotArea: d3.Selection<SVGElementInstance>;

    private arc: d3.svg.Arc<d3.layout.pie.Arc<number>>;
    private pie: d3.layout.Pie<number>;
    private pieDataset: d3.layout.pie.Arc<number>[];
    private arcSelection: d3.Selection<d3.layout.pie.Arc<number>>;
    private arcSelectionPath: d3.Selection<d3.layout.pie.Arc<number>>;

    private padding: number = 12;

    load(container: HTMLElement) {

      this.svgRoot = d3.select(container).append("svg");
      this.arc = d3.svg.arc<d3.layout.pie.Arc<number>>();
      this.pie = d3.layout.pie();

      this.arcSelection = this.svgRoot.selectAll("arc")
        .data(this.pie(this.dataset))
        .enter()
        .append("g")
        .attr("class", "arc");


      this.arcSelectionPath = this.arcSelection.append("path");

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

      var outerRadius = d3.min([plot.width / 2, plot.height / 2]);
      var innerRadius = outerRadius / 3;

      this.arc
        .innerRadius(innerRadius)
        .outerRadius(outerRadius);


      this.arcSelection
        .attr("transform", "translate(" + (outerRadius + this.padding) + ", " + (outerRadius + this.padding) + ")");

      var color = ["red", "green", "blue", "purple", "Yellow"];

      this.arcSelectionPath
        .attr("fill", (d, i) => { return color[i]; })
        .attr("d", this.arc);


    }
  }


}