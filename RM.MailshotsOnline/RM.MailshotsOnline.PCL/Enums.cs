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
            Exception = -1,
            Cancelled = 7
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
