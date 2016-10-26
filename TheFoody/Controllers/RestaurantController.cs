using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheFoody.Models;
using TheFoody.DataAccess;
using System.IO;


namespace TheFoody.Controllers
{
    public class RestaurantController : Controller
    {
        private TheFoodyContext db = new TheFoodyContext();

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

        // GET: Restaurant/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Restaurant/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RestaurantModel model)
        {
            if (ModelState.IsValid)
            {
                if (db.Users.Any(u => u.email.Equals(model.email)))
                {
                    //TODO E.g. ModelState.AddModelError
                    ModelState.AddModelError("", "Email already exists");

                }
                else
                {
                    User user = new User();
                    user.email = model.email;
                    HttpPostedFileBase photo = Request.Files["photo"];

                    if (photo != null && photo.ContentLength > 0)
                    {
                        var extension = Path.GetExtension(photo.FileName);
                        user.photo = user.email + extension;
                        var path = Path.Combine(Server.MapPath("~/Uploads/RestaurantOwner"), user.photo);
                        photo.SaveAs(path);
                    }

                    user.fname = model.fname;
                    user.lname = model.lname;
                    user.password = model.password;
                    user.phone = model.phone.ToString();
                    user.address = model.address;
                    user.city = model.city;
                    user.postcode = Convert.ToDecimal(model.postcode);
                    user.district = model.district;
                    user.status = "Active";
                    user.user_type = "RestaurantOwner";
                    user.created_date = DateTime.Now;
                    db.Users.Add(user);
                    db.SaveChanges();

                    //Session["UserEmail"] = model.email;
                    TempData["OwnerEmail"] = model.email;
                    return RedirectToAction("CreateRestaurant", "Restaurant");
                }
            }

            return View(model);
        }

        // GET: Restaurant/CreateRestaurant
        public ActionResult CreateRestaurant()
        {
            RestaurantDeatilModel r = new RestaurantDeatilModel();
            ViewBag.Categories = getCategories();
            return View();
        }



        // POST: Restaurant/CreateRestaurant
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateRestaurant(RestaurantDeatilModel model, FormCollection collection)
        {
            TempData.Keep();

            if (ModelState.IsValid)
            {
                Restaurant restaurant = new Restaurant();
                if (TempData.ContainsKey("OwnerEmail"))
                    restaurant.OwnerEmail = TempData["OwnerEmail"].ToString();
               
                if (restaurant.OwnerEmail != null)
                {
                    if (db.Users.Any(u => u.email.Equals(restaurant.OwnerEmail)))
                    {
                        if (collection["job_type_checkbox"] != null)
                        {
                            HttpPostedFileBase photo = Request.Files["logo"];
                            restaurant.RestaurantName = model.RestaurantName;

                            if (photo != null && photo.ContentLength > 0)
                            {
                                var extension = Path.GetExtension(photo.FileName);
                                restaurant.Logo = restaurant.OwnerEmail + "_" + restaurant.RestaurantName + extension;
                                var path = Path.Combine(Server.MapPath("~/Uploads/RestaurantLogo"), restaurant.Logo);
                                photo.SaveAs(path);
                            }

                            restaurant.Phone = model.Phone.ToString();
                            restaurant.Address = model.Address;
                            restaurant.City = model.City;
                            restaurant.PostCode = Convert.ToDecimal(model.PostCode);
                            restaurant.District = model.District;
                            restaurant.Website = model.Website;
                            restaurant.CompanyBackground = model.CompanyBackground;
                            restaurant.OpeningTime = model.OpeningTime;
                            restaurant.ClosingTime = model.ClosingTime;
                            restaurant.DeliveryStartingTime = model.DeliveryStartingTime;
                            restaurant.DeliveryEndingTime = model.DeliveryEndingTime;
                            restaurant.TimetakentoDeliver = model.TimetakentoDeliver.ToString();

                            db.Restaurants.Add(restaurant);
                            db.SaveChanges();

                            var categories = db.Categories.ToList();
                            model.Categories = new List<CategoryViewModel>();

                            var id = (from p in db.Restaurants
                                      where p.OwnerEmail == restaurant.OwnerEmail
                                      where p.RestaurantName == restaurant.RestaurantName
                                      select p).Single();

                            string type = collection["job_type_checkbox"];
                            string[] tt = type.Split(',');

                            restaurant.Restaurant_Type = new List<Restaurant_Type>();
                            foreach (var category in categories)
                            {
                                for (int i = 0; i < tt.Length; i++)
                                {
                                    if (category.category1 == tt[i])
                                    {
                                        restaurant.Restaurant_Type.Add(new Restaurant_Type { Category_id = category.id, Rest_id = id.Id });

                                    }
                                }
                            }

                            db.SaveChanges();

                            Session["UserEmail"] = restaurant.OwnerEmail;
                            return RedirectToAction("Index", "Home");
                            
                        }
                        else {
                            ModelState.AddModelError("Categories", "Please select atleast one restaurant type");
                        }
                    }
                    else
                    {
                        //TODO E.g. ModelState.AddModelError
                        ModelState.AddModelError("", "Owner have not been registered successfully");

                    }
                }
            }
            ViewBag.Categories = getCategories() ;
            return View();
        }

        [NonAction]
        public List<CategoryViewModel> getCategories()
        {
           
            var dbCategories = db.Categories.ToList();

            var categories = new List<CategoryViewModel>();

            foreach (var category in dbCategories)
            {
                categories.Add(new CategoryViewModel()
                {
                    id = category.id.ToString(),
                    category = category.category1,
                    isChecked = false //On the add view, no genres are selected by default
                });
            }

            return categories;
        }

    }
  
}