module powerbi.extensibility.visual {

    export class Visual implements IVisual {

        private container: JQuery;

        constructor(options: VisualConstructorOptions) {
            this.container = $(options.element);
        }

        public update(options: VisualUpdateOptions) {

            var table: JQuery = $("<table>", { "id": "myTable" })
                .append($("<tr>")
                    .append($("<td>").text("Width"))
                    .append($("<td>").text(options.viewport.width.toFixed(0))
                    ))
                .append($("<tr>")
                    .append($("<td>").text("Height"))
                    .append($("<td>").text(options.viewport.height.toFixed(0))
                    )
                );

            var scaledFontSizeWidth: number = Math.round(options.viewport.width / 7);
            var scaledFontSizeHeight: number = Math.round(options.viewport.height / 5);
            var scaledFontSize: number = Math.min(...[scaledFontSizeWidth, scaledFontSizeHeight]);
            var scaledFontSizeCss: string = scaledFontSize + "px";

            $("td", table).css({
                "font-size": scaledFontSizeCss
            });

            this.container.empty().append(table);

        }
    }
}
