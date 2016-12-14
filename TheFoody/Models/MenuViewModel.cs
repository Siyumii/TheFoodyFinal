using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TheFoody.Models
{
    public class MenuViewModel
    {
        public int Menu_id { get; set; }

        [Required]
        [Display(Name = "Meal Category")]
        public int MealCategory { get; set; }

        [Required]
        [Display(Name = "Menu  Name")]
        public string Menu_name { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [RegularExpression(@"^([0-9])+([[0-9])*([0-9])$", ErrorMessage = "PLease enter only numbers")]
        [Display(Name = "Price")]
        public Nullable<decimal> Price { get; set; }

        [Required]
        [Display(Name = "Menu Picture")]
        public string Photo { get; set; }


        [Required]
        [Display(Name = "Daily_stock")]
        public int Daily_fixed_count { get; set; }

        [Required]
        [Display(Name = "Current_stock")]
        public int Current_count { get; set; }

        [Required]
        [Display(Name = "Minimum_stock")]
        public int Minimum_count { get; set; }

        //public List<MealCategoryViewModel> MealCategories { get; set; }
    }

    public class MealCategoryViewModel
    {
        public int MealCategoryID { get; set; }
        public string MealCategoryName { get; set; }
    }

    public class MenuDetailModel
    {
        //public MenuDetailModel()
        //{

        //}

        public int Menu_id { get; set; }
        [Required]
        [Display(Name = "Meal Category")]
        public int MealCategory { get; set; }

        [Required]
        [Display(Name = "Menu  Name")]
        public string Menu_name { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Display(Name = "Price")]
        public Nullable<decimal> Price { get; set; }

        [Required]
        [Display(Name = "Menu Picture")]
        public string Photo { get; set; }


        [Required]
        [Display(Name = "Daily Fixed stock")]
        public int Daily_fixed_count { get; set; }

        [Required]
        [Display(Name = "Current stock")]
        public int Current_count { get; set; }

        [Required]
        [Display(Name = "Minimum stock")]
        public int Minimum_count { get; set; }



    }

    public class MenuDeleteModel
    {
        //public MenuDeleteModel()
        //{

        //}

        public int Menu_id { get; set; }

        [Display(Name = "Meal Category")]
        public int MealCategory { get; set; }

        [Display(Name = "Menu  Name")]
        public string Menu_name { get; set; }
        
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Price")]
        public Nullable<decimal> Price { get; set; }

        [Display(Name = "Menu Picture")]
        public string Photo { get; set; }

        public int RestaurantId { get; set; }

        [Display(Name = "Daily Fixed stock")]
        public int Daily_fixed_count { get; set; }

        [Display(Name = "Current stock")]
        public int Current_count { get; set; }

        [Display(Name = "Minimum stock")]
        public int Minimum_count { get; set; }

    }

    public class MenuMinUpdateModel
    {
        public int Menu_id { get; set; }

        [Display(Name = "Meal Category")]
        public int MealCategory { get; set; }

        [Display(Name = "Menu  Name")]
        public string Menu_name { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Price")]
        public Nullable<decimal> Price { get; set; }

        [Display(Name = "Menu Picture")]
        public string Photo { get; set; }

        public int RestaurantId { get; set; }

        [Display(Name = "Daily Fixed Stock")]
        public int Daily_fixed_count { get; set; }

        [Display(Name = "Current stock")]
        public int Current_count { get; set; }

        [Display(Name = "Minimum stock")]
        public int Minimum_count { get; set; }

        [Display(Name ="Updating Count")]
        public int Updating_count { get; set; }
    }
}