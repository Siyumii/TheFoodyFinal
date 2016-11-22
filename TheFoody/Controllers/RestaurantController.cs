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


namespace TheFoody.Controllers
{
    public class RestaurantController : Controller
    {
        private TheFoodyContext db = new TheFoodyContext();

        // GET: Restaurant
        public ActionResult Index()
        {
            using (TheFoodyContext db = new TheFoodyContext())
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
                //return View(db.Restaurants.ToList());
            }

        }
        public ActionResult ViewMenu(int id)
        {
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
                            restaurant.PostCode = model.PostCode;
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