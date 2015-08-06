using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Glass.Mapper.Umb.Configuration.Attributes;
using RM.MailshotsOnline.Entities.ViewModels;
using RM.MailshotsOnline.PCL.Models;

namespace RM.MailshotsOnline.Entities.PageModels.Profile
{
    [UmbracoType(AutoMap = true)]
    public class OrganisationDetails : EditProfileBasePage
    {
        public string RequiredFieldLabel { get; set; }

        public string MainTitle { get; set; }

        public string AddressTitle { get; set; }

        public string TelephoneNumbersTitle { get; set; }

        public string OrganisationNameLabel { get; set; }

        public string JobTitleLabel { get; set; }

        public string FlatNumberLabel { get; set; }

        public string BuildingNumberLabel { get; set; }

        public string BuildingNameLabel { get; set; }

        public string Address1Label { get; set; }

        public string Address2Label { get; set; }

        public string CityLabel { get; set; }

        public string PostcodeLabel { get; set; }

        public string CountryLabel { get; set; }

        public string WorkNumberLabel { get; set; }

        public string MobileNumberLabel { get; set; }

        public string SaveChangesCtaText { get; set; }
    }
}
