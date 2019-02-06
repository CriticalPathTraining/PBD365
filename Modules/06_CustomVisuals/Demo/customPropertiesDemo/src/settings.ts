module powerbi.extensibility.visual {

  import DataViewObjectsParser = powerbi.extensibility.utils.dataview.DataViewObjectsParser;

  export class VisualSettings extends DataViewObjectsParser {
    public myVisualProperties: MyVisualProperties = new MyVisualProperties();
  }

  export class MyVisualProperties {
    public message: string = "Hello D3";
    public backgroundColor: string = "#f2c80f";
  }

}
