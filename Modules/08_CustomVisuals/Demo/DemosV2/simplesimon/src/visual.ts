module powerbi.extensibility.visual {

    export interface VisualViewModel {
        IsValid: boolean;
        PlotWidth: number;
        PlotHeight: number;
        FontSize: number;
        FontColor: string;
        OutputDisplay?: string;
    }

    export class Visual implements IVisual {

        private svgRoot: d3.Selection<SVGElementInstance>;
        private text: d3.Selection<SVGElementInstance>;
        private settings: VisualSettings;

        constructor(options: VisualConstructorOptions) {
            console.log("EXECUTING: constructor...");

            this.settings = VisualSettings.getDefault() as VisualSettings;
            console.log("default settings", this.settings);

            this.svgRoot = d3.select(options.element).append("svg");

            this.text = this.svgRoot.append("text")
                .text("Add a measure")
                .attr("text-anchor", "middle")
                .attr("dominant-baseline", "central")
                .attr("x", options.element.clientWidth / 2)
                .attr("y", options.element.clientHeight / 2)
                .attr("fill", this.settings.visualProperties.fontColor.solid.color)
                .attr("font-size", this.settings.visualProperties.fontSize);
;

        }

        public update(options: VisualUpdateOptions) {
            console.log("EXECUTING: update...");

            if (options.dataViews[0]) {
                this.settings = VisualSettings.parse(options.dataViews[0]) as VisualSettings;
            }


            console.log("Parse settings", this.settings);

            var viewModel = this.createViewModel(options);

            this.svgRoot
                .attr("width", viewModel.PlotWidth)
                .attr("height", viewModel.PlotHeight);

            this.text
                .attr("x", viewModel.PlotWidth / 2)
                .attr("y", viewModel.PlotHeight / 2)
                .attr("width", viewModel.PlotWidth)
                .attr("height", viewModel.PlotHeight)
                .attr("fill", viewModel.FontColor)
                .attr("font-size", viewModel.FontSize);


            if (viewModel.IsValid) {
                this.text.text(viewModel.OutputDisplay);
            }
            else {
                this.text.text("View model not valid");
            }
        }

        public createViewModel(options: VisualUpdateOptions): VisualViewModel {
            console.log("EXECUTING: createViewModel...");

            var visualProperties: VisualProperties = this.settings.visualProperties;

            var viewModel: VisualViewModel = {
                IsValid: false,
                PlotWidth: options.viewport.width,
                PlotHeight: options.viewport.height,
                FontSize: visualProperties.fontSize,
                FontColor: <string>visualProperties.fontColor
            };

            var dataView: DataView = options.dataViews[0];

            if (dataView && dataView.single) {

                viewModel.IsValid = true;
                var measureValue: number = <number>dataView.single.value;

                var valueFormatterFactory = powerbi.extensibility.utils.formatting.valueFormatter;
                var valueFormatter = valueFormatterFactory.create({
                    format: dataView.metadata.columns[0].format,
                    formatSingleValues: true
                });

                viewModel.OutputDisplay = valueFormatter.format(measureValue);;
                if (visualProperties.showName) {
                    viewModel.OutputDisplay = dataView.metadata.columns[0].displayName + ": " + viewModel.OutputDisplay;
                }
            }

            return viewModel;
        }

        public enumerateObjectInstances(options: EnumerateVisualObjectInstancesOptions): VisualObjectInstance[] | VisualObjectInstanceEnumerationObject {
            console.log("EXECUTING: enumerateObjectInstances...");

            var visualObjects: VisualObjectInstanceEnumerationObject = <VisualObjectInstanceEnumerationObject>VisualSettings.enumerateObjectInstances(this.settings || VisualSettings.getDefault(), options);
            visualObjects.instances[0].validValues = {
                fontSize: { numberRange: { min: 10, max: 72 } }
            };
            return visualObjects;
        }
    }
}