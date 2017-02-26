/*
 *  Power BI Visual CLI
 *
 *  Copyright (c) Microsoft Corporation
 *  All rights reserved.
 *  MIT License
 *
 *  Permission is hereby granted, free of charge, to any person obtaining a copy
 *  of this software and associated documentation files (the ""Software""), to deal
 *  in the Software without restriction, including without limitation the rights
 *  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 *  copies of the Software, and to permit persons to whom the Software is
 *  furnished to do so, subject to the following conditions:
 *
 *  The above copyright notice and this permission notice shall be included in
 *  all copies or substantial portions of the Software.
 *
 *  THE SOFTWARE IS PROVIDED *AS IS*, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 *  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 *  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 *  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 *  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 *  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 *  THE SOFTWARE.
 */
var powerbi;
(function (powerbi) {
    var extensibility;
    (function (extensibility) {
        var visual;
        (function (visual) {
            var PBI_CV_A4A9F806_8937_4049_A1A6_4804BF4C04CD;
            (function (PBI_CV_A4A9F806_8937_4049_A1A6_4804BF4C04CD) {
                var Visual = (function () {
                    function Visual(options) {
                        console.log('Visual constructor', options);
                        this.target = options.element;
                        this.updateCount = 0;
                    }
                    Visual.prototype.update = function (options) {
                        console.log('Visual update', options);
                        this.target.innerHTML =
                            "<table>\n          <tr><td>Width:</td><td>" + options.viewport.width.toFixed(2) + "</td></tr>\n          <tr><td>Height:</td><td>" + options.viewport.height.toFixed(2) + "</td></tr>\n        </table>";
                    };
                    return Visual;
                }());
                PBI_CV_A4A9F806_8937_4049_A1A6_4804BF4C04CD.Visual = Visual;
            })(PBI_CV_A4A9F806_8937_4049_A1A6_4804BF4C04CD = visual.PBI_CV_A4A9F806_8937_4049_A1A6_4804BF4C04CD || (visual.PBI_CV_A4A9F806_8937_4049_A1A6_4804BF4C04CD = {}));
        })(visual = extensibility.visual || (extensibility.visual = {}));
    })(extensibility = powerbi.extensibility || (powerbi.extensibility = {}));
})(powerbi || (powerbi = {}));
var powerbi;
(function (powerbi) {
    var visuals;
    (function (visuals) {
        var plugins;
        (function (plugins) {
            plugins.PBI_CV_A4A9F806_8937_4049_A1A6_4804BF4C04CD_DEBUG = {
                name: 'PBI_CV_A4A9F806_8937_4049_A1A6_4804BF4C04CD_DEBUG',
                displayName: 'viz01',
                class: 'Visual',
                version: '1.0.0',
                apiVersion: '1.5.0',
                create: function (options) { return new powerbi.extensibility.visual.PBI_CV_A4A9F806_8937_4049_A1A6_4804BF4C04CD.Visual(options); },
                custom: true
            };
        })(plugins = visuals.plugins || (visuals.plugins = {}));
    })(visuals = powerbi.visuals || (powerbi.visuals = {}));
})(powerbi || (powerbi = {}));
//# sourceMappingURL=visual.js.map