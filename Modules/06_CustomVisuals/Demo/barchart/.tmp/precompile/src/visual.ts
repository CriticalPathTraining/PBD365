module powerbi.extensibility.visual.PBI_CV_996C8E45_0D05_4C52_B0C1_2C020A0F20DE  {

  export interface CategoryItem {
    Category: string;
    Value: number;
  }

  export class Visual implements IVisual {

    private svgRoot: d3.Selection<SVGElementInstance>;
    private svgGroupMain: d3.Selection<SVGElementInstance>;
    private padding: number = 12;
    private dataview: DataView;
    
    constructor(options: VisualConstructorOptions) {
      this.svgRoot = d3.select(options.element).append('svg');
      this.svgGroupMain = this.svgRoot.append("g");      
    }

    public update(options: VisualUpdateOptions) {

      // ensure that categorical dataview contains categories and measurable values 
      var categorical = options.dataViews[0].categorical;
      if (typeof categorical.categories === "undefined" || typeof categorical.values === "undefined") {
        // remove all existing SVG elements 
        this.svgGroupMain.empty();
        return;
      }
      
      // get categorical data from visual data view
      this.dataview = options.dataViews[0];
      
      // convert categorical data into specialized data structure for data binding
      var visualData: CategoryItem[] = Visual.converter(this.dataview.categorical);

      this.svgRoot
        .attr("width", options.viewport.width)
        .attr("height", options.viewport.height);

      var xAxisOffset = 50;
      var yAxisOffset = 24

      var plot = {
        xOffset: this.padding + xAxisOffset,
        yOffset: this.padding,
        width: options.viewport.width - (this.padding * 2) - xAxisOffset,
        height: options.viewport.height - (this.padding * 2) - yAxisOffset,
      };
      
      this.svgGroupMain.attr({
        height: plot.height,
        width: plot.width,
        transform: 'translate(' + plot.xOffset + ',' + plot.yOffset + ')'
      });


      // setup d3 scale
      var xScale = d3.scale.ordinal()
        .domain(visualData.map(function (d) { return d.Category; }))
        .rangeRoundBands([0, plot.width], 0.1);

      var yMax = d3.max(visualData, function (d) { return d.Value * 1.05 });

      var yScale = d3.scale.linear()
        .domain([0, yMax])
        .range([plot.height, 0]);

      this.svgRoot.selectAll('.axis').remove();
      this.svgRoot.selectAll('.bar').remove();

      // draw x axis
      var xAxis = d3.svg.axis()
        .scale(xScale)
        .orient('bottom');

      this.svgGroupMain
        .append('g')
        .attr('class', 'x axis')
        .style('fill', 'black')
        .attr('transform', 'translate(0,' + (plot.height) + ')')
        .call(xAxis);

      // draw y axis
      var formatValue = d3.format(".2s");
      var yAxis = d3.svg.axis()
        .scale(yScale)
        .orient('left')
        .ticks(5)
        .tickFormat(function (d) { return formatValue(d) });

      this.svgGroupMain
        .append('g')
        .attr('class', 'y axis')
        .style('fill', 'black') 
        .call(yAxis);


      var datasetSize = visualData.length;
      var xScaleFactor = plot.width / datasetSize; 
      var yValueMax = d3.max(visualData, (d: CategoryItem) => { return d.Value });   
      var yScaleFactor = plot.height / yValueMax;
      var barWidth = (plot.width / datasetSize) * 0.92;

      var bars = this.svgGroupMain
        .append('g')
        .selectAll('.bar')
        .data(visualData);
      
      bars
        .enter()
        .append('rect')
        .attr('class', 'bar');
      
      bars
        .attr("x", (d, i) => { return xScale(d.Category); })
        .attr("y", (d, i) => { return yScale(d.Value); })
        .attr("width", (d, i) => { return xScale.rangeBand(); })
        .attr("height", (d, i) => { return plot.height - yScale(d.Value); })
        .attr("fill", Visual.getFill(this.dataview).solid.color);

    }

    private static getFill(dataView: DataView): Fill {
      if (dataView) {
        var objects = dataView.metadata.objects;
        if (objects) {
          var object = objects['colorSelector'];
          if (object) {
            var fill = <Fill>object['fill'];
            if (fill)
              console.log(JSON.stringify(fill));
            return fill;
          }
        }
      }
      return { solid: { color: "teal" } };
    }

    public enumerateObjectInstances(options: EnumerateVisualObjectInstancesOptions): VisualObjectInstanceEnumeration {

      let objectName = options.objectName;
      let objectEnumeration: VisualObjectInstance[] = [];

      switch (objectName) {
        case 'colorSelector':
          objectEnumeration.push({
            objectName: objectName,
            properties: {
              fill: Visual.getFill(this.dataview)
            },
            selector: null
          });
          break;
      };

      return objectEnumeration;
    }

    public static converter(categoricalData: DataViewCategorical): CategoryItem[] {
      
      var visualData: CategoryItem[] = [];
   
      var categories: PrimitiveValue[] = categoricalData.categories[0].values;
      var categoryValues: PrimitiveValue[] = categoricalData.values[0].values;

      for (var i = 0; i < categoryValues.length; i++) {

        var category: string = <string>categories[i];
        var categoryValue: number = <number>categoryValues[i];

        visualData.push({
          Category: category,
          Value: categoryValue
        });
      }

      visualData.sort( (cat1, cat2) => { return cat2.Value - cat1.Value; })

      return visualData;
    }
    
  }
}