using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
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
         //GET: /Manage/
        public ActionResult Manage()
        {
            TheFoodyContext db = new TheFoodyContext(); 
            var usermanageviewmodel = new UserManageViewModel();
            return View(usermanageviewmodel);
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
        public ActionResult Manage(UserManageViewModel usermanageviewmodel)
        {
            TheFoodyContext db = new TheFoodyContext();
            string UserEmail = Session["UserEmail"].ToString();
            User user_to_update = db.Users.SingleOrDefault(s => s.email == UserEmail);
            if (ModelState.IsValid)
            {
                try
                {
                    HttpPostedFileBase photo = Request.Files["photo"];
                    if (photo != null && photo.ContentLength > 0)
                    {
                        var allowedExtensions = new[]{".Jpg", ".png", ".jpg", "jpeg"};
                        var fileName = Path.GetFileName(photo.FileName);
                        var extension = Path.GetExtension(photo.FileName);
                        var path = "";

                        if (allowedExtensions.Contains(extension))
                        {
                            string name = Path.GetFileNameWithoutExtension(fileName);
                            string myfile = name + "_" + user_to_update.email + extension;
                            path = Path.Combine(Server.MapPath("~/Img"), myfile);
                            photo.SaveAs(path);
                        }
                        else
                        {
                            ViewBag.message = "Please choose only Image file";
                        }

                        user_to_update.fname = usermanageviewmodel.FirstName;
                        user_to_update.lname = usermanageviewmodel.LastName;
                        user_to_update.phone = usermanageviewmodel.Phone;
                        user_to_update.address = usermanageviewmodel.Address;
                        user_to_update.city = usermanageviewmodel.City;
                        user_to_update.postcode = Convert.ToDecimal(usermanageviewmodel.PostCode);
                        user_to_update.district = usermanageviewmodel.District;
                        user_to_update.user_type = usermanageviewmodel.UserType;
                        user_to_update.status = usermanageviewmodel.Status;
                        user_to_update.photo = path;

                        db.Users.Add(user_to_update);
                        db.SaveChanges();

                        Session["UserEmail"] = UserEmail;
                        Session["FirstName"] = usermanageviewmodel.FirstName;
                        Session["LastName"] = usermanageviewmodel.LastName;
                        Session["Address"] = usermanageviewmodel.Address;
                        Session["City"] = usermanageviewmodel.City;
                        Session["PostCode"] = usermanageviewmodel.PostCode;
                        Session["District"] = usermanageviewmodel.District;
                        Session["UserType"] = usermanageviewmodel.UserType;
                        Session["Status"] = usermanageviewmodel.Status;
                        Session["Phone"] = usermanageviewmodel.Phone;
                        return RedirectToAction("Manage");
                    }
                }
                catch (DbEntityValidationException ex)
                {

                    // Retrieve the error messages as a list of strings.
                    var errorMessages = ex.EntityValidationErrors
                            .SelectMany(x => x.ValidationErrors)
                            .Select(x => x.ErrorMessage);

                    // Join the list to a single string.
                    var fullErrorMessage = string.Join("; ", errorMessages);

                    // Combine the original exception message with the new one.
                    var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                    // Throw a new DbEntityValidationException with the improved exception message.
                    throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
                }
                catch (Exception ex)
                {
                    return View(ex.StackTrace);
                }
                return View(usermanageviewmodel);
            }
            return View(usermanageviewmodel);
        }
	}
}