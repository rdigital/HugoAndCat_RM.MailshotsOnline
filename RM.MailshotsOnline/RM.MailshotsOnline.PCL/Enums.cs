using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RM.MailshotsOnline.PCL
{
    public class Enums
    {
        public enum CampaignStatus
        {
            Draft = 1,
            PendingModeration = 2,
            Ready = 3,
            SentForFulfilment = 4,
            Complete = 5,
            Exception = -1
        }

        public enum OrderStatus
        {
            Draft = 1,
            PendingPayment = 2,
            Paid = 3,
            Complete = 4,
            Exception = -1
        }

        public enum ContactStatus
        {
            New = 1,
            SentTo = 2,
            Inactive = 3
        }
    }
}
