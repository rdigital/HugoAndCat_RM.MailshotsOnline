using Glass.Mapper.Umb.Configuration.Attributes;
using RM.MailshotsOnline.PCL;
using RM.MailshotsOnline.PCL.Models;

namespace RM.MailshotsOnline.Entities.PageModels
{
    [UmbracoType]
    public class ListCreate : BasePage
    {
        // Upload File
        [UmbracoProperty]
        public string AddContactsTitle { get; set; }

        [UmbracoProperty]
        public string AddContactsIntroText { get; set; }

        [UmbracoProperty]
        public string ListNameLabel { get; set; }

        [UmbracoProperty]
        public string NewListPlaceholderText { get; set; }

        [UmbracoProperty]
        public string AddContactsHelpText { get; set; }

        [UmbracoProperty]
        public string AddContactsNotesTitle { get; set; }

        [UmbracoProperty]
        public string AddContactsNotesText { get; set; }

        [UmbracoProperty]
        public string UploadTitle { get; set; }

        [UmbracoProperty]
        public string ChooseFileText { get; set; }

        [UmbracoProperty]
        public string IndividualContactCreationText { get; set; }

        [UmbracoProperty]
        public string IndividualContactButtonText { get; set; }

        [UmbracoProperty]
        public string UploadingText { get; set; }

        // Confirm Contacts
        [UmbracoProperty]
        public string ConfirmTitle { get; set; }

        [UmbracoProperty]
        public string ConfirmIntroText { get; set; }

        [UmbracoProperty]
        public string ImportFirstRowLabel { get; set; }

        [UmbracoProperty]
        public string FirstRowIsHeaderLabel { get; set; }

        [UmbracoProperty]
        public string FirstRowIsContactLabel { get; set; }

        [UmbracoProperty]
        public string YourColumnHeaderText { get; set; }

        [UmbracoProperty]
        public string SampleDataHeaderText { get; set; }

        [UmbracoProperty]
        public string MapsToHeaderText { get; set; }

        [UmbracoProperty]
        public string CancelImportText { get; set; }

        [UmbracoProperty]
        public string FinishAndImportText { get; set; }

        [UmbracoProperty]
        public string ImportingText { get; set; }

        [UmbracoProperty]
        public string FileUploadedSuccessfullyText { get; set; }

        [UmbracoProperty]
        public string SummaryTitle { get; set; }

        [UmbracoProperty]
        public string SummaryIntroText { get; set; }

        [UmbracoProperty]
        public string SummaryContactsAddedText { get; set; }

        [UmbracoProperty]
        public string SummaryContactsAddedReportText { get; set; }

        [UmbracoProperty]
        public string SummaryContactsErrorText { get; set; }

        [UmbracoProperty]
        public string SummaryContactsErrorReportText { get; set; }

        [UmbracoProperty]
        public string SummaryContactsDuplicatedText { get; set; }

        [UmbracoProperty]
        public string SummaryContactsDuplicatedReportSummary { get; set; }

        [UmbracoProperty]
        public string SummaryCancelButtonText { get; set; }

        [UmbracoProperty]
        public string SummaryFinishButtonText { get; set; }

        [UmbracoProperty]
        public string SummaryContactsErrorTitle { get; set; }

        [UmbracoProperty]
        public string SummaryContactsErrorIntroText { get; set; }

        [UmbracoProperty]
        public string SummaryContactsDownloadErrorText { get; set; }

        [UmbracoProperty]
        public string SummaryContactAddedToList { get; set; }

        [UmbracoProperty]
        public string SummaryContactsDuplicateTitle { get; set; }

        [UmbracoProperty]
        public string SummaryContactsDuplicateIntroText { get; set; }

        [UmbracoProperty]
        public string SummaryNameHeaderText { get; set; }

        [UmbracoProperty]
        public string SummaryAddressHeaderText { get; set; }

        public IDistributionList DistributionList { get; set; }

        public Enums.DistributionListState CurrentStep { get; set; }
    }
}
