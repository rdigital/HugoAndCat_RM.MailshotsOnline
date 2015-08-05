using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Glass.Mapper.Umb.Configuration.Attributes;
using RM.MailshotsOnline.PCL.Models;

namespace RM.MailshotsOnline.Entities.PageModels
{
    [UmbracoType(AutoMap = true)]
    public class EditProfileBasePage : BasePage
    {
        [NotMapped]
        public IMember Member { get; set; }

        public string UpdateSuccessMessage { get; set; }

        public string UpdateFailMessage { get; set; }
    }
}
