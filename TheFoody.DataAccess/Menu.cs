//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TheFoody.DataAccess
{
    using System;
    using System.Collections.Generic;
    
    public partial class Menu
    {
        public int Menu_id { get; set; }
        public string Menu_name { get; set; }
        public string Description { get; set; }
        public Nullable<decimal> Price { get; set; }
        public string Photo { get; set; }
        public Nullable<int> Meal_Cat_IdFK { get; set; }
        public Nullable<int> RestaurantId { get; set; }
        public Nullable<System.DateTime> Created_date { get; set; }
    
        public virtual Meal_Category Meal_Category { get; set; }
        public virtual Restaurant Restaurant { get; set; }
    }
}
