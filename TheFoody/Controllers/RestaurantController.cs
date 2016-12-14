using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Web.Mvc;
using TheFoody.Models;
using TheFoody.DataAccess;
using System.IO;
using System.Threading.Tasks;
using System.Data.Entity;
using Mvc.Mailer;


namespace TheFoody.Controllers
{
    public class RestaurantController : Controller
    {
        TheFoodyContext db = new TheFoodyContext();

        [HttpPost]
        public ActionResult searchOpen_Restaurants(int openFlag)
        {


            TimeSpan currentTime = DateTime.Now.TimeOfDay;

            if (openFlag == 1)
            {
                var model = (from p in db.Restaurants
                             where ((p.OpeningTime <= currentTime) &&( p.ClosingTime >= currentTime))
                             select new RestaurantViewModel()
                             {
                                 RestId = p.Id,
                                 RestaurantName = p.RestaurantName,
                                 Logo = p.Logo,
                                 Address = p.Address,
                                 City = p.City,
                                 District = p.District,
                                 TimetakentoDeliver = p.TimetakentoDeliver,
                                 categories = p.Restaurant_Type.Select(a => a.Category.category1).ToList(),
                             });
                var no = model.Count();
                return PartialView("_RestaurantViewer", model.ToList());

            }
            else
                return RedirectToAction("Index");

        }

        [HttpPost]
        public ActionResult searchDelivery_Restaurants(int delivery)
        {
            TimeSpan currentTime = DateTime.Now.TimeOfDay;
            if (delivery == 2)
            {
                var model = (from p in db.Restaurants
                             where ((p.DeliveryStartingTime <= currentTime) &&( p.ClosingTime >= currentTime))
                             select new RestaurantViewModel()
                             {
                                 RestId = p.Id,
                                 RestaurantName = p.RestaurantName,
                                 Logo = p.Logo,
                                 Address = p.Address,
                                 City = p.City,
                                 District = p.District,
                                 TimetakentoDeliver = p.TimetakentoDeliver,
                                 categories = p.Restaurant_Type.Select(a => a.Category.category1).ToList(),
                             });
                var no = model.Count();
                return PartialView("_RestaurantViewer", model.ToList());

            }
            else
                return RedirectToAction("Index");

        }

        [HttpPost]
        public ActionResult searchReservation_Restaurants(int reservation)
        {
            TimeSpan currentTime = DateTime.Now.TimeOfDay;
            if (reservation == 3)
            {
                var model = (from p in db.Restaurants
                             
                             select new RestaurantViewModel()
                             {
                                 RestId = p.Id,
                                 RestaurantName = p.RestaurantName,
                                 Logo = p.Logo,
                                 Address = p.Address,
                                 City = p.City,
                                 District = p.District,
                                 TimetakentoDeliver = p.TimetakentoDeliver,
                                 categories = p.Restaurant_Type.Select(a => a.Category.category1).ToList(),
                             });
                var no = model.Count();
                return PartialView("_RestaurantViewer", model.ToList());

            }
            else
                return RedirectToAction("Index");

        }

        [HttpPost]
        //get values to meal category dropdoen from db
        public JsonResult getCuisineListFromDb()
        {

            var getCuisineList = (from m in db.Categories
                                  select new CategoryViewModel()
                                  {
                                      id = m.id.ToString(),
                                      category = m.category1
                                  }).ToList();

            SelectList list = new SelectList(getCuisineList, "id", "category");
            //ViewBag.cuisineList = list;
            return Json(list, JsonRequestBehavior.AllowGet);
        }


        // GET: Restaurant
        [HttpGet]
        public ActionResult Index()
        {

            var model = (from p in db.Restaurants // .Includes("Addresses") here?
                         select new RestaurantViewModel()
                         {
                             RestId = p.Id,
                             RestaurantName = p.RestaurantName,
                             Logo = p.Logo,
                             Address = p.Address,
                             City = p.City,
                             District = p.District,
                             TimetakentoDeliver = p.TimetakentoDeliver,
                             categories = p.Restaurant_Type.Select(a => a.Category.category1).ToList(),
                         });

            return View(model.ToList());
        }

