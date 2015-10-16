using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Glass.Mapper.Umb.Configuration.Attributes;
using RM.MailshotsOnline.Entities.ViewModels;

namespace RM.MailshotsOnline.Entities.PageModels
{
    [UmbracoType(AutoMap = true)]
    public class Register : BasePage
    {
        public int  Page { get; set; }

        public string RequiredFieldsLabel { get; set; }

        public string EmailLabel { get; set; }

        public string ConfirmEmailLabel { get; set; }

        public string TitleLabel { get; set; }

        public string PasswordErrorMessage { get; set; }

        public string AlreadyRegisteredMessage { get; set; }

        public IDictionary<string, string> TitleOptions { get; set; } 

        public string FirstNameLabel { get; set; }

        public string LastNameLabel { get; set; }

        public string PasswordLabel { get; set; }

        public string ConfirmPasswordLabel { get; set; }

        public string RoyalMailContactOptionsDisclaimer { get; set; }

        public string RoyalMailContactOptionsLabel { get; set; }

        public string ThirdPartyContactOptionsDisclaimer { get; set; }

        public string ThirdPartyContactOptionsLabel { get; set; }

        public string AgreeToTermsAndConditionsLabel { get; set; }

        public BasePage TermsAndConditionsPage { get; set; }

        public string TermsAndConditionsLinkText { get; set; }

        public string TermsAndConditionsLabelWithLink { get; set; }

        public string KeepYouInformedTitle { get; set; }

        public string KeepYouInformedSectionCopy { get; set; }

        public string NextButtonText { get; set; }

        public string PostcodeLabel { get; set; }

        public string OrganisationNameLabel { get; set; }

        public string JobTitleLabel { get; set; }

        public string FlatNumberLabel { get; set; }

        public string BuildingNumberLabel { get; set; }

        public string BuildingNameLabel { get; set; }

        public string Address1Label { get; set; }

        public string Address2Label { get; set; }

        public string CityLabel { get; set; }

        public string CountryLabel { get; set; }

        public string WorkPhoneNumberLabel { get; set; }

        public string MobilePhoneNumberLabel { get; set; }

        public string RegisterCtaText { get; set; }

        public string ThankYouLabel { get; set; }

        public string MyAccountCtaText { get; set; }

        public Profile.Profile MyProfilePage { get; set; }

        public string RegisterCompleteEmail { get; set; }

        public MarketingPreferencesViewModel RoyalMailMarketingPreferencesViewModel { get; set; }

        public MarketingPreferencesViewModel ThirdPartyMarketingPreferencesViewModel { get; set; }

        public RegisterViewModel ViewModel { get; set; }
    }
}
