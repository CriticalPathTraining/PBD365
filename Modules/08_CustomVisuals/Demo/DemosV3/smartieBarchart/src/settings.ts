import { dataViewObjectsParser } from "powerbi-visuals-utils-dataviewutils";
import DataViewObjectsParser = dataViewObjectsParser.DataViewObjectsParser;

import powerbi from "powerbi-visuals-api";
import Fill = powerbi.Fill;

export class VisualSettings extends DataViewObjectsParser {
  public barchartProperties: BarchartProperties = new BarchartProperties();
}

export class BarchartProperties {
  sortBySize: boolean = true;
  xAxisFontSize: number = 10;
  yAxisFontSize: number = 10;
  barColor: Fill = { "solid": { "color": "#018a80" } }; // default color is  teal
}

