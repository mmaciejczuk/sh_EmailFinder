using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EmailFinder.Models;

namespace EmailFinder.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "EmailFinder";
            return View(new EmailFinderViewModel());
        }
    }
}
