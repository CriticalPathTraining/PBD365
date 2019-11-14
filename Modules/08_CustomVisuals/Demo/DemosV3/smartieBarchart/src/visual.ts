import "./../style/visual.less";

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

import ISelectionIdBuilder = powerbi.extensibility.ISelectionIdBuilder;
import ISelectionId = powerbi.visuals.ISelectionId;
import ISelectionManager = powerbi.extensibility.ISelectionManager;


import { valueFormatter as vf, textMeasurementService as tms } from "powerbi-visuals-utils-formattingutils";
import IValueFormatter = vf.IValueFormatter;

import { VisualSettings, BarchartProperties } from './settings';

import * as d3 from "d3";
type Selection<T extends d3.BaseType> = d3.Selection<T, any, any, any>;
type DataSelection<T> = d3.Selection<d3.BaseType, T, any, any>;

export interface BarchartDataPoint {
    Category: string;
    Value: number;
    Opacity: number;
    selectionId: ISelectionId;
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

export class SmartieBarchart implements IVisual {

    private svg: Selection<SVGElement>;
    private barContainer: Selection<SVGGElement>;
    private plotBackground: Selection<SVGRectElement>;
    private barSelection: DataSelection<BarchartDataPoint>;
    private xAxisContainer: Selection<SVGGElement>;
    private yAxisContainer: Selection<SVGGElement>;

    private hostService: IVisualHost;
    private locale: string;
    private selectionManager: ISelectionManager;

    private settings: VisualSettings;

    private viewModel: BarchartViewModel;
    private barDataPoints: BarchartDataPoint[];

    private static margin = {
        top: 20,
        right: 20,
        bottom: 20,
        left: 50
    };

    constructor(options: VisualConstructorOptions) {

        console.log("Constructor executing", options);

        this.hostService = options.host;
        this.locale = options.host.locale;
        this.selectionManager = options.host.createSelectionManager();

        this.svg = d3.select(options.element)
            .append('svg')
            .classed('barChart', true);

        this.barContainer = this.svg
            .append('g')
            .classed('barContainer', true);

        this.plotBackground = this.barContainer
            .append('rect')
            .classed('plotBackground', true);

        this.xAxisContainer = this.svg
            .append('g')
            .classed('xAxis', true);

        this.yAxisContainer = this.svg
            .append('g')
            .classed('yAxis', true);

        this.settings = VisualSettings.getDefault() as VisualSettings;

        this.selectionManager.registerOnSelectCallback(() => {
            this.syncSelectionState(this.barSelection, this.selectionManager.getSelectionIds() as ISelectionId[]);
        });

    }

    public update(options: VisualUpdateOptions) {

        console.log("Update executing...", options);

        var viewModel: BarchartViewModel = this.createViewModel(options.dataViews[0]);
        if (viewModel.IsNotValid) {
            return;
        }

        this.barDataPoints = viewModel.DataPoints;

        // set height and width of root SVG element using viewport passed by Power BI host
        this.svg.attr("height", options.viewport.height);
        this.svg.attr("width", options.viewport.width);

        let marginLeft = SmartieBarchart.margin.left * (viewModel.YAxisFontSize / 10);
        let marginBottom = SmartieBarchart.margin.bottom * (viewModel.XAxisFontSize / 10);
        let marginTop = SmartieBarchart.margin.top;
        let marginRight = SmartieBarchart.margin.right;

        let plotArea = {
            x: marginLeft,
            y: marginTop,
            width: options.viewport.width - (marginLeft + SmartieBarchart.margin.right),
            height: options.viewport.height - (marginTop + marginBottom)
        };

        this.barContainer
            .attr("transform", "translate(" + plotArea.x + "," + plotArea.y + ")");

        this.plotBackground.attr("width", plotArea.width);
        this.plotBackground.attr("height", plotArea.height);

        var xScale = d3.scaleBand()
            .rangeRound([0, plotArea.width])
            .padding(0.1)
            .domain(viewModel.DataPoints.map((dataPoint: BarchartDataPoint) => dataPoint.Category));

        this.xAxisContainer
            .attr("class", "xAxis")
            .attr("transform", "translate(" + plotArea.x + "," + (plotArea.height + plotArea.y) + ")")
            .call(d3.axisBottom(xScale));

        d3.select(".xAxis").selectAll("text").style("font-size", viewModel.XAxisFontSize);

        let maxValueY: number = d3.max(viewModel.DataPoints, (dataPoint: BarchartDataPoint) => +(dataPoint.Value));

        var valueFormatter = vf.create({
            format: viewModel.Format,
            value: maxValueY / 100,
            cultureSelector: this.hostService.locale
        });

        var yScale = d3.scaleLinear()
            .rangeRound([plotArea.height, 0])
            .domain([0, maxValueY * 1.02]);

        var yAxis = d3.axisLeft(yScale)
            .tickFormat((d) => valueFormatter.format(d));

        this.yAxisContainer
            .attr("class", "yAxis")
            .attr("transform", "translate(" + plotArea.x + "," + plotArea.y + ")")
            .call(yAxis);


        d3.select(".yAxis").selectAll("text").style("font-size", viewModel.YAxisFontSize);

        this.barSelection = this.barContainer
            .selectAll('.bar')
            .data(viewModel.DataPoints);

        const barSelectionMerged = this.barSelection
            .enter()
            .append('rect')
            .merge(<any>this.barSelection)
            .classed('bar', true);

        barSelectionMerged
            .attr("x", (dataPoint: BarchartDataPoint) => xScale(dataPoint.Category))
            .attr("y", (dataPoint: BarchartDataPoint) => yScale(Number(dataPoint.Value)))
            .attr("width", xScale.bandwidth())
            .attr("height", (dataPoint: BarchartDataPoint) => (plotArea.height - yScale(Number(dataPoint.Value))))
            .style("fill", (dataPoint: BarchartDataPoint) => viewModel.BarColor)
            .style("opacity", (dataPoint: BarchartDataPoint) => dataPoint.Opacity);

        barSelectionMerged.on('click', (d) => {
            const isCtrlPressed: boolean = (d3.event as MouseEvent).ctrlKey;
            this.selectionManager.select(d.selectionId, isCtrlPressed)
                .then((ids: ISelectionId[]) => {
                    this.syncSelectionState(barSelectionMerged, ids);
                });
            (<Event>d3.event).stopPropagation();
        });

        this.barSelection
            .exit()
            .remove();

    }

