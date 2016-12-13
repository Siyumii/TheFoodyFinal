using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TheFoody.Controllers
{
    public class FavouriteRestaurantController : Controller
    {
        // GET: FavouriteRestaurant
        public ActionResult FavouriteRestaurant()
        {
            return View();
        }
    }
}