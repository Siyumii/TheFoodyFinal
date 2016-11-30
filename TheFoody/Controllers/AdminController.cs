using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheFoody.DataAccess;
using TheFoody.Models;

namespace TheFoody.Controllers
{
    public class AdminController : Controller
    {
        TheFoodyContext db = new TheFoodyContext();

        // GET: Admin
        public ActionResult Index()
        {
            
            string UserEmail = Session["UserEmail"].ToString();

            User usr = db.Users.Where(u => u.email == UserEmail).SingleOrDefault();
            ManageViewModel model = new ManageViewModel();
            Session["AdminProfile"] = model;
            model.FirstName = usr.fname;
            model.LastName = usr.lname;
            model.photo = usr.photo;
            model.Phone = usr.phone;
            model.Address = usr.address;
            model.City = usr.city;
            model.PostCode = usr.postcode;
            model.District = usr.district;
            return View(model);
        }


        public PartialViewResult AdminProfile(string partialViewType, int restaurantId)
        {
            ManageViewModel manageViewModel = (ManageViewModel)Session["AdminProfile"];
            if (partialViewType == "Profile")
            {
                
                return PartialView("_Manage", manageViewModel);
            }
            return PartialView("_Manage", manageViewModel);
            
        }
    }
}
