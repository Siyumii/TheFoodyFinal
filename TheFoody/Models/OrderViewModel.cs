using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TheFoody.DataAccess;

namespace TheFoody.Models
{
    public class OrderViewModel
    {
        public List<OrderDetailsModel> list { get; set; }
    }

    public class OrderDetailsModel
    {

        [Display(Name = "Order Id")]
        public int Order_id { get; set; }

        [Display(Name = "Restaurant Id")]
        public Nullable<int> Rest_id { get; set; }

        [Display(Name = "Restaurant Name")]
        public string RestaurantName { get; set; }

        [Display(Name = "Customer Email")]
        public string Cus_email { get; set; }

        [Display(Name = "Customer")]
        public string CustomerFirstName { get; set; }

        [Display(Name = "Ordered Date")]
        public Nullable<System.DateTime> Order_date { get; set; }

        [Display(Name = "Order Type")]
        public string Order_type { get; set; }

        [Display(Name = "Delivery Address")]
        public string Delivery_address { get; set; }

        [Display(Name = "Phone")]
        public string Phone { get; set; }

        [Display(Name = "Landmarks")]
        public string Landmarks { get; set; }

        [Display(Name = "Payment Status")]
        public string Payment_status { get; set; }

        [Display(Name = "Order Status")]
        public string Order_status { get; set; }

        [Display(Name = "Dispatched Date")]
        public Nullable<System.DateTime> Dispatched_date { get; set; }

        [Display(Name = "Total Price (Rs.)")]
        public Nullable<decimal> Total_price { get; set; }

        [Required]
        [Display(Name = "Delivery Man")]
        public string Deliver_Man { get; set; }

        [Display(Name = "Delivery / Pick Up Time")]
        public Nullable<System.TimeSpan> Delivery_time { get; set; }

        public List<OrderedMenusModel> Menus { get; set; }
    }

    public class OrderedMenusModel
    {

        public int Order_food_id { get; set; }

        [Display(Name = "Menu Id")]
        public Nullable<int> Menu_id { get; set; }

        [Display(Name = "Menu")]
        public string Menu { get; set; }

        [Display(Name = "Quantity")]
        public Nullable<int> Quantity { get; set; }

        [Display(Name = "Price (Rs.)")]
        public Nullable<decimal> Price { get; set; }
    }
}