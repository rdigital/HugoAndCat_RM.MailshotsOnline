﻿using Glass.Mapper.Umb.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.MailshotsOnline.Entities.PageModels
{
    [UmbracoType(AutoMap = true)]
    public class OrderDetails : BasePage
    {
        public string CostBreakdownLabel { get; set; }

        public string BillingAddressLabel { get; set; }

        //public MyOrders MyOrdersPage { get; set; }
    }
}
