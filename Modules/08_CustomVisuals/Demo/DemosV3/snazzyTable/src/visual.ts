
import powerbi from "powerbi-visuals-api";
import VisualConstructorOptions = powerbi.extensibility.visual.VisualConstructorOptions;
import VisualUpdateOptions = powerbi.extensibility.visual.VisualUpdateOptions;
import IVisual = powerbi.extensibility.visual.IVisual;

import DataView = powerbi.DataView;
import DataViewObjects = powerbi.DataViewObjects;
import DataViewValueColumn = powerbi.DataViewValueColumn;
import DataViewTable = powerbi.DataViewTable;
import DataViewTableRow = powerbi.DataViewTableRow;

import DataViewMetadataColumn = powerbi.DataViewMetadataColumn;
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

import * as $ from "jquery";

import "./../style/visual.less";

export class Visual implements IVisual {


    private rootElement: JQuery;
    private dataView: DataView;
    private hostService: IVisualHost;

    constructor(options: VisualConstructorOptions) {
        this.hostService = options.host;
        this.rootElement = $(options.element);
    }

    public update(options: VisualUpdateOptions) {
        this.rootElement.empty();

        var dataView: DataView = this.dataView = options.dataViews[0];
        if (dataView != null && dataView.table != null) {

            var table: DataViewTable = dataView.table;
            var columns: DataViewMetadataColumn[] = table.columns;
            var rows: DataViewTableRow[] = table.rows;

            var snazzyTable: JQuery = $("<table>", { id: "snazzyTable" });
            var headerRow: JQuery = $("<tr>");
            for (var headerIndex: number = 0; headerIndex < columns.length; headerIndex++) {
                headerRow.append($("<th>").text(columns[headerIndex].displayName));
            }
            snazzyTable.append(headerRow);

            for (var rowIndex: number = 0; rowIndex < rows.length; rowIndex++) {
                var tableRow: JQuery = $("<tr>");
                for (var columnIndex: number = 0; columnIndex < columns.length; columnIndex++) {

                    var valueFormat: string = columns[columnIndex].format;

                    var valueFormatter = vf.create({
                        format: valueFormat,
                        cultureSelector: this.hostService.locale
                    });
                    var value: PrimitiveValue = rows[rowIndex][columnIndex].valueOf();
                    var tableCell: JQuery = $("<td>").text(valueFormatter.format(value));
                    if (columns[columnIndex].type.numeric || columns[columnIndex].type.integer) {
                        tableCell.css({ "text-align": "right" });
                    }
                    if (this.getValue<boolean>(columns[columnIndex].objects, "columnFormatting", "fontBold", false)) {
                        tableCell.css({ "font-weight": "bold" });
                    }
                    tableRow.append(tableCell);
                }
                snazzyTable.append(tableRow);
            }

            var snazzyTableContainer: JQuery = $("<div>", { id: "snazzyTableContainer" });
            snazzyTableContainer.css({
                "width": options.viewport.width,
                "height": options.viewport.height
            });
            snazzyTableContainer.append(snazzyTable);
            this.rootElement.append(snazzyTableContainer);

        }
    }

    public enumerateObjectInstances(options: EnumerateVisualObjectInstancesOptions): VisualObjectInstanceEnumeration {
        let objectName = options.objectName;
        let objectEnumeration: VisualObjectInstance[] = [];

        var metadataColumns: DataViewMetadataColumn[] = this.dataView.metadata.columns;

        switch (objectName) {
            case 'columnFormatting':
                for (var i = 0; i < metadataColumns.length; i++) {
                    var currentColumn: DataViewMetadataColumn = metadataColumns[i];
                    objectEnumeration.push({
                        objectName: objectName,
                        displayName: currentColumn.displayName,
                        properties: {
                            fontBold: this.getValue<boolean>(currentColumn.objects, objectName, "fontBold", false)
                        },
                        selector: { metadata: currentColumn.queryName }
                    });

                };
                break;
        }
        return objectEnumeration;
    }

    public getValue<T>(objects: DataViewObjects, objectName: string, propertyName: string, defaultValue: T): T {
        if (objects) {
            let object = objects[objectName];
            if (object) {
                let property: T = <T>object[propertyName];
                if (property !== undefined) {
                    return property;
                }
            }
        }
        return defaultValue;
    }



}