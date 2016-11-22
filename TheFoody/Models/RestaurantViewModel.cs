using System;
using System.Collections.Generic;
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
        public double TotalPrice { get; set; }
    }
    
    
}