        //POST:Restaurant
        [HttpPost]
        public ActionResult Index(string search)
        {
            //using (TheFoodyContext db = new TheFoodyContext())
            //{

            var model = (from p in db.Restaurants // .Includes("Addresses") here?
                         where p.City.StartsWith(search) || search == null
                         select new RestaurantViewModel()
                         {
                             RestId = p.Id,
                             RestaurantName = p.RestaurantName,
                             Logo = p.Logo,
                             Address = p.Address,
                             City = p.City,
                             District = p.District,
                             TimetakentoDeliver = p.TimetakentoDeliver,
                             categories = p.Restaurant_Type.Select(a => a.Category.category1).ToList(),
                         });

            return View(model.ToList());

            //}

        }
   
        [HttpPost]
        public string ratingResponse()
        {
            TheFoodyContext db = new TheFoodyContext();
            int restuarantId = Convert.ToInt16(Request["RestuarantId"]);
            string userEmail = Request["UserEmail"];
            int rating = Convert.ToInt16(Request["Rating"]);
            string review = Request["Review"];
            DateTime currentDateTime = DateTime.Now;
            string created_Date = currentDateTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
            Rating rate = new Rating();
            rate.RestuarantId = restuarantId;
            rate.UserEmail = userEmail;
            rate.Rating1 = rating;
            rate.Review = review;
            rate.Created_Date = currentDateTime;
            db.Ratings.Add(rate);
            db.SaveChanges();

            int ratings = db.Ratings.Where(u => u.Id >= 0).Count();
            int id = restuarantId;

            string rev = Session["ReviewHtml"].ToString();

            for (int x = 5; x > 0; x--)
            {
                Rating theRating = db.Ratings.Where(u => u.Id == ratings && u.RestuarantId == id).FirstOrDefault();
                if (theRating == null) { continue; }
                rev = rev + "<div class='review-list'><div class='clearfix'><div class='pull-left'><h6><i class='fa fa-calendar'></i>"
                    + theRating.Created_Date.ToString()
                    + "</h6><h6>By " + theRating.UserEmail.ToString() + "</h6><ul class='list-unstyled list-inline rating-star-list'>";
                switch (theRating.Rating1)
                {
                    case 0:
                        rev = rev + "<li><i class='fa fa-star-o'></i></li><li><i class='fa fa-star-o'></i></li><li><i class='fa fa-star-o'></i></li><li><i class='fa fa-star-o'></i></li><li><i class='fa fa-star-o'></i></li>";
                        break;
                    case 1:
                        rev = rev + "<li><i class='fa fa-star'></i></li><li><i class='fa fa-star-o'></i></li><li><i class='fa fa-star-o'></i></li><li><i class='fa fa-star-o'></i></li><li><i class='fa fa-star-o'></i></li>";
                        break;
                    case 2:
                        rev = rev + "<li><i class='fa fa-star'></i></li><li><i class='fa fa-star'></i></li><li><i class='fa fa-star-o'></i></li><li><i class='fa fa-star-o'></i></li><li><i class='fa fa-star-o'></i></li>";
                        break;
                    case 3:
                        rev = rev + "<li><i class='fa fa-star'></i></li><li><i class='fa fa-star'></i></li><li><i class='fa fa-star'></i></li><li><i class='fa fa-star-o'></i></li><li><i class='fa fa-star-o'></i></li>";
                        break;
                    case 4:
                        rev = rev + "<li><i class='fa fa-star'></i></li><li><i class='fa fa-star'></i></li><li><i class='fa fa-star'></i></li><li><i class='fa fa-star'></i></li><li><i class='fa fa-star-o'></i></li>";
                        break;
                    case 5:
                        rev = rev + "<li><i class='fa fa-star'></i></li><li><i class='fa fa-star'></i></li><li><i class='fa fa-star'></i></li><li><i class='fa fa-star'></i></li><li><i class='fa fa-star'></i></li>";
                        break;
                }
                rev = rev + "</ul></div><img src='@Session[\"Path\"].ToString()' alt='Image' class='img-responsive pull-right'></div><div class='review-list-content'><p>";
                rev = rev + theRating.Review.ToString() + "</p></div></div>";
                ratings++;
            }
            return rev;
        }

