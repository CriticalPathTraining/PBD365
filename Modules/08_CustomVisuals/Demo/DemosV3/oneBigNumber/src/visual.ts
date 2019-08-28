import "core-js/stable";
import "./../style/visual.less";

import * as $ from 'jquery';

import powerbi from "powerbi-visuals-api";
import VisualConstructorOptions = powerbi.extensibility.visual.VisualConstructorOptions;
import VisualUpdateOptions = powerbi.extensibility.visual.VisualUpdateOptions;
import IVisual = powerbi.extensibility.visual.IVisual;
import IVisualHost = powerbi.extensibility.visual.IVisualHost;
import EnumerateVisualObjectInstancesOptions = powerbi.EnumerateVisualObjectInstancesOptions;
import VisualObjectInstance = powerbi.VisualObjectInstance;
import VisualObjectInstanceEnumeration = powerbi.VisualObjectInstanceEnumeration;
import DataView = powerbi.DataView;
import DataViewObjects = powerbi.DataViewObjects;
import DataViewMetadataColumn = powerbi.DataViewMetadataColumn;
import Fill = powerbi.Fill;
import VisualObjectInstanceEnumerationObject = powerbi.VisualObjectInstanceEnumerationObject;

import { valueFormatter as vf, textMeasurementService as tms } from "powerbi-visuals-utils-formattingutils";
import IValueFormatter = vf.IValueFormatter;

import { VisualSettings } from "./settings";
export class Visual implements IVisual {

    private rootElement: JQuery;
    private dataView: DataView;
    private hostService: IVisualHost;

    constructor(options: VisualConstructorOptions) {
      this.rootElement = $(options.element);
      this.hostService = options.host;
    }

    public update(options: VisualUpdateOptions) {

      this.dataView = options.dataViews[0];

      this.rootElement.empty();

      if (this.dataView != null) {
              
        var defaultFontColor: Fill = { "solid": { "color": "#000000" } };
        var defaultBackgroundColor: Fill = { "solid": { "color": "#018a80" } };

        var propertyGroups: DataViewObjects = this.dataView.metadata.objects;
        var propertyGroupName: string = "oneBigNumberProperties";
        var showName: boolean = getValue<boolean>(propertyGroups, propertyGroupName, "showName", true);
        var fontBold: string = getValue<boolean>(propertyGroups, propertyGroupName, "fontBold", true) ? "bold": "normal";
        var fontColor: string = getValue<Fill>(propertyGroups, propertyGroupName, "fontColor", defaultFontColor).solid.color;
        var backgroundColor: string = getValue<Fill>(propertyGroups, propertyGroupName, "backgroundColor", defaultBackgroundColor).solid.color;
        var fontType = getValue<string>(this.dataView.metadata.objects, propertyGroupName, "fontType", "boring"); 
        var fontSize = getValue<number>(this.dataView.metadata.objects, propertyGroupName, "fontSize", 24);

        var value: number = <number>this.dataView.single.value;
        var column: DataViewMetadataColumn = this.dataView.metadata.columns[0];
        var valueName: string = column.displayName
        var valueFormat: string = column.format;

        var valueFormatter = vf.create({
            format: valueFormat,
            cultureSelector: this.hostService.locale
        });

        var valueString: string = valueFormatter.format(value);

        var outputString: string = valueString;
        if (showName) {
          outputString = valueName + ": " + outputString;
        }

        var fontName = ""; 
        
        switch(fontType){
          case "boring":
          fontName = "Times New Roman";
          break;
          case "fancy":
          fontName = "Corbel";
          break;
          case "professional":
          fontName = "Franklin Gothic Medium";
          break;
          case "wacky":
          fontName = "Comic Sans MS";
          break;
        };

        var outputDiv = $("<div>")
          .text(outputString)
          .css({
            "display": "table-cell",
            "text-align": "center",
            "vertical-align": "middle",
            "text-wrap": "none",
            "width": options.viewport.width,
            "height": options.viewport.height,
            "padding": "12px",
            "font-weight": fontBold,
            "color": fontColor,
            "background-color": backgroundColor,
            "font-size": fontSize,
            "font-family": fontName
        });
        
        this.rootElement.append(outputDiv);
        
      }
      else {
        this.rootElement.append($("<div>")
          .text("Please add a measure")
          .css({
            "display": "table-cell",
            "text-align": "center",
            "vertical-align": "middle",
            "text-wrap": "none",
            "width": options.viewport.width,
            "height": options.viewport.height,
            "padding": "12px",
            "color": "red"
          }));
      }
    }
    
    private static parseSettings(dataView: DataView): VisualSettings {
        return VisualSettings.parse(dataView) as VisualSettings;
    }

    public enumerateObjectInstances(options: EnumerateVisualObjectInstancesOptions): VisualObjectInstanceEnumeration {

        console.log(options.objectName);
  
        let objectName: string = options.objectName;
        let objectEnumeration: VisualObjectInstance[] = [];
  
        switch (objectName) {
          case 'oneBigNumberProperties':
            objectEnumeration.push({
              objectName: objectName,
              displayName: objectName,
              properties: {
                showName: getValue<boolean>(this.dataView.metadata.objects, objectName, "showName", true),
                fontBold: getValue<boolean>(this.dataView.metadata.objects, objectName, "fontBold", true),
                fontColor: getValue<Fill>(this.dataView.metadata.objects, objectName, "fontColor", { "solid": { "color": "#000000" } }),
                backgroundColor: getValue<Fill>(this.dataView.metadata.objects, objectName, "backgroundColor", { "solid": { "color": "#018a80" } }),
                fontType: getValue<string>(this.dataView.metadata.objects, objectName, "fontType", "boring"),
                fontSize: getValue<number>(this.dataView.metadata.objects, objectName, "fontSize", 24)
               },
              validValues: {
                fontSize: { numberRange: { min: 10, max: 72 } }
              }
              ,
              selector: null
            });
            break;
        };
  
        return objectEnumeration;
      }
}

export function getValue<T>(objects: DataViewObjects, objectName: string, propertyName: string, defaultValue: T): T {
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