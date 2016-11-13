using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TheFoody.Models
{
    public class MenuViewModel
    {
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
        [Display(Name = "Price")]
        public Nullable<decimal> Price { get; set; }

        [Required]
        [Display(Name = "Menu Picture")]
        public string Photo { get; set; }

        //public List<MealCategoryViewModel> MealCategories { get; set; }
    }

    public class MealCategoryViewModel
    {
        public int MealCategoryID { get; set; }
        public string MealCategoryName { get; set; }
    }

    public class MenuDetailModel
    {
        public MenuDetailModel()
        {

        }
        [Required]
        [Display(Name = "Meal Category")]
        public string MealCategory { get; set; }

        [Required]
        [Display(Name = "Menu  Name")]
        public string Menu_name { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Price")]
        public Nullable<decimal> Price { get; set; }

        [Required]
        [Display(Name = "Menu Picture")]
        public string Photo { get; set; }


    }
}