        public ActionResult ViewMenu(int id)
        {
            TheFoodyContext db = new TheFoodyContext();
            int ratings = db.Ratings.Where(u => u.Id >= 0).Count();
            Session["CurrentRestaurentId"] = id;

            string review = "<!-- -->";

            for (int x = 5; x > 0 ; x-- )
            {
                Rating theRating = db.Ratings.Where(u => u.Id == ratings && u.RestuarantId == id).FirstOrDefault();
                if (theRating == null) { continue; }
                review = review + "<div class='review-list'><div class='clearfix'><div class='pull-left'><h6><i class='fa fa-calendar'></i>"
                    + theRating.Created_Date.ToString()
                    + "</h6><h6>By " + theRating.UserEmail.ToString() + "</h6><ul class='list-unstyled list-inline rating-star-list'>";
                switch (theRating.Rating1)
                {
                    case 0:
                        review = review + "<li><i class='fa fa-star-o'></i></li><li><i class='fa fa-star-o'></i></li><li><i class='fa fa-star-o'></i></li><li><i class='fa fa-star-o'></i></li><li><i class='fa fa-star-o'></i></li>";
                        break;
                    case 1:
                        review = review + "<li><i class='fa fa-star'></i></li><li><i class='fa fa-star-o'></i></li><li><i class='fa fa-star-o'></i></li><li><i class='fa fa-star-o'></i></li><li><i class='fa fa-star-o'></i></li>";
                        break;
                    case 2:
                        review = review + "<li><i class='fa fa-star'></i></li><li><i class='fa fa-star'></i></li><li><i class='fa fa-star-o'></i></li><li><i class='fa fa-star-o'></i></li><li><i class='fa fa-star-o'></i></li>";
                        break;
                    case 3:
                        review = review + "<li><i class='fa fa-star'></i></li><li><i class='fa fa-star'></i></li><li><i class='fa fa-star'></i></li><li><i class='fa fa-star-o'></i></li><li><i class='fa fa-star-o'></i></li>";
                        break;
                    case 4:
                        review = review + "<li><i class='fa fa-star'></i></li><li><i class='fa fa-star'></i></li><li><i class='fa fa-star'></i></li><li><i class='fa fa-star'></i></li><li><i class='fa fa-star-o'></i></li>";
                        break;
                    case 5:
                        review = review + "<li><i class='fa fa-star'></i></li><li><i class='fa fa-star'></i></li><li><i class='fa fa-star'></i></li><li><i class='fa fa-star'></i></li><li><i class='fa fa-star'></i></li>";
                        break;
                }
                review = review + "</ul></div><img src='@Session[\"Path\"].ToString()' alt='Image' class='img-responsive pull-right'></div><div class='review-list-content'><p>";
                review = review + theRating.Review.ToString() + "</p></div></div>";
                ratings++;
            }

            Session["ReviewHtml"] = review;


            RestaurantViewModel restaurantVm = new RestaurantViewModel();
            using (TheFoodyContext context = new TheFoodyContext())
            {
                Restaurant restaurant = context.Restaurants.Where(x => x.Id == id).SingleOrDefault();
                Session["MinDelivery"] = restaurant.MinDelivery;
                restaurantVm = TransformToRestaurantVm(restaurant);

                //get meal categories
                List<Meal_Category> mealCategoryList = context.Menus
                    .Where(x => x.RestaurantId == id)
                    .Select(x => x.Meal_Category).Distinct().ToList();

                restaurantVm.MealCategories = new List<MealCategoryVm>();

                for (int i = 0; i < mealCategoryList.Count; i++)
                {
                    restaurantVm.MealCategories.Add(TransformToMealCategoryVm(mealCategoryList[i]));
                }

                List<Menu> menuList = context.Menus.Where(x => x.RestaurantId == id).ToList();

                restaurantVm.MenuList = new List<MenuVm>();

                for (int j = 0; j < menuList.Count; j++)
                {
                    restaurantVm.MenuList.Add(TransformToMenuVm(menuList[j]));
                }
            }

            return View(restaurantVm);
        }

        //

        public PartialViewResult GetMenus(int? restId, int? categoryId)
        {
            List<MenuVm> menuVmList = new List<MenuVm>();
            using (TheFoodyContext context = new TheFoodyContext())
            {
                List<Menu> menuList = context.Menus.Where(x => x.RestaurantId == restId && x.Meal_Cat_IdFK == categoryId).ToList();

                for (int j = 0; j < menuList.Count; j++)
                {
                    menuVmList.Add(TransformToMenuVm(menuList[j]));
                }
            }

            return PartialView("_MenuList", menuVmList);
        }


        //public ActionResult Item(int id)
        //{
        //    RestaurantViewModel restaurantVm = new RestaurantViewModel();
        //    using (TheFoodyContext context = new TheFoodyContext())
        //    {
        //        Restaurant restaurant = context.Restaurants.Where(x => x.Id == id).SingleOrDefault();

