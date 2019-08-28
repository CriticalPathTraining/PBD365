// import CSS
import 'bootstrap/dist/css/bootstrap.min.css';
import 'font-awesome/css/font-awesome.css'
import './../css/app.css';

// import JavaScript libraries
import * as $ from 'jquery';
import 'popper.js';
import 'bootstrap';

import {
  ICustomVisual,
  IViewPort,
  VisualGallery
} from './visuals';

let app: App;

$(() => {
  app = new App();
});


class App {

  private leftNavCollapsed: boolean = false;
  private visuals: ICustomVisual[] = VisualGallery.Visuals;
  private loadedVisual = this.visuals[0];

  constructor() {

    // load visual nav links into left nav 
    this.visuals.forEach((visual: ICustomVisual, index: number) => {
      let navLink = this.CreateLeftNavLink(visual.name, visual.icon, index);
      $("#left-nav-list").append(navLink);
    });

    // load first visual in visual gallery
    this.LoadVisual(VisualGallery.Visuals[0]);

    $("#left-nav-toggle").click(()=>{
      this.onNavigationToggle()
    });

    $(window).resize(()=>{
      this.updateUI();
    });

    this.updateUI();

  }

  CreateLeftNavLink(caption: string, icon: string, index: number) {
    let i = $("<i>", { class: "leftNavIcon fa " + icon });
    let span: JQuery = $("<span>", { class: "leftNavItemCaption" }).text(caption);
    let a: JQuery = $("<a>", { id:index.toString(), class: "leftNavLink", href: "javascript:void(0)" })
      .append(i)
      .append(span)
      .click(this.onNavPickerSelect);
    return $("<li>", { class: "leftNavItem" }).append(a);
  }

  onNavigationToggle() {
    this.leftNavCollapsed = !this.leftNavCollapsed;
    this.updateUI();
  }

  onNavPickerSelect(evt) {
    console.log("onNavPickerSelect", evt);
    var targetId: string = evt.currentTarget.id;
    var index: number = parseInt(targetId);
    app.LoadVisual(app.visuals[index])
  }

  LoadVisual(visual: ICustomVisual) {
    console.log("Loading visual: " + visual.name);
    $("#visualName").html("&nbsp;-&nbsp;" + visual.name);
    var vizContainer: HTMLElement = document.getElementById("viz");
    $(vizContainer).empty();
    visual.load(vizContainer);
    this.loadedVisual = visual;
    this.updateUI();
  }


  updateUI() {

    if (this.leftNavCollapsed) {
      $("#left-nav").addClass("navigationPaneCollapsed");
      $("#content-body").addClass("contentBodyNavigationCollapsed");
      $(".leftNavItemCaption").addClass("leftNavHide").hide();
    }
    else {
      $("#left-nav").removeClass("navigationPaneCollapsed");
      $("#content-body").removeClass("contentBodyNavigationCollapsed");
      $(".leftNavItemCaption").removeClass("leftNavHide").show();
    }
  
    var windowHeight = $(window).height();
    var bannerHeight = $("#banner").height();
    $("#left-nav").height(windowHeight - bannerHeight);

    if (this.visuals !== undefined) {

      let paddingWidth: number = 16;
      let paddingHeight: number = 4;
      var viewport: IViewPort = {
        width: $(window).width() - $("#left-nav").width() - paddingWidth,
        height: $(window).height() - $("#banner").height() - paddingHeight
      };

      this.loadedVisual.update(viewport);
    }

  }

}
