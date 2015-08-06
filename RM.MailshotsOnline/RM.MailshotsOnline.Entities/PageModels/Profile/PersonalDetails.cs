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
    public class PersonalDetails : EditProfileBasePage
    {
        public string RequiredFieldLabel { get; set; }

        #region Personal details section

        public string PasswordSectionTitle { get; set; }

        public string TitleLabel { get; set; }

        public string FirstNameLabel { get; set; }

        public string LastNameLabel { get; set; }

        public string EmailAddressLabel { get; set; }

        public string SavePersonalDetailsCtaText { get; set; }

        #endregion

        #region Password section

        public string PesonalDetailsSectionTitle { get; set; }

        public string CurrentPasswordLabel { get; set; }

        public string NewPasswordLabel { get; set; }

        public string ConfirmNewPasswordLabel { get; set; }

        public string SavePasswordCtaText { get; set; }

        #endregion
    }
}
