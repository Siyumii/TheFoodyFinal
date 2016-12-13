using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TheFoody.Models;

namespace TheFoody.Controllers
{
    public class Item
    {
        private CartItem cartitem=new CartItem();
        private int quantity;

        public CartItem Cartitem
        {
            get
            {
                return cartitem;
            }

            set
            {
                cartitem = value;
            }
        }

        public int Quantity
        {
            get {return quantity; }
            set { quantity= value; }
        }

        public Item()
        {

        }

        public Item(CartItem cartitem)
        {
            this.cartitem = cartitem;
        }

        
    }
}