using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Glass.Mapper.Umb.Configuration.Attributes;
using RM.MailshotsOnline.Entities.MemberModels;
using RM.MailshotsOnline.PCL.Models;
using Newtonsoft.Json;

namespace RM.MailshotsOnline.Entities.JsonModels
{
    [UmbracoType(AutoMap = true)]
    public class PrivateLibraryImage : Image
    {
        [JsonIgnore]
        public string Username { get; set; }

        [JsonIgnore]
        public string OriginalBlobId { get; set; }

        [JsonIgnore]
        public string SmallThumbBlobId { get; set; }

        [JsonIgnore]
        public string LargeThumbBlobId { get; set; }

        public string OriginalUrl { get; set; }

        public string SmallThumbUrl { get; set; }

        public string LargeThumbUrl { get; set; }

        public override string Src
        {
            get
            {
                return OriginalUrl;
            }
        }

        public override string MediumSrc
        {
            get
            {
                return LargeThumbUrl;
            }
        }

        public override string SmallSrc
        {
            get
            {
                return SmallThumbUrl;
            }
        }

        public int ImageId
        {
            get { return base.MediaId; }
        }
    }
}
