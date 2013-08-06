using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InversionOfControl.Web.Controllers {
  public class HomeController : Controller {
    private string _message;

    public HomeController(IBox box) {
      this._message = box.Name;
    }

    public ActionResult Index() {
      ViewBag.Message = this._message;
      return View();
    }

  }
}