        //        restaurantVm = TransformToRestaurantVm(restaurant);

        //        //get meal categories
        //        List<Meal_Category> mealCategoryList = context.Menus
        //            .Where(x => x.RestaurantId == id)
        //            .Select(x => x.Meal_Category).Distinct().ToList();

        //        restaurantVm.MealCategories = new List<MealCategoryVm>();

        //        for (int i = 0; i < mealCategoryList.Count; i++)
        //        {
        //            restaurantVm.MealCategories.Add(TransformToMealCategoryVm(mealCategoryList[i]));
        //        }

        //        List<Menu> menuList = context.Menus.Where(x => x.RestaurantId == id).ToList();

        //        restaurantVm.MenuList = new List<MenuVm>();

        //        for (int j = 0; j < menuList.Count; j++)
        //        {
        //            restaurantVm.MenuList.Add(TransformToMenuVm(menuList[j]));
        //        }
        //    }

        //    return View(restaurantVm);
        //}

        private MenuVm TransformToMenuVm(Menu menu)
        {
            if (menu == null)
                return null;

            MenuVm menuVm = new MenuVm();
            menuVm.MenuID = menu.Menu_id;
            menuVm.MenuName = menu.Menu_name;
            menuVm.MenuDescription = menu.Description;
            menuVm.MenuPhoto = menu.Photo;
            menuVm.MenuPrice = Convert.ToDouble(menu.Price);
            
            if (menu.Meal_Cat_IdFK.HasValue)
                menuVm.MealCategoryID = menu.Meal_Cat_IdFK.Value;

            return menuVm;
        }

        private MealCategoryVm TransformToMealCategoryVm(Meal_Category category)
        {
            if (category == null)
                return null;

            MealCategoryVm categoryVm = new MealCategoryVm();
            categoryVm.MealCategoryID = category.Meal_Cat_Id;
            categoryVm.MealCategoryName = category.CategoryName;

            return categoryVm;
        }

        private RestaurantViewModel TransformToRestaurantVm(Restaurant restaurant)
        {
            if (restaurant == null)
                return null;

            RestaurantViewModel restaurantVm = new RestaurantViewModel();

            restaurantVm.Address = restaurant.Address;
            restaurantVm.categories = restaurant.Restaurant_Type.Select(x => x.Category.category1).ToList();
            restaurantVm.City = restaurant.City;
            restaurantVm.District = restaurant.District;
            restaurantVm.Logo = restaurant.Logo;
            restaurantVm.RestaurantName = restaurant.RestaurantName;
            restaurantVm.RestId = restaurant.Id;
            restaurantVm.TimetakentoDeliver = restaurant.TimetakentoDeliver;
            restaurantVm.MinDelivery = Convert.ToDecimal(restaurant.MinDelivery);

            return restaurantVm;
        }

        public PartialViewResult AddtoCart(int id)
        {
            if (Session["Cart"] == null)
            {
                List<Item> itemList = new List<Item>();
                TheFoodyContext context = new TheFoodyContext();
                //{
                //List<Menu> menulist = context.Menus.Where(x => x.Menu_id == id).ToList() ;
                itemList.Add(new Item(TransformToCartItem(context.Menus.Find(id))));
                //itemList.Add(TransformToCartItem(menulist[0]));
                Session["Cart"] = itemList;
                //}
                return PartialView("_AddtoCart");
            }
            else
            {
                List<Item> itemList = (List<Item>)Session["Cart"];
                TheFoodyContext context = new TheFoodyContext();
                //{
                //List<Menu> menulist = context.Menus.Where(x => x.Menu_id == id).ToList();
                itemList.Add(new Item(TransformToCartItem(context.Menus.Find(id))));
                Session["Cart"] = itemList;
                //}
                return PartialView("_AddtoCart");
            }

        }

