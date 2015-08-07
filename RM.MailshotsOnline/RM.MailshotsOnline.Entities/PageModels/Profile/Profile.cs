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
    public class Profile : EditProfileBasePage
    {
        public string MainTitle { get; set; }

        public string UpdatePersonalDetailsCtaText { get; set; }

        public BasePage PersonalDetailsPage { get; set; }

        public string OrganisationDetailsTitle { get; set; }

        public string UpdateOrganisationDetailsCtaText { get; set; }

        public BasePage OrganisationDetailsPage { get; set; }

        public string EmailTitle { get; set; }

        public string WorkNumberTitle { get; set; }

        public string MobileNumberTitle { get; set; }
    }
}
