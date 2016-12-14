using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheFoody.Models
{
    public class MailModel
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

    }

    public class OrderDispatchMailModel
    {
        public string To { get; set; }
        public string Subject { get; set; }

        public OrderDetailsModel Order { get; set; }

        public string Body { get; set; }

    }
}