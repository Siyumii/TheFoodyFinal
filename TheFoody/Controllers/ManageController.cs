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
                        image = "http://localhost:1672/Content/Images/Img/" + myfile;
                        path= Path.Combine(Server.MapPath("~/Content/Images/Img"), myfile);
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
                    user_to_update.postcode = Convert.ToDecimal(manageviewmodel.PostCode);
                    user_to_update.district = manageviewmodel.District;
                    user_to_update.user_type = manageviewmodel.UserType;
                    user_to_update.status = manageviewmodel.Status;
                    user_to_update.photo = image;

                    //db.Users.AddOrUpdate(user_to_update);
                    db.SaveChanges();

                    Session["Path"] = image;
                    Session["UserEmail"] = UserEmail;
                    Session["FirstName"] = manageviewmodel.FirstName;
                    Session["LastName"] = manageviewmodel.LastName;
                    Session["Address"] = manageviewmodel.Address;
                    Session["City"] = manageviewmodel.City;
                    Session["PostCode"] = manageviewmodel.PostCode;
                    Session["District"] = manageviewmodel.District;
                    Session["UserType"] = manageviewmodel.UserType;
                    Session["Status"] = manageviewmodel.Status;
                    Session["Phone"] = manageviewmodel.Phone;
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