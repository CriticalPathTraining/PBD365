import powerbi from "powerbi-visuals-api";
import VisualConstructorOptions = powerbi.extensibility.visual.VisualConstructorOptions;
import VisualUpdateOptions = powerbi.extensibility.visual.VisualUpdateOptions;
import IVisual = powerbi.extensibility.visual.IVisual;

import DataView = powerbi.DataView;
import DataViewValueColumn = powerbi.DataViewValueColumn;
import DataViewCategorical = powerbi.DataViewCategorical;
import DataViewCategoricalColumn = powerbi.DataViewCategoricalColumn;
import DataViewCategoryColumn = powerbi.DataViewCategoryColumn;
import PrimitiveValue = powerbi.PrimitiveValue;
import IVisualHost = powerbi.extensibility.visual.IVisualHost;

import IColorPalette = powerbi.extensibility.IColorPalette;
import VisualObjectInstance = powerbi.VisualObjectInstance;
import VisualObjectInstanceEnumeration = powerbi.VisualObjectInstanceEnumeration;
import VisualObjectInstanceEnumerationObject = powerbi.VisualObjectInstanceEnumerationObject;
import EnumerateVisualObjectInstancesOptions = powerbi.EnumerateVisualObjectInstancesOptions;
import Fill = powerbi.Fill;
import VisualTooltipDataItem = powerbi.extensibility.VisualTooltipDataItem;
import ISelectionManager = powerbi.extensibility.ISelectionManager;

import { valueFormatter as vf, textMeasurementService as tms } from "powerbi-visuals-utils-formattingutils";
import IValueFormatter = vf.IValueFormatter;

import { VisualSettings } from './settings';

import * as d3 from "d3";

import "./../style/visual.less";

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
    ColumnName?: string;
    MeasureName?: string;
}

export class Visual implements IVisual {

    private svg: d3.Selection<SVGElement, {}, HTMLElement, any>;
    private svgGroupMain: d3.Selection<SVGElement, {}, HTMLElement, any>;
    private hostService: IVisualHost;
    private settings: VisualSettings;

    constructor(options: VisualConstructorOptions) {
        this.hostService = options.host;

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

        // set height and width of root SVG element using viewport passed by Power BI host
        this.svg.attr("height", options.viewport.height);
        this.svg.attr("width", options.viewport.width);

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

        this.svg.selectAll("g").remove();

        var margin = {
            top: 20,
            right: 20,
            bottom: 30,
            left: 80
        };
        var width = +(this.svg.attr("width")) - margin.left - margin.right;
        var height = +(this.svg.attr("height")) - margin.top - margin.bottom;
        var svgGroupMain = this.svg.append("g").attr("transform", "translate(" + margin.left + "," + margin.top + ")");


        var xScale = d3.scaleBand()
            .rangeRound([0, width])
            .padding(0.1);

        xScale.domain(viewModel.DataPoints.map(function (d) {
            return d.Category;
        }));

        var yScale = d3.scaleLinear()
            .rangeRound([height, 0]);

        yScale.domain([0, d3.max(viewModel.DataPoints, function (d) {
            return Number(d.Value);
        })]);

        svgGroupMain.append("g")
             .attr("class", "xAxis")
            .attr("transform", "translate(0," + height + ")")
            .call(d3.axisBottom(xScale));

        var valueFormatter = vf.create({
            format: viewModel.Format,
            cultureSelector: this.hostService.locale
        });


        var yAxis = d3.axisLeft(yScale)
            .tickFormat(function (d) { return valueFormatter.format(d) });
  //          .tickPadding(12).ticks(5);


        svgGroupMain.append("g")
        .attr("class", "yAxis")
            .call(yAxis);

        svgGroupMain.selectAll(".bar")
            .data(viewModel.DataPoints)
            .enter().append("rect")
            .attr("class", "bar")
            .attr("fill",viewModel.BarColor)
            .attr("x", function (d) { return xScale(d.Category); })
            .attr("y", function (d) { return yScale(Number(d.Value)); })
            .attr("width", xScale.bandwidth())
            .attr("height", function (d) { return height - yScale(Number(d.Value)); });

        d3.select(".xAxis").selectAll("text").style("font-size", viewModel.XAxisFontSize);
        d3.select(".yAxis").selectAll("text").style("font-size", viewModel.YAxisFontSize);

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

        var barColor: string = "";
        if (typeof (this.settings.barchartProperties.barColor) == "string") {
            barColor = <string>this.settings.barchartProperties.barColor;
        }
        else {
            barColor = <string>this.settings.barchartProperties.barColor.solid.color;
        }

        // sort dataset rows by measure value if required
        if (sortBySize) {
            barchartDataPoints.sort((x, y) => { return y.Value - x.Value })
        }
        else {
            barchartDataPoints.sort((x, y) => { return (y.Category > x.Category) ? -1 : 1 })
        }

        // return view model to update method
        return {
            IsNotValid: false,
            DataPoints: barchartDataPoints,
            Format: format,
            SortBySize: sortBySize,
            BarColor: barColor,
            XAxisFontSize: xAxisFontSize,
            YAxisFontSize: yAxisFontSize,
            ColumnName: dataView.metadata.columns[1].displayName,
            MeasureName: dataView.metadata.columns[0].displayName
        };

    }

    public enumerateObjectInstances(options: EnumerateVisualObjectInstancesOptions): VisualObjectInstanceEnumeration {
        console.log("EXECUTING: enumerateObjectInstances...");
        var visualObjects: VisualObjectInstanceEnumerationObject = <VisualObjectInstanceEnumerationObject>VisualSettings.enumerateObjectInstances(this.settings, options);
        visualObjects.instances[0].validValues = {
            xAxisFontSize: { numberRange: { min: 7, max: 24 } },
            yAxisFontSize: { numberRange: { min: 7, max: 24 } },
        };
        return visualObjects;
    }

}