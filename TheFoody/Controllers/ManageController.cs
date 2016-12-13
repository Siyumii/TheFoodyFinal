using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheFoody.DataAccess;
using TheFoody.Models;
using System.Data.Entity.Migrations;

namespace TheFoody.Controllers
{
    public class ManageController : Controller
    {
        //ManageContext db = new ManageContext();
        //
         //GET: /Manage/
        public ActionResult Manage()
        {
            
            TheFoodyContext db = new TheFoodyContext();
            string UserEmail = Session["UserEmail"].ToString();
            
            User usr = db.Users.Where(u => u.email == UserEmail).SingleOrDefault();

            //var usr= (from u in db.Users // .Includes("Addresses") here?
            //          where u.email==UserEmail
            //          select new ManageViewModel()
            //          {
            //              FirstName = u.fname,
            //              LastName=u.lname,
            //              Email=u.email,
            //              Phone=u.phone,
            //              photo=u.photo,
            //              Address = u.address,
            //              City = u.city,
            //              PostCode=u.postcode,
            //              District = u.district
            //          });
            ManageViewModel model = new ManageViewModel();
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

        private bool isValidContentType(string contentType)
        {
            return contentType.Equals("Images/png") || contentType.Equals("Images/gif") || contentType.Equals("Images/jpg") || contentType.Equals("Images/jpeg");
        }

        private bool isValidContentLength(int contentLength)
        {
            return (contentLength / 1024) / 1024 < 1; //1MB
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage(ManageViewModel manageviewmodel, FormCollection collection)
        {
            TheFoodyContext db = new TheFoodyContext();
            //User user = new User();
            string UserEmail = Session["UserEmail"].ToString();
            User user_to_update = db.Users.Find(UserEmail);

            if (ModelState.IsValid)
            {

                try
                {

                //if (!isValidContentType(Photo.ContentType))
                //{
                //    ViewBag.Error = "Only JPG , JPEG , GIF & PNG are allowed!";
                //    return View("Manage");
                //}

                //else if (!isValidContentLength(Photo.ContentLength))
                //{
                //    ViewBag.Error = "Your File is too Large!";
                //    return View("Manage");
                //}

                //else
                //{
                //if(user_to_update != null)
                //{
                    HttpPostedFileBase photo = Request.Files["photo"];

                if (photo != null && photo.ContentLength > 0)
                {
                    var path = "";
                    var image = "";
                    var fileName = Path.GetFileName(photo.FileName);
                    var extension = Path.GetExtension(photo.FileName);
                    var allowedExtensions = new[] {".Jpg", ".png", ".jpg", "jpeg"};
                    if (allowedExtensions.Contains(extension))
                    {
                        string name = Path.GetFileNameWithoutExtension(fileName);
                        string myfile = name + "_" + UserEmail + extension;
                        //image = "http://localhost:1672/Content/Images/Img/" + myfile;
                        path= Path.Combine(Server.MapPath("~/images/user-images"), myfile);
                        photo.SaveAs(path);
                        user_to_update.photo = myfile;

                    }
                    else
                    {
                        ViewBag.message = "Please choose only Image file";
                    }


                    user_to_update.email = UserEmail;
                    user_to_update.fname = manageviewmodel.FirstName;
                    user_to_update.lname = manageviewmodel.LastName;
                    user_to_update.phone = manageviewmodel.Phone;
                    user_to_update.address = manageviewmodel.Address;
                    user_to_update.city = manageviewmodel.City;
                    user_to_update.postcode = manageviewmodel.PostCode;
                    user_to_update.district = manageviewmodel.District;
                    //user_to_update.user_type = manageviewmodel.UserType;
                    //user_to_update.status = manageviewmodel.Status;
                    //user_to_update.photo = image;

                    //db.Users.AddOrUpdate(user_to_update);
                    db.SaveChanges();

                    //Session["Path"] = image;
                    //Session["UserEmail"] = UserEmail;
                    //Session["FirstName"] = manageviewmodel.FirstName;
                    //Session["LastName"] = manageviewmodel.LastName;
                    //Session["Address"] = manageviewmodel.Address;
                    //Session["City"] = manageviewmodel.City;
                    //Session["PostCode"] = manageviewmodel.PostCode;
                    //Session["District"] = manageviewmodel.District;
                    //Session["UserType"] = manageviewmodel.UserType;
                    //Session["Status"] = manageviewmodel.Status;
                    //Session["Phone"] = manageviewmodel.Phone;
                    return RedirectToAction("Manage");
                }
                //}


                }
                catch (Exception ex)
                {
                 return View(ex.Message);
                }
                return View(manageviewmodel);
                }

                return View(manageviewmodel);

            }
        //}

        //public void LoadData()
        //{
        //    using (TheFoodyContext db = new TheFoodyContext())
        //    {
        //        ManageViewModel manageviewmodel = new ManageViewModel();
        //        var query = db.Users.AsNoTracking().SingleOrDefault(s => s.email == manageviewmodel.Email);

        //        if (query != null)
        //        {
        //            manageviewmodel.FirstName = query.fname;
        //        }
        //    }
        //}
	}
}