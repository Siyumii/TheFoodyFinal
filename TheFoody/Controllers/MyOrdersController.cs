using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheFoody.DataAccess;
using TheFoody.Models;
using System.Data.Entity;
using System.IO;

namespace TheFoody.Controllers
{
    public class MyOrdersController : Controller
    {
        TheFoodyContext db = new TheFoodyContext();

        // GET: MyOrders
        public ActionResult Index()
        {
            string email = Session["UserEmail"].ToString();
            var model = (from p in db.Orders
                         where (p.Cus_email == email)
                         select new MyOrdersViewModel()
                         {
                             orderid = p.Order_id,
                             restid = (int)p.Rest_id,
                             order_date = (DateTime)p.Order_date,
                             order_type = p.Order_type,
                             order_status=p.Order_status
                             
                         });

            //List<Orders> or = db.Orders.Where(x => x.Cus_email == email).ToList();
            return View(model.ToList());
        }
    }
}