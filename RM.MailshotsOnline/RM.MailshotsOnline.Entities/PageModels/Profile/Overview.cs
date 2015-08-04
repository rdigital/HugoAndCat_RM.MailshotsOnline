using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Glass.Mapper.Umb.Configuration.Attributes;
using RM.MailshotsOnline.Entities.ViewModels;

namespace RM.MailshotsOnline.Entities.PageModels.Profile
{
    [UmbracoType(AutoMap = true)]
    public class Overview : BasePage
    {
        public string PersonalDetailsTitle { get; set; }

        public string UpdatePersonalDetailsCtaText { get; set; }

        public string OrganisationDetailsTitle { get; set; }

        public string UpdateOrganisationDetailsCtaText { get; set; }

        public RegisterViewModel ProfileViewModel { get; set; }

        public string EmailTitle { get; set; }

        public string WorkNumberTitle { get; set; }

        public string MobileNumberTitle { get; set; }
    }
}
