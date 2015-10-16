using Glass.Mapper.Umb.Configuration.Attributes;
using RM.MailshotsOnline.Entities.DataModels;
using RM.MailshotsOnline.PCL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.MailshotsOnline.Entities.PageModels
{
    [UmbracoType(AutoMap = true)]
    public class CampaignHub : BasePage
    {
        #region Campaign Status Text

        public string DraftStatusTitle { get; set; }

        public string DraftStatusDescription { get; set; }

        public string ProcessingStatusTitle { get; set; }

        public string ProcessingStatusDescription { get; set; }

        public string DespatchedStatusTitle { get; set; }

        public string DespatchedStatusDescription { get; set; }

        public string DeliveredStatusTitle { get; set; }

        public string DeliveredStatusDescription { get; set; }

        public string CancelledStatusTitle { get; set; }

        public string FailedChecksStatusTitle { get; set; }

        public string PaymentDeclinedStatusTitle { get; set; }

        public string MoreInformationStatusPrompt { get; set; }

        public string OrderDetailsPageLinkText { get; set; }

        public BasePage OrderDetailsPage { get; set; }

        #endregion

        #region Introduction

        public string IntroText { get; set; }

        public string SummarySectionPrompt { get; set; }

        #endregion

        #region Data section

        public string DataSectionTitle { get; set; }

        public string DataSectionPrompt { get; set; }

        public string DataGetStartedPrompt { get; set; }

        public string RecipientsHeading { get; set; }

        public string ListNameHeading { get; set; }

        public string YourDataHeading { get; set; }

        public string OurDataHeading { get; set; }

        #endregion

        #region Design section

        public string DesignSectionTitle { get; set; }

        public string DesignSectionPrompt { get; set; }

        public string DesignGetStartedPrompt { get; set; }

        public BasePage CreateCanvasPage { get; set; }

        #endregion

        #region Campaign Summary

        public string SummarySectionTitle { get; set; }

        public string CheckoutButtonText { get; set; }

        public string ApprovePrompt { get; set; }

        public string PrintAndDeliveryHeading { get; set; }

        public string ShouldArriveByText { get; set; }

        public string ServiceFeesHeading { get; set; }

        public string MailshotsOnlineServiceFeeHeading { get; set; }

        public string DataSearchFeeHeading { get; set; }

        public string PostageLabel { get; set; }

        public string TaxHeading { get; set; }

        public string VatLabel { get; set; }

        public string TotalCostLabel { get; set; }

        public BasePage ProfileUpdatePage { get; set; }

        public BasePage PaymentConfirmationPage { get; set; }

        public BasePage PaymentCancelledPage { get; set; }

        #endregion

        #region Buttons and labels

        public string ApproveButtonText { get; set; }

        public string EditButtonText { get; set; }

        public string PreviewButtonText { get; set; }

        public string DownloadButtonText { get; set; }

        public string RemoveButtonText { get; set; }

        public string ApprovedLabel { get; set; }

        public string UnapproveButtonText { get; set; }

        public string FailedLabel { get; set; }
        
        #endregion

        #region Error messages

        public BasePage MyCampaignsPage { get; set; }

        public string MyCampaignsPageLinkText { get; set; }

        public string NoCampaignMessage { get; set; }

        public string CampaignErrorMessage { get; set; }

        #endregion

        #region Terms and conditions

        public string IAgreeToLabel { get; set; }

        public string TermsAndConditionsLinkText { get; set; }

        public ContentPage TermsAndConditionsPage { get; set; }

        #endregion

        public ICampaign Campaign { get; set; }

        public bool DisplayNoCampaignMessage { get; set; }

        public bool DisplayCampaignErrorMessage { get; set; }

        public ICampaignPriceBreakdown PriceBreakdown { get; set; }

        public IMember LoggedInMember { get; set; }

        public IEnumerable<IPostalOption> PostalOptions { get; set; }

        public IMailshot Mailshot { get; set; }
    }
}
