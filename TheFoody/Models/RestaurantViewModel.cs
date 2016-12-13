using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TheFoody.DataAccess;

namespace TheFoody.Models
{
    public class RestaurantViewModel
    {
        public int RestId { get; set; }
        public string RestaurantName { get; set; }
        public string Logo { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public List<string> categories { get; set; }
        public string TimetakentoDeliver { get; set; }
        public decimal MinDelivery { get; set; }

        public List<MealCategoryVm> MealCategories { get; set; }
        public List<MenuVm> MenuList { get; set; }
    }

    public class MealCategoryVm
    {
        public int MealCategoryID { get; set; }
        public string MealCategoryName { get; set; }

        //public List<MenuVm> MenuList { get; set; }
    }

    public class MenuVm
    {
        public int MenuID { get; set; }
        public string MenuName { get; set; }
        public int MealCategoryID { get; set; }
        public string MenuDescription { get; set; }
        public double MenuPrice { get; set; }
        public string MenuPhoto { get; set; }
    }

    public class CartItem
    {
        public int MenuID { get; set; }
        public string MenuName { get; set; }
        public double MenuPrice { get; set; }
        public int Quantity { get; set; }
        public double TotalPrice { get; set; }
    }

    public class RestaurantModel
    {
        [Required]
        [Display(Name = "FirstName")]
        public string fname { get; set; }

        [Required]
        [Display(Name = "LastName")]
        public string lname { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone Number")]
        public int phone { get; set; }

        [Display(Name = "Profile Picture")]
        public string photo { get; set; }

        [Required]
        [Display(Name = "Address")]
        public string address { get; set; }

        [Required]
        [Display(Name = "City")]
        [RegularExpression(@"^([a-zA-Z])+([a-zA-Z\s])*([a-zA-Z])$", ErrorMessage = "No Spaces allowed at beginning and end.Please enter only letters")]
        public string city { get; set; }

        [Required]
        [Display(Name = "PostCode")]
        public string postcode { get; set; }

        [Required]
        [Display(Name = "District")]
        [RegularExpression(@"^([a-zA-Z])+([a-zA-Z\s])*([a-zA-Z])$", ErrorMessage = "No Spaces allowed at beginning and end.Please enter only letters")]
        public string district { get; set; }

    }

    public class RestaurantDeatilModel
    {
        public RestaurantDeatilModel()
        {
        }
        [Required]
        [Display(Name = "Restaurant Name")]
        public string RestaurantName { get; set; }

        [Display(Name = "Logo")]
        public string Logo { get; set; }

        [Required]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Required]
        [Display(Name = "City")]
        [RegularExpression(@"^([a-zA-Z])+([a-zA-Z\s])*([a-zA-Z])$", ErrorMessage = "No Spaces allowed at beginning and end.Please enter only letters")]
        public string City { get; set; }

        [Required]
        [Display(Name = "District")]
        [RegularExpression(@"^([a-zA-Z])+([a-zA-Z\s])*([a-zA-Z])$", ErrorMessage = "No Spaces allowed at beginning and end.Please enter only letters")]
        public string District { get; set; }

        [Required]
        [Display(Name = "PostCode")]
        public string PostCode { get; set; }

        [DataType(DataType.Url)]
        [Display(Name = "Website")]
        public string Website { get; set; }

        [Required]
        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [Display(Name = "Company Background")]
        public string CompanyBackground { get; set; }

        [Required]
        [Display(Name = "Opening Time")]
        [DataType(DataType.Time)]
        public System.TimeSpan OpeningTime { get; set; }

        [Required]
        [Display(Name = "Closing Time")]
        [DataType(DataType.Time)]
        public System.TimeSpan ClosingTime { get; set; }

        [Required]
        [Display(Name = "Delivery Starting Time")]
        [DataType(DataType.Time)]
        public System.TimeSpan DeliveryStartingTime { get; set; }

        [Required]
        [Display(Name = "Delivery Ending Time")]
        [DataType(DataType.Time)]
        public System.TimeSpan DeliveryEndingTime { get; set; }

        [Required]
        [Display(Name = "Time taken to Deliver")]

        public int TimetakentoDeliver { get; set; }

        [Display(Name = "Categories")]
        public List<CategoryViewModel> Categories { get; set; }

        public int id { get; set; }
        public string detailsOpeningTime { get; set; }

        public string detailsClosingTime { get; set; }

        public string detailsDeliveryStartingTime { get; set; }

        public string detailsDeliveryEndingTime { get; set; }
    }
}