using Glass.Mapper.Umb.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.MailshotsOnline.Entities.PageModels
{
    [UmbracoType(AutoMap = true)]
    public class CaptureDetails : BasePage
    {
        public string BackToCampaignLinkText { get; set; }

        public string DetailsPrompt { get; set; }

        public string RequiredFieldPrompt { get; set; }

        public string ChooseAddressSubtitle { get; set; }

        public string BusinessNameLabel { get; set; }

        public string JobTitleLabel { get; set; }

        public string OrganisationAddressSubtitle { get; set; }

        public string FlatNumberLabel { get; set; }

        public string BuildingNumberLabel { get; set; }

        public string BuildingNameLabel { get; set; }

        public string StreetAddress1Label { get; set; }

        public string StreetAddress2Label { get; set; }

        public string TownLabel { get; set; }

        public string CountryLabel { get; set; }

        public string PostcodeLabel { get; set; }

        public string TelephoneNumbersSubtitle { get; set; }

        public string WorkPhoneLabel { get; set; }

        public string MobileNumberLabel { get; set; }

        public string ContinueToPaymentButton { get; set; }

        public BasePage CampaignHubPage { get; set; }

        public string BackToCampaignUrl { get; set; }

        public BasePage CampaignListingPage { get; set; }
    }
}
