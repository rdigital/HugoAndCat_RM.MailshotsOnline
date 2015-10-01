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

        /// <summary>
        /// The type of file being stored against a distribution list
        /// </summary>
        public enum DistributionListFileType
        {
            /// <summary>
            /// A "final" distribution list, should be an XML document.
            /// </summary>
            Final = 0,
            /// <summary>
            /// An uploaded, unprocessed file, if it's a CSV it's probably not been processed.
            /// </summary>
            Working,
            /// <summary>
            /// A set of invalid records that could not be imported into a final list. 
            /// </summary>
            Errors,
        }

        /// <summary>
        /// What state is the Distribution List in?
        /// </summary>
        public enum DistributionListState
        {
            /// <summary>
            /// The list hasn't been configured at all yet.
            /// </summary>
            Unknown = 0,
            /// <summary>
            /// Not to be saved on a list - used when about to upload new contacts.
            /// </summary>
            AddNewContacts,
            /// <summary>
            /// The list has had a CSV uploaded against it, but these haven't been mapped to contacts yet.
            /// </summary>
            ConfirmFields,
            /// <summary>
            /// The CSV has been processed, but hasn't been merged into the list yet.
            /// </summary>
            FixIssues,
            /// <summary>
            /// The list is good to use.
            /// </summary>
            Complete,
        }
    }
}
