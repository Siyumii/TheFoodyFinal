using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Web.Mvc;
using TheFoody.Models;
using TheFoody.DataAccess;
using System.Threading.Tasks;

namespace TheFoody.Controllers
{
    public class RestaurantController : Controller
    {
        // GET: Restaurants
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

            return restaurantVm;
        }

    }
}