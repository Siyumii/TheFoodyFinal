using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheFoody.Models;
using TheFoody.DataAccess;

namespace TheFoody.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {

        //// GET: Account
        //public ActionResult Index()
        //{
        //    return View();
        //}

        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                
                using (TheFoodyContext db = new TheFoodyContext())
                {
                    if (db.Users.Any(u => u.email.Equals(model.Email)))
                    {
                        //TODO E.g. ModelState.AddModelError
                        ModelState.AddModelError("", "Email already exists");
                        
                    }
                    else
                    {
                        User usr = new User();
                        usr.email = model.Email;
                        usr.fname = model.FirstName;
                        usr.lname = model.LastName;
                        usr.password = model.Password;
                        usr.status = "Active";
                        usr.user_type = "Admin";
                        usr.created_date = DateTime.Now;

                        db.Users.Add(usr);
                        db.SaveChanges();

                        Session["UserEmail"] = model.Email;
                        return RedirectToAction("Index", "Home");
                    }
                }
                
                return View(model);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        // GET: /Account/Login
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            using (TheFoodyContext db = new TheFoodyContext())
            {
                //var usr = db.Users.Single(u => u.email == model.Email && u.password == model.Password);
                var usr = db.Users.Where(u => u.email == model.Email && u.password == model.Password).FirstOrDefault();
                if (usr == null)
                {
                    ModelState.AddModelError("", "Invalid Email or password");
                }
                else
                {
                    Session["UserEmail"] = usr.email.ToString();
                    return RedirectToAction(returnUrl);
                }
                
            }

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            Session["UserEmail"] = null;
            return RedirectToAction("Index", "Home");
        }

    }
}