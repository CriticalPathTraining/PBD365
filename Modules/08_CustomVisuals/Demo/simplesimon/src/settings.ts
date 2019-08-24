module powerbi.extensibility.visual {

  import DataViewObjectsParser = powerbi.extensibility.utils.dataview.DataViewObjectsParser;

  export class VisualSettings extends DataViewObjectsParser {
    public visualProperties: VisualProperties = new VisualProperties();
  }

  export class VisualProperties {
    showName: boolean = true;
    fontSize: number = 14;
    fontColor: Fill = { "solid": { "color": "purple" } };
  }

}
