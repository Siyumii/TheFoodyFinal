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

        

        public Item()
        {

        }

        public Item(CartItem cartitem)
        {
            this.cartitem = cartitem;
        }

        
    }
}