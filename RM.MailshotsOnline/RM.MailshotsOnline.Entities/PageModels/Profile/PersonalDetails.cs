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
        public string MainTitle { get; set; }

        public string AddressTitle { get; set; }

        public string TelephoneNumbersTitle { get; set; }

        public string SaveChangesCtaText { get; set; }
    }
}
