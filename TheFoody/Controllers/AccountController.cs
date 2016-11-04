using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheFoody.Models;
using TheFoody.DataAccess;
using System.Web.Security;
using System.Web.Mail;
using System.Net.Mail;
using WebMatrix.WebData;
using DotNetOpenAuth.AspNet.Clients;

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
                        usr.phone = "0111234567";
                        usr.photo = "Not Set Yet";
                        usr.postcode = 00000;
                        usr.address = "Not Set Yet";
                        usr.city = "Not Set Yet";
                        usr.district = "Not Set Yet";
                        usr.status = "Active";
                        usr.user_type = "Admin";
                        usr.created_date = DateTime.Now;

                        db.Users.Add(usr);
                        db.SaveChanges();
                        

                        Session["UserEmail"] = model.Email;
                        Session["FirstName"] = model.FirstName;
                        Session["LastName"] = model.LastName;
                            Session["Phone"] = "0111234567";
                            Session["Address"] = "Not Set Yet";
                            Session["City"] = "Not Set Yet";
                            Session["PostCode"] = "00000";
                            Session["District"] = "Not Set Yet";
                            Session["UserType"] = "Admin";
                            Session["Status"] = "Active";
                            Session["Photo"] = "Not Set Yet";

                        return RedirectToAction("Index", "Home");
                        }
                    
                }
                
                return View(model);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //private Uri RedirectUri
        //{
        //    get
        //    {
        //        var uriBuilder = new UriBuilder(Request.Url);
        //        uriBuilder.Query = null;
        //        uriBuilder.Fragment = null;
        //        uriBuilder.Path = Url.Action("FacebookCallback");
        //        return uriBuilder.Uri;
        //    }
        //}

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
                    FormsAuthentication.SetAuthCookie(model.Email,model.RememberMe);
                    
                    Session["UserEmail"] = usr.email.ToString();
                    Session["FirstName"] = usr.fname.ToString();
                    Session["LastName"] = usr.lname.ToString();
                    Session["Address"] = usr.address.ToString();
                        Session["City"] = usr.city.ToString();
                        Session["PostCode"] = usr.postcode.ToString();
                        Session["District"] = usr.district.ToString();
                        Session["UserType"] = usr.user_type.ToString();
                        Session["Status"] = usr.status.ToString();
                        Session["Photo"] = usr.photo.ToString();
                        Session["Phone"] = usr.phone.ToString();

                    if(model.RememberMe)
                    {
                        HttpCookie cookie = new HttpCookie("Login");
                        cookie.Values.Add("UserEmail",usr.email);
                        //cookie.Values.Add("Password", usr.password);
                        cookie.Expires = DateTime.Now.AddDays(15);
                        Response.Cookies.Add(cookie);
                    }
                    return RedirectToLocal(returnUrl);
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

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public ActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordModel resetpasswordmodel)
        {
            if (ModelState.IsValid)
            {
                //User user;
                MembershipUser member;
                using (TheFoodyContext db = new TheFoodyContext())
                {
                    var foundemail = (from e in db.Users
                                         where e.email == resetpasswordmodel.Email
                                         select e.email).FirstOrDefault();

                    if (foundemail != null)
                    {
                        
                        member = Membership.GetUser(foundemail.ToString());    
                    }
                    else
                    {
                        member = null;
                    }
                }

                if (member != null)
                {
                    //Generate password token that will be used in the email link to authenticate user
                    var token = WebSecurity.GeneratePasswordResetToken(member.Email);
                    // Generate the html link sent via email
                    string resetLink = "<a href='"
                       + Url.Action("ResetPasswordView", "Account", new { rt = token }, "http")
                       + "'>Reset Password Link</a>";
                    // Email stuff
                    string subject = "Reset your password for TheFoody.com";
                    string body = "You link: " + resetLink;
                    string from = "punyabhagyani863@gmail.com";
                    string to = resetpasswordmodel.Email;

                    System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage(from, to);
                    message.Subject = subject;
                    message.Body = body;
                    SmtpClient client = new SmtpClient();

                    // Attempt to send the email
                    try
                    {
                        client.Send(message);
                    }
                    catch (Exception e)
                    {
                        ModelState.AddModelError("", "Issue sending email: " + e.Message);
                    }
                }
                else // Email not found
                {
                    ModelState.AddModelError("", "No user found by that email.");
                }
            }
            return View(resetpasswordmodel);
        }

        [AllowAnonymous]
        public ActionResult ResetPasswordView(string rt)
        {
            ResetPasswordViewModel model = new ResetPasswordViewModel();
            model.ReturnToken = rt;
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPasswordView(ResetPasswordViewModel resetpasswordviewmodel)
        {
            if (ModelState.IsValid)
            {
                bool resetResponse = WebSecurity.ResetPassword(resetpasswordviewmodel.ReturnToken, resetpasswordviewmodel.Password);
                if (resetResponse)
                {
                    ViewBag.Message = "Successfully Changed";
                }
                else
                {
                    ViewBag.Message = "Something went horribly wrong!";
                }
            }

            return View(resetpasswordviewmodel);
        }

        

        //[AllowAnonymous]
        //public ActionResult Facebook()
        //{
        //    var fb = new FacebookClient();
        //    var loginUrl = fb.GetLoginUrl(new
        //    {
        //        client_id = "CLIENT ID",
        //        client_secret = "CLIENT SECRET",
        //        redirect_uri = RedirectUri.AbsoluteUri,
        //        response_type = "code",
        //        scope = "email"
        //    });

        //    return Redirect(loginUrl.AbsoluteUri);
        //}

        //public ActionResult FacebookCallback(string code)
        //{
        //    var fb = new FacebookClient();
        //    dynamic result = fb.Post("oauth/access_token", new
        //    {
        //        client_id = "CLIENT ID",
        //        client_secret = "SECRET",
        //        redirect_uri = RedirectUri.AbsoluteUri,
        //        code = code
        //    });

        //    var accessToken = result.access_token;

        //    // Store the access token in the session for farther use
        //    Session["AccessToken"] = accessToken;

        //    // update the facebook client with the access token so
        //    // we can make requests on behalf of the user
        //    fb.AccessToken = accessToken;

        //    // Get the user's information, like email, first name, middle name etc
        //    dynamic me = fb.Get("me?fields=first_name,middle_name,last_name,id,email");
        //    string email = me.email;
        //    string firstname = me.first_name;
        //    string middlename = me.middle_name;
        //    string lastname = me.last_name;

        //    // Set the auth cookie
        //    FormsAuthentication.SetAuthCookie(email, false);
        //    return RedirectToAction("Index", "Home");
        //}
    }
}