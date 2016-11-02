using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheFoody.DataAccess;
using TheFoody.Models;

namespace TheFoody.Controllers
{
    public class ManageController : Controller
    {
        //ManageContext db = new ManageContext();
        //
         //GET: /Manage/
        public ActionResult Manage()
        {
            //LoadData();
            TheFoodyContext db = new TheFoodyContext();
            //string UserEmail = Session["UserEmail"].ToString();
            //User user_to_view = db.Users.SingleOrDefault(s => s.email == UserEmail);
            ////User user_to_view;
            var manageviewmodel = new ManageViewModel();

            ////user_to_view = (from data in db.Users
            ////                   where data.email == UserEmail
            ////                   select data).SingleOrDefault();

            //manageviewmodel.FirstName = user_to_view.fname;
            //manageviewmodel.LastName = user_to_view.lname;
            //manageviewmodel.Email = user_to_view.email;
            //manageviewmodel.Address = user_to_view.address;
            //manageviewmodel.District = user_to_view.district;
            //manageviewmodel.City = user_to_view.city;
            //manageviewmodel.PostCode = (int)user_to_view.postcode;
            //manageviewmodel.UserType = user_to_view.user_type;
            //manageviewmodel.Status = user_to_view.status;
            //manageviewmodel.Phone = user_to_view.phone;
            
            return View(manageviewmodel);
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
        public ActionResult Manage(ManageViewModel manageviewmodel)
        {
            TheFoodyContext db = new TheFoodyContext();
            User user = new User();
            User user_to_update = db.Users.SingleOrDefault(s => s.email == manageviewmodel.Email);

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
                    

                    var allowedExtensions = new[] {  
                                    ".Jpg", ".png", ".jpg", "jpeg"
                                  };

                    user_to_update.email = manageviewmodel.Email;
                    user_to_update.fname = manageviewmodel.FirstName;
                    user_to_update.lname = manageviewmodel.LastName;
                    user_to_update.phone = manageviewmodel.Phone;
                    user_to_update.address = manageviewmodel.Address;
                    user_to_update.city = manageviewmodel.City;
                    user_to_update.postcode = manageviewmodel.PostCode;
                    user_to_update.district = manageviewmodel.District;
                    user_to_update.user_type = manageviewmodel.UserType;
                    user_to_update.status = manageviewmodel.Status;
                    

                    var fileName = Path.GetFileName(photo.FileName);
                    var extension = Path.GetExtension(photo.FileName);

                    //user_to_update.photo = fileName;

                    if (allowedExtensions.Contains(extension))
                    {
                        string name = Path.GetFileNameWithoutExtension(fileName);
                        string myfile = name + "_" + user_to_update.email + extension;
                        var path = Path.Combine(Server.MapPath("~/Img"), myfile);
                        photo.SaveAs(path);
                        user_to_update.photo = myfile;
                        
                    }
                    else
                    {
                        ViewBag.message = "Please choose only Image file";
                    }
                    //    user.photo = user.email + extension;
                    //    var path = Path.Combine(Server.MapPath("~/Content/Images"), user.photo);
                    //    photo.SaveAs(path);
                    //}
                    //db.SaveChanges();
                    db.Users.Add(user_to_update);
                    db.SaveChanges();
                    return RedirectToAction("Manage");
                }
                //}


                }
                catch (Exception ex)
                {
                 return View("Error");
                }
                return View(manageviewmodel);
                }

                return View(manageviewmodel);

            }
        //}

        public void LoadData()
        {
            using (TheFoodyContext db = new TheFoodyContext())
            {
                ManageViewModel manageviewmodel = new ManageViewModel();
                var query = db.Users.AsNoTracking().SingleOrDefault(s => s.email == manageviewmodel.Email);

                if (query != null)
                {
                    manageviewmodel.FirstName = query.fname;
                }
            }
        }
	}
}