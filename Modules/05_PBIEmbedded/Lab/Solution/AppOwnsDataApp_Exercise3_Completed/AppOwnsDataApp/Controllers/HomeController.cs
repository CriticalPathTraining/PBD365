using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AppOwnsDataApp.Models;

namespace AppOwnsDataApp.Controllers {
  public class HomeController : Controller {

    public ActionResult Index() {
      return View();
    }

    public async Task<ActionResult> Report() {
      ReportEmbeddingData embeddingData = await PbiEmbeddingManager.GetReportEmbeddingData();
      return View(embeddingData);
    }

  }
}