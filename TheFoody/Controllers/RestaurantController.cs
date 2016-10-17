using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheFoody.Models;
using TheFoody.DataAccess;


namespace TheFoody.Controllers
{
    public class RestaurantController : Controller
    {
        // GET: Restaurant
        public ActionResult Index()
        {
            TheFoodyContext db = new TheFoodyContext();
            
                //string query = "select * " 
                //+"from Restaurant r inner join Restaurant_Type t on r.Id = t.Rest_id " 
                //+"inner join Category c on t.Category_id = c.id";
                //var query = from Restaurant in db.Restaurants
                //            from Category in Restaurant.Categories
                //            select new
                //            {
                //                OwnerEmail = Restaurant.OwnerEmail,
                //                RestaurantName = Restaurant.RestaurantName,
                //                Logo = Restaurant.Logo,
                //                City = Restaurant.City,
                //                TimetakenToDeliver = Restaurant.TimetakentoDeliver,
                //                CatName=Category.category1
                //            };
                //IEnumerable<Restaurant> data = db.Database.SqlQuery<Restaurant>(query);
                //var listOfFooId = <IList>.Select(m => m.Id).ToList;
                //return db.Categories.Where(m => m.Restaurants.Any(x => listOfFooId.Contains(x.)));
                //var prod = db.Categories.Where(x => x.Restaurants.Any(c => c.Id == x.id));
                //var prod = db.Categories.SelectMany(c=>c.Restaurants).SelectMany(r=>r.Categories);
                return View(db.Restaurants.ToList());
            
                
        }
    }
}