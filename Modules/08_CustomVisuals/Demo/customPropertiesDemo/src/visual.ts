module powerbi.extensibility.visual {

    export class Visual implements IVisual {

        private settings: VisualSettings;

        private svgRoot: d3.Selection<SVGElementInstance>;
        private ellipse: d3.Selection<SVGElementInstance>;
        private text: d3.Selection<SVGElementInstance>;
        private padding: number = 20;

        constructor(options: VisualConstructorOptions) {

            this.svgRoot = d3.select(options.element).append("svg")
                .attr("width", options.element.clientWidth)
                .attr("height", options.element.clientHeight);

            this.ellipse = this.svgRoot.append("ellipse");

            this.text = this.svgRoot.append("text")
                .text("Hello D3")
                .attr("x", options.element.clientWidth / 2)
                .attr("y", options.element.clientHeight / 2);
        }

        public update(options: VisualUpdateOptions) {

            this.settings = Visual.parseSettings(options && options.dataViews && options.dataViews[0]);

            this.text.text(this.settings.myVisualProperties.message);

            this.svgRoot
                .attr("width", options.viewport.width)
                .attr("height", options.viewport.height);

            var plot = {
                xOffset: this.padding,
                yOffset: this.padding,
                width: options.viewport.width - (this.padding * 2),
                height: options.viewport.height - (this.padding * 2),
            };

            this.ellipse
                .attr("cx", plot.xOffset + (plot.width * 0.5))
                .attr("cy", plot.yOffset + (plot.height * 0.5))
                .attr("rx", (plot.width * 0.5))
                .attr("ry", (plot.height * 0.5))
                .attr("fill", this.settings.myVisualProperties.backgroundColor);

            var fontSizeForWidth: number = plot.width * .20;
            var fontSizeForHeight: number = plot.height * .35;
            var fontSize: number = d3.min([fontSizeForWidth, fontSizeForHeight]);

            this.text
                .attr("x", plot.xOffset + (plot.width / 2))
                .attr("y", plot.yOffset + (plot.height / 2))
                .attr("width", plot.width)
                .attr("height", plot.height)
                .attr("font-size", fontSize);
        }

        private static parseSettings(dataView: DataView): VisualSettings {
            return VisualSettings.parse(dataView) as VisualSettings;
        }

        public enumerateObjectInstances(options: EnumerateVisualObjectInstancesOptions): VisualObjectInstance[] | VisualObjectInstanceEnumerationObject {
            return VisualSettings.enumerateObjectInstances(this.settings || VisualSettings.getDefault(), options);
        }        
    
    }
}

