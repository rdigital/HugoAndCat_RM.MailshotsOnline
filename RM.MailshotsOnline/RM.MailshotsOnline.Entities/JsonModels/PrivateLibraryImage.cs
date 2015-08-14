using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Glass.Mapper.Umb.Configuration.Attributes;
using RM.MailshotsOnline.Entities.MemberModels;
using RM.MailshotsOnline.PCL.Models;

namespace RM.MailshotsOnline.Entities.JsonModels
{
    [UmbracoType(AutoMap = true)]
    public class PrivateLibraryImage : Image
    {
        public string Username { get; set; }

        public string OriginalBlobId { get; set; }

        public string SmallThumbBlobId { get; set; }

        public string LargeThumbBlobId { get; set; }

        public string OriginalUrl { get; set; }

        public string SmallThumbUrl { get; set; }

        public string LargeThumbUrl { get; set; }
    }
}
