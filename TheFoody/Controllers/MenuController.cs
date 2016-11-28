using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheFoody.DataAccess;
using System.IO;
using TheFoody.Models;
using System.Net;

namespace TheFoody.Controllers
{
    public class MenuController : Controller
    {
        TheFoodyContext db = new TheFoodyContext();
        
        // GET: Menus
        public ActionResult Index()
        {
            //using (TheFoodyContext db = new TheFoodyContext())
            //{
                //var model = (from p in db.Menus
                //             select new MenuViewModel()
                //             {
                //                 Menu_name = p.Menu_name,
                //                 Description=p.Description,
                //                 Price=p.Price,
                //                 Photo=p.Photo,
                //                 //MealCategories=p.Meal_Category.

                //             }); 


            //}
            
            return View(db.Menus.ToList());
        }

        
        //public ActionResult Create(MenuViewModel model)
        //{
        //    //List<MealCategoryViewModel> mealcatList = new List<MealCategoryViewModel>();
        //    ////List<SelectListItem> items = new List<SelectListItem>();
        //    ////items = db.Meal_Category.ToList();
        //    //List<Meal_Category> mealcat = db.Meal_Category.ToList();
        //    //mealcatList.Add(TransformToMealCat(mealcat.));
        //    //var content = from p in db.Meal_Category
        //    //              select new { p.Meal_Cat_Id, p.CategoryName };
            
        //    //return View(model);
        //}

        //GET Menu Create View
        public ActionResult Create()

        {
            var x = db.Meal_Category.ToList().Select(c => new SelectListItem
            {
                Text = c.CategoryName,
                Value = c.Meal_Cat_Id.ToString(),
                Selected = (c.Meal_Cat_Id == 1)
            }).ToList();

            ViewBag.mealcategories = x;
            return View();
        }
        //private MealCategoryViewModel TransformToMealCat(Meal_Category mealcategory)
        //{
        //    if (mealcategory == null)
        //        return null;
        //    MealCategoryViewModel mealcatvm = new MealCategoryViewModel();
        //    mealcatvm.MealCategoryID = mealcategory.Meal_Cat_Id;
        //    mealcatvm.MealCategoryName = mealcategory.CategoryName;

        //    return mealcatvm;
        //}


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MenuViewModel model, FormCollection collection)
        {
            TempData.Keep();

            if (ModelState.IsValid)
            {
                Menu menu = new Menu();
                HttpPostedFileBase photo = Request.Files["photo"];

                string email = Session["UserEmail"].ToString();

                int id = (from p in db.Restaurants
                          where p.OwnerEmail == email
                          select p.Id).Single();

                if (photo != null && photo.ContentLength > 0)
                {
                    var extension = Path.GetExtension(photo.FileName);
                    menu.Photo = id + "_" + menu.Menu_name + extension;
                    var path = Path.Combine(Server.MapPath("~/icons/images/menuimages"), menu.Photo);
                    photo.SaveAs(path);
                }

                

                menu.Menu_name = model.Menu_name;
                menu.Description = model.Description;
                menu.Price = model.Price;
                menu.Meal_Cat_IdFK = model.MealCategory;
                menu.RestaurantId = id;
                    //collection["mealcategories"];
                //string email = Session["UserEmail"].ToString();
                //var content = from p in db.Restaurants
                //              where p.OwnerEmail == email
                //              select new { p.Id };
                //int x = Convert.ToInt32(content);
                //menu.RestaurantId = x;

                db.Menus.Add(menu);
                db.SaveChanges();




                return RedirectToAction("Index");

            }
            return View();
        }

        public ActionResult EditMenu (int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Menu menu = db.Menus.Find(id);
            if (menu == null)
            {
                return HttpNotFound();
            }

            MenuDetailModel Menud = new MenuDetailModel();

            Menud.Menu_name = menu.Menu_name;
            Menud.Description = menu.Description;
            Menud.Price = menu.Price;
            return View(Menud);
        }
    }
}