using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RM.MailshotsOnline.PCL
{
    public class Enums
    {
        /// <summary>
        /// Status for campaigns
        /// </summary>
        public enum CampaignStatus
        {
            Draft = 1,
            PendingModeration = 2,
            ReadyForFulfilment = 3,
            SentForFulfilment = 4,
            Fulfilled = 5,
            ReadyToCheckout = 6,
            Exception = -1
        }

        /// <summary>
        /// Status for Orders
        /// </summary>
        public enum OrderStatus
        {
            Draft = 1,
            PendingPayment = 2,
            Paid = 3,
            Complete = 4,
            Exception = -1
        }

        /// <summary>
        /// Status for Contacts
        /// </summary>
        public enum ContactStatus
        {
            New = 1,
            SentTo = 2,
            Inactive = 3
        }

        /// <summary>
        /// Status for PDF render jobs
        /// </summary>
        public enum PdfRenderStatus
        {
            None = 1,
            Pending = 2,
            Complete = 3,
            Failed = 4
        }

        /// <summary>
        /// Status for third-party data search requests
        /// </summary>
        public enum DataSearchStatus
        {
            Draft = 1,
            Finalised = 2,
            Purchased = 3,
            Fulfilled = 4
        }

        public enum InvoiceStatus
        {
            Draft = 1,
            Submitted = 2,
            Processing = 3,
            Paid = 4,
            Cancelled = 5,
            Refunded = 6
        }
    }
}
