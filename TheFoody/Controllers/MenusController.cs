using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheFoody.DataAccess;

namespace TheFoody.Controllers
{
    public class MenusController : Controller
    {
        TheFoodyContext db = new TheFoodyContext();
        
        // GET: Menus
        public ActionResult Index()
        {
            return View(db.Menus.ToList());
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]

        public ActionResult Create(Menu menu)
        {
            if (ModelState.IsValid)
            {
                db.Menus.Add(menu);
                db.SaveChanges();
                return RedirectToAction("Index");

            }
            return View(menu);
        }
    }
}