    private syncSelectionState(selection: DataSelection<BarchartDataPoint>, selectionIds: ISelectionId[]): void {

        console.log("syncSelectionState executing...")
        if (!selection || !selectionIds) { return; }

        if (!selectionIds.length) {
            selection
                .style("fill-opacity", null)
                .style("stroke-opacity", null);
            return;
        }

        const self: this = this;

        selection.each(function (barDataPoint: BarchartDataPoint) {

            const isSelected: boolean = self.isSelectionIdInArray(selectionIds, barDataPoint.selectionId);

            const opacity: number = isSelected ? 1.0 : 0.5;

            d3.select(this).transition().duration(500)
                .style("fill-opacity", opacity)
                .style("stroke-opacity", opacity);
        });
    }

    private isSelectionIdInArray(selectionIds: ISelectionId[], selectionId: ISelectionId): boolean {
        if (!selectionIds || !selectionId) {
            return false;
        }

        return selectionIds.some((currentSelectionId: ISelectionId) => {
            return currentSelectionId.includes(selectionId);
        });
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

        var categoricalDataView: DataViewCategorical = dataView.categorical;
        var categoryColumn: DataViewCategoryColumn = categoricalDataView.categories[0];
        var categoryNames: PrimitiveValue[] = categoricalDataView.categories[0].values;
        var categoryValues: PrimitiveValue[] = categoricalDataView.values[0].values;
        // use hightlights to determine which datapoints are selected
        var categoryHighlightedValues: PrimitiveValue[] = categoricalDataView.values[0].highlights;

        var barchartDataPoints: BarchartDataPoint[] = [];

        for (var i = 0; i < categoryValues.length; i++) {

            // get category name and category value
            var category: string = <string>categoryNames[i];
            var categoryValue: number = <number>categoryValues[i];
            //
            var HighlightedValueIsNull: boolean = (categoryHighlightedValues != null) && (categoryHighlightedValues[i] == null);
            var opacity: number = (HighlightedValueIsNull ? 0.5 : 1.0);

            let selectionId: ISelectionId =
                <ISelectionId>this.hostService
                    .createSelectionIdBuilder()
                    .withCategory(categoryColumn, i)
                    .createSelectionId();


            // add new data point to barchartDataPoints collection
            barchartDataPoints.push({
                Category: category,
                Value: categoryValue,
                Opacity: opacity,
                selectionId: selectionId
            });
        }

        // get formatting code for the field that is the measure
        var format: string = categoricalDataView.values[0].source.format;

        // get persistent property values
        var sortBySize: boolean = this.settings.barchartProperties.sortBySize;
        var xAxisFontSize: number = this.settings.barchartProperties.xAxisFontSize;
        var yAxisFontSize: number = this.settings.barchartProperties.yAxisFontSize;
        var barColor: string = typeof (this.settings.barchartProperties.barColor) == "string" ?
            this.settings.barchartProperties.barColor :
            this.settings.barchartProperties.barColor.solid.color;

        // sort dataset rows by measure value instead of category value
        if (sortBySize) {
            barchartDataPoints.sort((x, y) => { return y.Value - x.Value })
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

        var visualObjects: VisualObjectInstanceEnumerationObject = <VisualObjectInstanceEnumerationObject>VisualSettings.enumerateObjectInstances(this.settings, options);

        visualObjects.instances[0].validValues = {
            xAxisFontSize: { numberRange: { min: 7, max: 24 } },
            yAxisFontSize: { numberRange: { min: 7, max: 24 } },
        };

        return visualObjects;
    }

}