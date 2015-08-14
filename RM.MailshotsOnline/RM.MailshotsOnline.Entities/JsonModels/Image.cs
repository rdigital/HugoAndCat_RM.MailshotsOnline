using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Glass.Mapper.Umb.Configuration.Attributes;
using RM.MailshotsOnline.PCL.Models;

namespace RM.MailshotsOnline.Entities.JsonModels
{
    [UmbracoType(AutoMap = true)]
    public class Image : Media
    {
        public string Src { get; set; }

        public string MediumSrc
        {
            get { return GetSrcForSize(1000); }
        }

        public string SmallSrc
        {
            get { return GetSrcForSize(200); }
        }

        public string Width { get; set; }

        public string Height { get; set; }

        public string Size
        {
            get { return (_size / 1024) + "KB"; }
            set { _size = Convert.ToInt32(value); }
        }

        public string Type { get; set; }

        private int _size { get; set; }

        private string GetSrcForSize(int size)
        {
            var dotPosition = this.Src.LastIndexOf('.');
            return string.Format("{0}_{1}.{2}",
                this.Src.Substring(0, dotPosition),
                size,
                this.Src.Substring(dotPosition + 1));
        }
    }
}
