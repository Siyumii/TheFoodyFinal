using SRVTextToImage;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheFoody.DataAccess;
using TheFoody.Models;

namespace TheFoody.Controllers
{
    public class DeleteAccountController : Controller
    {
        //
        // GET: /DeleteAccount/
        public ActionResult DeleteAccount()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteAccount(DeleteAccountViewModel deleteaccountviewmodel, string CaptchaText)
        {
            TheFoodyContext db = new TheFoodyContext();
            string UserEmail = Session["UserEmail"].ToString();
            User user_to_update = db.Users.SingleOrDefault(s => s.email == UserEmail);

            if (user_to_update != null)
            {
                if ((deleteaccountviewmodel.Password == user_to_update.password) && (this.Session["CaptchaImageText"].ToString() == CaptchaText))
                {
                    db.Users.Remove(user_to_update);
                    Session["UserEmail"] = null;
                    db.SaveChanges();
                    return RedirectToAction("Index", "Home");
                }
            }
            return RedirectToAction("DeleteAccount");
        }

        public ActionResult FeedbackForm()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FeedbackForm(CustomCaptchaViewModel customcaptchamodel,string CaptchaText)
        {
            if(this.Session["CaptchaImageText"].ToString() == CaptchaText)
            {
                ViewBag.Message = "Captcha Validation Success!";
            }
            else
            {
                ViewBag.Message = "Captcha Validation Failed!";
            }
            return View(customcaptchamodel);
        }

        //This action for get captcha image
        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]//This is for output cache false.
        public FileResult GetCaptchaImage()
        {
            CaptchaRandomImage CI = new CaptchaRandomImage();
            this.Session["CaptchaImageText"] = CI.GetRandomString(5); //Here 5 means i want to get 5 long captcha.
            CI.GenerateImage(this.Session["CaptchaImageText"].ToString(), 300, 75,Color.DarkGray,Color.White);
            //Or we can use another one for get custom color captcha image.
            MemoryStream stream = new MemoryStream();
            CI.Image.Save(stream, ImageFormat.Png);
            stream.Seek(0,SeekOrigin.Begin);
            return new FileStreamResult(stream,"image/png");
        }


	}
}