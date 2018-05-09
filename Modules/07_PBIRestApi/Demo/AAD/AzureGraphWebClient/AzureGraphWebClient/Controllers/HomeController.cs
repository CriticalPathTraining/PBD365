using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AzureGraphWebClient.Controllers {

  public class HomeController : Controller {
    public ActionResult Index() {
      return View();
    }

    public ActionResult About() {
      ViewBag.Message = "AzureGraphWebClient Demo.";
      return View();
    }

  }

}