        public PartialViewResult RemovefromCart(Item item)
        {
            //if (Session["Cart"] == null)
            //{
            //    List<Item> itemList = new List<Item>();
            //    TheFoodyContext context = new TheFoodyContext();
            //    //{
            //    //List<Menu> menulist = context.Menus.Where(x => x.Menu_id == id).ToList() ;
            //    itemList.Add(new Item(TransformToCartItem(context.Menus.Find(id)), 1));
            //    //itemList.Add(TransformToCartItem(menulist[0]));
            //    Session["Cart"] = itemList;
            //    //}
            //    return PartialView("_AddtoCart");
            //}
            //else
            //{
                List<Item> itemList = (List<Item>)Session["Cart"];
                TheFoodyContext context = new TheFoodyContext();
            //{
            //List<Menu> menulist = context.Menus.Where(x => x.Menu_id == id).ToList();
            //itemList.Remove(item);
            int index=itemList.IndexOf(item);
            itemList.RemoveAt(index);

            Session["Cart"] = itemList;
                //}
            return PartialView("_AddtoCart");
            //}

        }

        private CartItem TransformToCartItem(Menu menu)
        {
            if (menu == null)
                return null;

            CartItem item = new CartItem();
            item.MenuID = menu.Menu_id;
            item.MenuName = menu.Menu_name;
            item.MenuPrice = Convert.ToDouble(menu.Price);

            //if (menu.Meal_Cat_IdFK.HasValue)
            //    item.MealCategoryID = menu.Meal_Cat_IdFK.Value;

            return item;
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
                    user.postcode = model.postcode;
                    user.district = model.district;
                    user.status = "Inactive";
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

        // GET: Restaurant/CreateRestaurant
        public ActionResult MoveToHome()
        {
            return View("RestaurantRegistration");
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
                            restaurant.PostCode = model.PostCode.ToString();
                            restaurant.District = model.District;
                            restaurant.Website = model.Website;
                            restaurant.CompanyBackground = model.CompanyBackground;
                            restaurant.OpeningTime = model.OpeningTime;
                            restaurant.ClosingTime = model.ClosingTime;
                            restaurant.DeliveryStartingTime = model.DeliveryStartingTime;
                            restaurant.DeliveryEndingTime = model.DeliveryEndingTime;
                            restaurant.TimetakentoDeliver = model.TimetakentoDeliver.ToString();
                            restaurant.MinDelivery = model.MinDelivery;
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
                            //TempData["OwnerEmail"] = restaurant.OwnerEmail;
                            //Session["UserEmail"] = restaurant.OwnerEmail;
                            //Session["UserType"] = "RestaurantOwner";
                            //return RedirectToAction("EditRestaurant", "Restaurant", new
                            //{
                            //    id = (from p in db.Restaurants
                            //          where p.OwnerEmail == restaurant.OwnerEmail
                            //          where p.RestaurantName == restaurant.RestaurantName
                            //          select p.Id).Single()
                            //});
                            Session["UserEmail"] = null;
                            Session["UserType"] = null;
                            return RedirectToAction("MoveToHome");
                        }
                        else
                        {
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
            ViewBag.Categories = getCategories();
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

        [NonAction]
        public List<CategoryViewModel> getSelectedCategories(int id)
        {

            List<CategoryViewModel> allCategories = getCategories();

            var categorieID = (from p in db.Restaurant_Type
                               where p.Rest_id == id
                               select p).ToList();

            foreach (CategoryViewModel cat in allCategories)
            {
                foreach (Restaurant_Type res in categorieID)
                {
                    if (res.Category_id.ToString() == cat.id)
                    {
                        cat.isChecked = true;
                    }
                }
            }

            return allCategories;
        }

        public PartialViewResult RestaurantProfile(string partialViewType)
        {
            RestaurantDeatilModel restaurantDetailModel = (RestaurantDeatilModel)Session["RestaurantDetailModel"];
            if (partialViewType == "Profile")
            {

                ViewBag.Categories = getSelectedCategories(restaurantDetailModel.id);
                return PartialView("_RestaurantProfileDetails", restaurantDetailModel);
            }
            else if (partialViewType == "Edit")
            {

                ViewBag.Categories = getSelectedCategories(restaurantDetailModel.id);
                return PartialView("_EditRestaurantProfile", restaurantDetailModel);
            }
            else if (partialViewType == "OrdersList")
            {
                Session["OrdersList"] = getOrders().list;
                return PartialView("_OrdersList");
            }
            else
            {
                return PartialView("_ChangePassword", restaurantDetailModel);
            }

        }
        public PartialViewResult OrderDetails(int  orderID)
        {

            OrderDetailsModel order = getOrder(orderID);

            return PartialView("_OrderDetails",order);
            
        }

        public class UserMailer : MailerBase
        {
            public UserMailer()
            {
                //MasterName="_Layout";
            }

            public virtual MvcMailMessage Dispatch(Models.OrderDispatchMailModel _objModelMail)
            {
                //var resources = new Dictionary<string, string>();
                //resources["logo"] = "~/Img/logo.png";
                //LinkedResource logo = new LinkedResource(resources["logo"]);

                //PopulateBody(mailMessage, "WelcomeMessage", resources);
                ViewData.Model = _objModelMail;
                return Populate(x =>
                {
                    x.Subject = _objModelMail.Subject;
                    x.ViewName = "OrderDispatchEmail";
                    x.To.Add(_objModelMail.To);
                    //x.LinkedResources.Add("logo", resources["logo"]);
                });
            }


        }
        UserMailer userMailer = new UserMailer();
        public ActionResult SendDispatchedMail()
        {
            TheFoody.Models.OrderDispatchMailModel _objModelMail = new TheFoody.Models.OrderDispatchMailModel();
            //change email to restaurant owner's email
            
            _objModelMail.Subject = "Thank You For Your Order!";
            _objModelMail.Order = (OrderDetailsModel)Session["Order"];
            _objModelMail.To = _objModelMail.Order.Cus_email;
            if (_objModelMail.Order.Order_type == "Delivery")
            {
                _objModelMail.Order.Deliver_Man = Session["Order"].ToString();
            }
            
            _objModelMail.Body = "Dear " + _objModelMail.Order.CustomerFirstName + " , Thank you for using TheFoody for odering with " + _objModelMail.Order.RestaurantName + ". Enjoy Your Meal!";
            userMailer.Dispatch(_objModelMail).Send();

            Order order = (from p in db.Orders
                          where p.Order_id == _objModelMail.Order.Order_id
                         select p).FirstOrDefault();
            order.Order_status = "Dispatched";
            db.SaveChanges();

            return RedirectToAction("OrderEmailSent");


        }

        public ActionResult OrderEmailSent()
        {
            return View("OrderEmailSent");
        }
        [NonAction]
        public OrderDetailsModel getOrder(int id)
        {
            string email = Session["UserEmail"].ToString();
            var restaurant = (from p in db.Restaurants
                              where p.OwnerEmail == email
                              select p).FirstOrDefault();

            OrderDetailsModel orderDetails = new OrderDetailsModel();
            
            if (restaurant != null)
            {
                var order = (from p in db.Orders
                            where p.Order_id == id
                            select p).FirstOrDefault();

                
                    
                    orderDetails.Order_id = order.Order_id;
                    orderDetails.Rest_id = Convert.ToInt32(order.Rest_id);
                    orderDetails.RestaurantName = restaurant.RestaurantName;
                    orderDetails.Cus_email = order.Cus_email;
                    orderDetails.Total_price = Convert.ToDecimal(order.Total_price);
                    orderDetails.Order_type = order.Order_type;
                    orderDetails.Order_status = order.Order_status;
                    orderDetails.Payment_status = order.Payment_status;
                    orderDetails.Order_date = Convert.ToDateTime(order.Order_date);
                    orderDetails.Delivery_address = order.Delivery_address;
                    orderDetails.Phone = order.Phone;
                    orderDetails.Landmarks = order.Landmarks;
                    orderDetails.Dispatched_date = order.Dispatched_date;
                    orderDetails.Deliver_Man = order.Deliver_Man;
                
                    orderDetails.Delivery_time = (TimeSpan)order.Delivery_time;
                   
                   
                    orderDetails.CustomerFirstName = (from p in db.Users
                                                      where p.email == order.Cus_email
                                                      select p.fname).FirstOrDefault();

                    var Menus = (from p in db.Order_foods
                                                      where p.Order_id == order.Order_id
                                                      select p).ToList();

                    orderDetails.Menus = new List<OrderedMenusModel>();
                    foreach (Order_foods menu in Menus)
                    {
                        OrderedMenusModel menuDetails = new OrderedMenusModel();
                        menuDetails.Order_food_id = menu.Order_food_id;
                        menuDetails.Menu_id = menu.Menu_id;
                        menuDetails.Quantity = menu.Quantity;
                        menuDetails.Price = menu.Price;

                        menuDetails.Menu = (from p in db.Menus
                                            where p.Menu_id == menu.Menu_id
                                            select p.Menu_name).FirstOrDefault();

                        orderDetails.Menus.Add(menuDetails);

                    }

                   
                
            }

            return orderDetails;
        } 
        [NonAction]
        public OrderViewModel getOrders()
        {
            string email =  Session["UserEmail"].ToString();
            var restaurant = (from p in db.Restaurants
                               where p.OwnerEmail == email
                               select p).FirstOrDefault();

            OrderViewModel orders = new OrderViewModel();
            orders.list = new List<OrderDetailsModel>();
            if (restaurant != null)
            {
                var list = (from p in db.Orders
                               where p.Rest_id == restaurant.Id
                               where p.Order_status == "Pending"
                               orderby p.Delivery_time,p.Order_date
                               select p).ToList();

                foreach (Order order in list)
                {
                    OrderDetailsModel orderDetails = new OrderDetailsModel();
                    orderDetails.Order_id = order.Order_id;
                    orderDetails.Cus_email = order.Cus_email;
                    orderDetails.Total_price = Convert.ToDecimal(order.Total_price);
                    orderDetails.Order_type = order.Order_type;
                    orderDetails.Payment_status = order.Payment_status;

                    orderDetails.CustomerFirstName = (from p in db.Users
                                        where p.email == order.Cus_email
                                        select p.fname).FirstOrDefault();

                    orders.list.Add(orderDetails);
                }
            }

            return orders;
        } 
        // GET: Restaurant/Edit/5
        public ActionResult EditRestaurant()
        {
            string email = Session["UserEmail"].ToString();
            Restaurant restaurant = db.Restaurants.Where(u => u.OwnerEmail == email).SingleOrDefault();
            if (restaurant == null)
            {
                return HttpNotFound();
            }

            RestaurantDeatilModel restaurantDeatilModel = new RestaurantDeatilModel();
            Session["RestaurantDetailModel"] = restaurantDeatilModel;
            restaurantDeatilModel.id = restaurant.Id;
            restaurantDeatilModel.RestaurantName = restaurant.RestaurantName;

            restaurantDeatilModel.Phone = restaurant.Phone.ToString();
            restaurantDeatilModel.Address = restaurant.Address;
            restaurantDeatilModel.Logo = restaurant.Logo;
            restaurantDeatilModel.City = restaurant.City;
            restaurantDeatilModel.PostCode = restaurant.PostCode;
            restaurantDeatilModel.District = restaurant.District;
            restaurantDeatilModel.Website = restaurant.Website;
            restaurantDeatilModel.CompanyBackground = restaurant.CompanyBackground;
            restaurantDeatilModel.OpeningTime = restaurant.OpeningTime;
            restaurantDeatilModel.ClosingTime = restaurant.ClosingTime;
            restaurantDeatilModel.DeliveryStartingTime = restaurant.DeliveryStartingTime;
            restaurantDeatilModel.DeliveryEndingTime = restaurant.DeliveryEndingTime;

            restaurantDeatilModel.OpeningTime = restaurant.OpeningTime;
            DateTime time = DateTime.Today.Add(restaurantDeatilModel.OpeningTime);
            restaurantDeatilModel.detailsOpeningTime = time.ToString("hh:mm tt");

            restaurantDeatilModel.ClosingTime = restaurant.ClosingTime;
            time = DateTime.Today.Add(restaurantDeatilModel.ClosingTime);
            restaurantDeatilModel.detailsClosingTime = time.ToString("hh:mm tt");

            restaurantDeatilModel.DeliveryStartingTime = restaurant.DeliveryStartingTime;
            time = DateTime.Today.Add(restaurantDeatilModel.DeliveryStartingTime);
            restaurantDeatilModel.detailsDeliveryStartingTime = time.ToString("hh:mm tt");

            restaurantDeatilModel.DeliveryEndingTime = restaurant.DeliveryEndingTime;
            time = DateTime.Today.Add(restaurantDeatilModel.DeliveryEndingTime);
            restaurantDeatilModel.detailsDeliveryEndingTime = time.ToString("hh:mm tt");

            restaurantDeatilModel.TimetakentoDeliver = Convert.ToInt32(restaurant.TimetakentoDeliver);
            restaurantDeatilModel.MinDelivery = Convert.ToDecimal(restaurant.MinDelivery);

            TempData["RestaurantId"] = restaurantDeatilModel.id;
            TempData["RestaurantLogo"] = restaurantDeatilModel.Logo;
            ViewBag.Categories = getSelectedCategories(restaurantDeatilModel.id);
            return View(restaurantDeatilModel);
        }

        // POST: Restaurant/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditRestaurant(RestaurantDeatilModel model, FormCollection collection)
        {
            TempData.Keep();
            if (ModelState.IsValid)
            {
                Restaurant restaurant = new Restaurant();
                
                restaurant.OwnerEmail = Session["UserEmail"].ToString();
                
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
                                model.Logo = restaurant.Logo;
                                var path = Path.Combine(Server.MapPath("~/Uploads/RestaurantLogo"), restaurant.Logo);
                                photo.SaveAs(path);
                            }
                            else {
                                restaurant.Logo = TempData["RestaurantLogo"].ToString();
                                model.Logo = restaurant.Logo;
                            }
                            restaurant.Id = Convert.ToInt16(TempData["RestaurantId"]);
                            model.id = restaurant.Id;
                            restaurant.Phone = model.Phone.ToString();
                            restaurant.Address = model.Address;
                            restaurant.City = model.City;
                            restaurant.PostCode = model.PostCode.ToString();
                            restaurant.District = model.District;
                            restaurant.Website = model.Website;
                            restaurant.CompanyBackground = model.CompanyBackground;
                            restaurant.OpeningTime = model.OpeningTime;
                            restaurant.ClosingTime = model.ClosingTime;
                            restaurant.DeliveryStartingTime = model.DeliveryStartingTime;
                            restaurant.DeliveryEndingTime = model.DeliveryEndingTime;


                            DateTime time = DateTime.Today.Add(restaurant.OpeningTime);
                            model.detailsOpeningTime = time.ToString("hh:mm tt");


                            time = DateTime.Today.Add(restaurant.ClosingTime);
                            model.detailsClosingTime = time.ToString("hh:mm tt");


                            time = DateTime.Today.Add(restaurant.DeliveryStartingTime);
                            model.detailsDeliveryStartingTime = time.ToString("hh:mm tt");


                            time = DateTime.Today.Add(restaurant.DeliveryEndingTime);
                            model.detailsDeliveryEndingTime = time.ToString("hh:mm tt");

                            restaurant.TimetakentoDeliver = model.TimetakentoDeliver.ToString();
                            restaurant.MinDelivery = model.MinDelivery;

                            db.Entry(restaurant).State = EntityState.Modified;
                            db.SaveChanges();

                            var categories = db.Categories.ToList();
                            string type = collection["job_type_checkbox"];
                            string[] tt = type.Split(',');

                            restaurant.Restaurant_Type = new List<Restaurant_Type>();

                            List<string> ca = new List<string>();

                            foreach (var category in categories)
                            {

                                for (int i = 0; i < tt.Length; i++)
                                {
                                    if (category.category1 == tt[i])
                                    {
                                        ca.Add(tt[i]);
                                        var categorieID = (from p in db.Restaurant_Type
                                                           where p.Rest_id == restaurant.Id
                                                           where p.Category_id == category.id
                                                           select p).FirstOrDefault();
                                        if (categorieID == null)
                                        {
                                            restaurant.Restaurant_Type.Add(new Restaurant_Type { Category_id = category.id, Rest_id = restaurant.Id });
                                        }

                                    }

                                }

                            }

                            foreach (var category in categories)
                            {

                                if (!ca.Contains(category.category1))
                                {
                                    var categorieID = (from p in db.Restaurant_Type
                                                       where p.Rest_id == restaurant.Id
                                                       where p.Category_id == category.id
                                                       select p).FirstOrDefault();
                                    if (categorieID != null)
                                    {
                                        db.Restaurant_Type.Remove(categorieID);
                                    }

                                }

                            }
                            db.SaveChanges();

                            Session["UserEmail"] = restaurant.OwnerEmail;
                            TempData["RestaurantLogo"] = model.Logo;
                            Session["RestaurantDetailModel"] = model;
                            ViewBag.Categories = getSelectedCategories(Convert.ToInt32(restaurant.Id));
                            return View(model);
                        }
                        else {
                            ModelState.AddModelError("Categories", "Please select atleast one restaurant type");
                        }
                    }
                    else
                    {
                        //TODO E.g. ModelState.AddModelError
                        ModelState.AddModelError("", "Owner has not been logged in successfully");

                    }
                }

            }

            TempData["RestaurantLogo"] = model.Logo;
            ViewBag.Categories = getSelectedCategories(Convert.ToInt32(TempData["RestaurantId"]));
            return View(model);
        }

    }

}