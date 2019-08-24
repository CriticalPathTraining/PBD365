module powerbi.extensibility.visual {

    export interface BarchartDataPoint {
        Category: string;
        Value: number;
    }

    export interface BarchartViewModel {
        IsNotValid: boolean;
        DataPoints?: BarchartDataPoint[];
        Format?: string;
        SortBySize?: boolean;
        XAxisFontSize?: number;
        YAxisFontSize?: number;
        BarColor?: string;
    }

    export class Visual implements IVisual {

        private svg: d3.Selection<SVGElement>;
        private svgGroupMain: d3.Selection<SVGElement>;
        private settings: VisualSettings;
     
        constructor(options: VisualConstructorOptions) {
            console.log("Constructor executing...");
            this.svg = d3.select(options.element).append('svg');
            this.svgGroupMain = this.svg.append('g');
            this.svgGroupMain.append("g").append("text")
            .text("Please add fields to create a valid dataset")
            .attr("dominant-baseline", "hanging")
            .attr("font-size", 14)
            .style("fill", "red");

            this.settings = VisualSettings.getDefault() as VisualSettings;
            console.log("default settings", this.settings);
        }

        public update(options: VisualUpdateOptions) {
            console.log("Update executing...");

            // erase any existing graphics
            this.svgGroupMain.selectAll("g").remove();

            // set height and width of root SVG element using viewport passed by Power BI host
            this.svg.attr({
                height: options.viewport.height,
                width: options.viewport.width
            });

            var viewModel: BarchartViewModel = this.createViewModel(options.dataViews[0]);
            if (viewModel.IsNotValid) {
                // handle case where categorical DataView is not valid
                this.svgGroupMain.append("g").append("text")
                    .text("Please add fields to create a valid dataset")
                    .attr("dominant-baseline", "hanging")
                    .attr("font-size", 14)
                    .style("fill", "red");
                return;
            }

            var xAxisOffset: number = viewModel.XAxisFontSize * 6;
            var yAxisOffset: number = viewModel.YAxisFontSize * 2;
            var paddingSVG: number = 12;

            // create plot variable to assist with rendering barchart into plot area
            var plot = {
                xOffset: paddingSVG + xAxisOffset,
                yOffset: paddingSVG,
                width: options.viewport.width - (paddingSVG * 2) - xAxisOffset,
                height: options.viewport.height - (paddingSVG * 2) - yAxisOffset,
            };


            // offset x and y coordinates for SVG group used to create bars 
            this.svgGroupMain.attr({
                height: plot.height,
                width: plot.width,
                transform: 'translate(' + plot.xOffset + ',' + plot.yOffset + ')'
            });

            // convert data from categorical DataView into dataset used with D3 data binding
            var barchartDataPoints: BarchartDataPoint[] = viewModel.DataPoints;

            // setup D3 ordinal scale object to map input category names in dataset to output range of x coordinate
            var xScale = d3.scale.ordinal()
                .domain(barchartDataPoints.map(function (d) { return d.Category; }))
                .rangeRoundBands([0, plot.width], 0.1);

            // determine maximum value for the bars in the barchart
            var yMax = d3.max(barchartDataPoints, function (d) { return +d.Value * 1.05 });

            // setup D3 linear scale object to map input data values to output range of y coordinate
            var yScale = d3.scale.linear()
                .domain([0, yMax])
                .range([plot.height, 0]);

            // remove existing SVG elements from previous update
            this.svg.selectAll('.axis').remove();
            this.svg.selectAll('.bar').remove();

            // draw x axis
            var xAxis = d3.svg.axis()
                .scale(xScale)
                .tickSize(0)
                .tickPadding(12)
                .orient('bottom');

            // draw x axis
            this.svgGroupMain
                .append('g')
                .attr('class', 'x axis')
                .style('fill', 'black')
                .attr('transform', 'translate(0,' + (plot.height) + ')')
                .call(xAxis);

            // get format string for measure
            var valueFormatterFactory = powerbi.extensibility.utils.formatting.valueFormatter;
            var valueFormatter = valueFormatterFactory.create({
                format: viewModel.Format,
                formatSingleValues: true
            });

            // draw y axis
            var yAxis = d3.svg.axis()
                .scale(yScale)
                .orient('left')
                .ticks(5)
                .tickSize(0)
                .tickPadding(12)
                .tickFormat(function (d) { return valueFormatter.format(d) });

            this.svgGroupMain
                .append('g')
                .attr('class', 'y axis')
                .style('fill', 'black') // you can get from metadata
                .call(yAxis);

            // draw bar
            var svgGroupBars = this.svgGroupMain
                .append('g')
                .selectAll('.bar')
                .data(barchartDataPoints);

            svgGroupBars.enter()
                .append('rect')
                .attr('class', 'bar')
                .attr('fill', viewModel.BarColor)
                .attr('stroke', 'black')
                .attr('x', function (d) {
                    return xScale(d.Category);
                })
                .attr('width', xScale.rangeBand())
                .attr('y', function (d) {
                    return yScale(d.Value);
                })
                .attr('height', function (d) {
                    return plot.height - yScale(d.Value);
                });

            svgGroupBars
                .exit()
                .remove();

            d3.select(".x.axis").selectAll("text").style({ "font-size": viewModel.XAxisFontSize });
            d3.select(".y.axis").selectAll("text").style({ "font-size": viewModel.YAxisFontSize });

        }

        public createViewModel(dataView: DataView): BarchartViewModel {

            // handle case where categorical DataView is not valid
            if (typeof dataView === "undefined" ||
                typeof dataView.categorical === "undefined" ||
                typeof dataView.categorical.categories === "undefined" ||
                typeof dataView.categorical.values === "undefined") {
                return { IsNotValid: true };
            }

            this.settings = VisualSettings.parse(dataView) as VisualSettings;

            console.log("this.settings.barchartProperties.barColor: " + this.settings.barchartProperties.barColor)

            var categoricalDataView: DataViewCategorical = dataView.categorical;
            var categoryColumn: DataViewCategoricalColumn = categoricalDataView.categories[0];
            var categoryNames: PrimitiveValue[] = categoricalDataView.categories[0].values;
            var categoryValues: PrimitiveValue[] = categoricalDataView.values[0].values;

            var barchartDataPoints: BarchartDataPoint[] = [];

            for (var i = 0; i < categoryValues.length; i++) {
                // get category name and category value
                var category: string = <string>categoryNames[i];
                var categoryValue: number = <number>categoryValues[i];
                // add new data point to barchartDataPoints collection
                barchartDataPoints.push({
                    Category: category,
                    Value: categoryValue
                });
            }

            // get formatting code for the field that is the measure
            var format: string = categoricalDataView.values[0].source.format;

            // get persistent property values
            var sortBySize: boolean = this.settings.barchartProperties.sortBySize;
            var xAxisFontSize: number = this.settings.barchartProperties.xAxisFontSize;
            var yAxisFontSize: number = this.settings.barchartProperties.yAxisFontSize;

            var barColor: string  = "";
            if(typeof(this.settings.barchartProperties.barColor)=="string"){
                barColor = <string>this.settings.barchartProperties.barColor;
            }
            else{
                barColor = <string>this.settings.barchartProperties.barColor.solid.color;
            }            

            // sort dataset rows by measure value if required
            if (sortBySize) {
                barchartDataPoints.sort((x, y) => { return y.Value - x.Value })
            }
            else{
                barchartDataPoints.sort((x, y) => { return (y.Category > x.Category) ? -1: 1 })
            }

            // return view model to update method
            return {
                IsNotValid: false,
                DataPoints: barchartDataPoints,
                Format: format,
                SortBySize: sortBySize,
                BarColor: barColor,
                XAxisFontSize: xAxisFontSize,
                YAxisFontSize: yAxisFontSize
            };

        }

        public enumerateObjectInstances(options: EnumerateVisualObjectInstancesOptions): VisualObjectInstanceEnumeration {
            console.log("EXECUTING: enumerateObjectInstances...");
            var visualObjects: VisualObjectInstanceEnumerationObject = <VisualObjectInstanceEnumerationObject>VisualSettings.enumerateObjectInstances(this.settings, options);         
            visualObjects.instances[0].validValues = {
                xAxisFontSize: { numberRange: { min: 10, max: 36 } },
                yAxisFontSize: { numberRange: { min: 10, max: 36 } },                
            };         
            return visualObjects;          
        }

    }
}