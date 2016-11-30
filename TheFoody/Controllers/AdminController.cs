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
            if (partialViewType == "NewRestaurants")
            {
                var NewRestaurants = getNewRestaurants();
                ViewBag.NewRestaurants = getNewRestaurants();

                return PartialView("_NewRestaurantDetails", NewRestaurants);
            }
            else if (partialViewType == "View")
            {

                var restaurant = (from p in db.Restaurants
                                  where p.Id == restaurantId
                                  select p).SingleOrDefault();
                RestaurantDeatilModel restaurantDeatilModel = new RestaurantDeatilModel();

                Session["RestaurantDetailModel"] = restaurantDeatilModel;
                restaurantDeatilModel.id = restaurant.Id;
                restaurantDeatilModel.RestaurantName = restaurant.RestaurantName;

                restaurantDeatilModel.Phone = restaurant.Phone.ToString();
                restaurantDeatilModel.Address = restaurant.Address;
                restaurantDeatilModel.Logo = restaurant.Logo;
                restaurantDeatilModel.City = restaurant.City;
                restaurantDeatilModel.PostCode = restaurant.PostCode;
                restaurantDeatilModel.District = restaurant.District;
                restaurantDeatilModel.Website = restaurant.Website;
                restaurantDeatilModel.CompanyBackground = restaurant.CompanyBackground;
                restaurantDeatilModel.OpeningTime = restaurant.OpeningTime;
                restaurantDeatilModel.ClosingTime = restaurant.ClosingTime;
                restaurantDeatilModel.DeliveryStartingTime = restaurant.DeliveryStartingTime;
                restaurantDeatilModel.DeliveryEndingTime = restaurant.DeliveryEndingTime;

                return PartialView("_RestaurantDetails", restaurantDeatilModel);
            }
            else
            {
                ManageViewModel manageViewModel = (ManageViewModel)Session["AdminProfile"];
                return PartialView("_Manage", manageViewModel);
            }
 
        }

        [NonAction]
        public List<RestaurantDeatilModel> getNewRestaurants()
        {

            var dbNewRestaurantOwners = db.Users.Where(u => u.status == "Deactive").ToList();

            var newRestaurants = new List<RestaurantDeatilModel>();

            foreach (var category in dbNewRestaurantOwners)
            {
                var restaurant = (from p in db.Restaurants
                                  where p.OwnerEmail == category.email
                                  select p).SingleOrDefault();
                if (restaurant != null)
                {
                    RestaurantDeatilModel restaurantDeatilModel = new RestaurantDeatilModel();
                    Session["RestaurantDetailModel"] = restaurantDeatilModel;
                    restaurantDeatilModel.id = restaurant.Id;
                    restaurantDeatilModel.RestaurantName = restaurant.RestaurantName;

                    restaurantDeatilModel.Phone = restaurant.Phone.ToString();
                    restaurantDeatilModel.Address = restaurant.Address;
                    restaurantDeatilModel.Logo = restaurant.Logo;
                    restaurantDeatilModel.City = restaurant.City;
                    restaurantDeatilModel.PostCode = restaurant.PostCode;
                    restaurantDeatilModel.District = restaurant.District;
                    restaurantDeatilModel.Website = restaurant.Website;
                    restaurantDeatilModel.CompanyBackground = restaurant.CompanyBackground;
                    restaurantDeatilModel.OpeningTime = restaurant.OpeningTime;
                    restaurantDeatilModel.ClosingTime = restaurant.ClosingTime;
                    restaurantDeatilModel.DeliveryStartingTime = restaurant.DeliveryStartingTime;
                    restaurantDeatilModel.DeliveryEndingTime = restaurant.DeliveryEndingTime;
                    newRestaurants.Add(restaurantDeatilModel);
                }
            }

            return newRestaurants;
        }
    }
}
