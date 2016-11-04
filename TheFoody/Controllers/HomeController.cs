using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TheFoody.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (Session["UserEmail"] == null) {

                Session["FirstName"] = "null";
                Session["LastName"] = "null";
                Session["Phone"] = "0111234567";
                Session["Photo"] = "Not Set Yet";
                Session["Address"] = "Not Set Yet";
                Session["City"] = "Not Set Yet";
                Session["PostCode"] = 00000;
                Session["District"] = "Not Set Yet";
                Session["UserType"] = "Admin";
                Session["Status"] = "Active";

            }

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}