using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheFoody.DataAccess;
using TheFoody.Models;

namespace TheFoody.Controllers
{
    public class ChangePasswordController : Controller
    {
        //
        // GET: /ChangePassword/
        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordViewModel changepasswordviewmodel)
        {
            TheFoodyContext db = new TheFoodyContext();
            string UserEmail = Session["UserEmail"].ToString();
            User user_to_update = db.Users.SingleOrDefault(s => s.email == UserEmail);

            if (user_to_update != null)
            {
                if ((changepasswordviewmodel.OldPassword == user_to_update.password) && (changepasswordviewmodel.NewPassword == changepasswordviewmodel.ConfirmPassword))
                {
                    user_to_update.password = changepasswordviewmodel.NewPassword;
                    db.SaveChanges();
                    return RedirectToAction("ChangePassword");
                    
                }  
            }
            return RedirectToAction("ChangePassword");
        }
	}
}