using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TheFoody.DataAccess;

namespace TheFoody.Models
{
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
        public RestaurantDeatilModel() {   
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
    }
}