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
            /// <summary>
            /// Campaign has draft data and/or mailshot data
            /// </summary>
            Draft = 1,

            /// <summary>
            /// Campaign is pending moderation
            /// </summary>
            PendingModeration = 2,

            /// <summary>
            /// Campaign has passed moderation and can be passed to the fulfilment house
            /// </summary>
            ReadyForFulfilment = 3,

            /// <summary>
            /// Campaign has been sent to the fulfilment house for printing
            /// </summary>
            SentForFulfilment = 4,

            /// <summary>
            /// Campaign has been fulfilled
            /// </summary>
            Fulfilled = 5,

            /// <summary>
            /// Campaign has confirmed data and mailshot content - Invoice can be created
            /// </summary>
            ReadyToCheckout = 6,

            /// <summary>
            /// Exception has occurred - most likely failed checks
            /// </summary>
            Exception = -1,

            /// <summary>
            /// Campaign has been cancelled
            /// </summary>
            Cancelled = 7,

            /// <summary>
            /// Payment for this campaign has failed
            /// </summary>
            PaymentFailed = -2
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

        /// <summary>
        /// Status for invoices
        /// </summary>
        public enum InvoiceStatus
        {
            /// <summary>
            /// Has not yet been passed to PayPal
            /// </summary>
            Draft = 1,

            /// <summary>
            /// User has been passed to PayPal - waiting for payment confirmation
            /// </summary>
            Submitted = 2,

            /// <summary>
            /// PayPal order has been created - waiting for moderation and fulfilment
            /// </summary>
            Processing = 3,

            /// <summary>
            /// Invoice has been paid
            /// </summary>
            Paid = 4,

            /// <summary>
            /// Invoice has been cancelled - PayPal order has been voided
            /// </summary>
            Cancelled = 5,

            /// <summary>
            /// Invoice was previously paid, but has been refunded
            /// </summary>
            Refunded = 6,

            /// <summary>
            /// Capturing payment from PayPal has failed
            /// </summary>
            Failed = 7
        }
    }
}
