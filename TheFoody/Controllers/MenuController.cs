using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheFoody.DataAccess;
using System.IO;
using TheFoody.Models;
using System.Net;
using System.Data.Entity;

namespace TheFoody.Controllers
{
    public class MenuController : Controller
    {
        TheFoodyContext db = new TheFoodyContext();
        
        // GET: Menus
        public ActionResult Index()
        {
           
            string email = Session["UserEmail"].ToString();

            int id = (from p in db.Restaurants
                      where p.OwnerEmail == email
                      select p.Id).Single();

            List<Menu> r1 = db.Menus.Where(x => x.RestaurantId == id).ToList();
            return View(r1);
        }

        //Get: Menus current stock equals to minimum stock
        public ActionResult MinimumIndex()
        {

            string email = Session["UserEmail"].ToString();

            int id = (from p in db.Restaurants
                      where p.OwnerEmail == email
                      select p.Id).Single();
            if (id != 0)
            {
                List<Menu> r2 = db.Menus.Where(x => x.RestaurantId == id && x.Minimum_count == x.Current_count).ToList();
                return View(r2);
            }

            else
            {
                return View("There aren't available any menus equals to the minimum count");
            }

                
         
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
                menu.Menu_name = model.Menu_name;

                if (photo != null && photo.ContentLength > 0)
                {
                    var extension = Path.GetExtension(photo.FileName);
                    menu.Photo = id + "_" + menu.Menu_name + extension;
                    var path = Path.Combine(Server.MapPath("~/images/menuimages"), menu.Photo);
                    photo.SaveAs(path);
                }

                

                
                menu.Description = model.Description;
                menu.Price = model.Price;
                menu.Meal_Cat_IdFK = model.MealCategory;
                menu.RestaurantId = id;
                menu.Daily_fixed_count = model.Daily_fixed_count;
                menu.Current_count = model.Current_count;
                menu.Minimum_count = model.Minimum_count;
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


        //GET Menu Create EditView
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

            var x = db.Meal_Category.ToList().Select(c => new SelectListItem
            {
                Text = c.CategoryName,
                Value = c.Meal_Cat_Id.ToString(),
                Selected = (c.Meal_Cat_Id == 1)
            }).ToList();

       

            MenuDetailModel Menud = new MenuDetailModel();
            //Session["MenuDetailModel"] = Menud;
            Menud.Menu_id = menu.Menu_id;
            Menud.Menu_name = menu.Menu_name;
            Menud.Description = menu.Description;
            Menud.Price = menu.Price;
            Menud.Photo = menu.Photo;
            Menud.Daily_fixed_count = Convert.ToInt16(menu.Daily_fixed_count);
            Menud.Current_count = Convert.ToInt16(menu.Current_count);
            Menud.Minimum_count = Convert.ToInt16(menu.Minimum_count);


            TempData["MenuPhoto"] = Menud.Photo;
            Session["Menuid"] = Menud.Menu_id;
            ViewBag.mealcategories = x;
            return View(Menud);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditMenu(MenuDetailModel model, FormCollection collection)
        {
            TempData.Keep();
            if (ModelState.IsValid)
            {
                int menuid = Convert.ToInt32(Session["Menuid"]);
                Menu menu = db.Menus.Find(menuid);

                HttpPostedFileBase photo = Request.Files["photo"];

                string email = Session["UserEmail"].ToString();

                int id = (from p in db.Restaurants
                          where p.OwnerEmail == email
                          select p.Id).Single();

                if (photo != null && photo.ContentLength > 0)
                {
                    var extension = Path.GetExtension(photo.FileName);
                    menu.Photo = id + "_" + model.Menu_name + extension;
                    model.Photo = menu.Photo;
                    var path = Path.Combine(Server.MapPath("~/images/menuimages"), menu.Photo);
                    photo.SaveAs(path);
                }
                else
                {
                    menu.Photo = TempData["menuimages"].ToString();
                    model.Photo = menu.Photo;
                }
                //menu.Menu_id = Convert.ToInt16(TempData["Menuid"]);
                //model.Menu_id = menu.Menu_id;
                //model.Menu_id = mid;
                //menu.Menu_id = model.Menu_id;

                


                menu.Menu_name = model.Menu_name;
                menu.Description = model.Description;
                menu.Price = model.Price;
                //menu.Meal_Cat_IdFK = model.MealCategory;
                menu.RestaurantId = id;
                menu.Daily_fixed_count = model.Daily_fixed_count;
                menu.Current_count = model.Current_count;
                menu.Minimum_count = model.Minimum_count;

                //db.Entry(menu).State = EntityState.Modified;
                db.SaveChanges();

                //var x = db.Meal_Category.ToList().Select(c => new SelectListItem
                //{
                //    Text = c.CategoryName,
                //    Value = c.Meal_Cat_Id.ToString(),
                //    Selected = (c.Meal_Cat_Id == model.MealCategory)
                //}).ToList();
                //ViewBag.mealcategories = x;
            }
           
            TempData["menuimages"] = model.Photo;
            Session["MenuDetailModel"] = model;
            
            //ViewBag.Categories = getSelectedCategories(Convert.ToInt32(TempData["RestaurantId"]));
            return RedirectToAction("Index","Menu");

        }


        //GET Menu DeleteView
        public ActionResult Delete(int id)
        {
            Menu menu = db.Menus.Find(id);
            if (menu == null)
            {
                return HttpNotFound();
            }

            var x = db.Meal_Category.ToList().Select(c => new SelectListItem
            {
                Text = c.CategoryName,
                Value = c.Meal_Cat_Id.ToString(),
                Selected = (c.Meal_Cat_Id == 1)
            }).ToList();

            MenuDeleteModel Menud = new MenuDeleteModel();
            Session["MenuDeleteModel"] = Menud;
            Menud.Menu_id = menu.Menu_id;
            Menud.Menu_name = menu.Menu_name;
            Menud.Description = menu.Description;
            Menud.Price = menu.Price;
            Menud.Photo = menu.Photo;
            
         
            ViewBag.mealcategories = x;
            return View(Menud);
        }

       

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Menu menu = db.Menus.Find(id);
            db.Menus.Remove(menu);
            db.SaveChanges();
            return RedirectToAction("Index");
        }



        public ActionResult UpdateCurrentStock(int id)
        {
            
            Menu menu = db.Menus.Find(id);
            if (menu == null)
            {
                return HttpNotFound();
            }

            MenuMinUpdateModel Menum = new MenuMinUpdateModel();
            Menum.Menu_id = menu.Menu_id;
            Menum.Menu_name = menu.Menu_name;
            Menum.Photo = menu.Photo;
            Menum.Minimum_count = Convert.ToInt16(menu.Minimum_count);
            Menum.Current_count = Convert.ToInt16(menu.Current_count);
            Menum.Daily_fixed_count = Convert.ToInt16(menu.Daily_fixed_count);

            TempData["MenuPhoto"] = Menum.Photo;
            Session["Menuid"] = Menum.Menu_id;
            return View(Menum);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateCurrentStock(MenuMinUpdateModel model,FormCollection collection)
        {
            TempData.Keep();
            if (ModelState.IsValid) {

                int menuid = Convert.ToInt32(Session["Menuid"]);
                Menu menu = db.Menus.Find(menuid);

                int mcount = model.Minimum_count;
                int ucount = model.Updating_count;
                int tot = mcount + ucount;

                menu.Current_count = tot;

            db.SaveChanges();

            }
            Session["MenuMinUpdateModel"] = model;

            return RedirectToAction("UpdateCurrentStock");

        }

    }
}