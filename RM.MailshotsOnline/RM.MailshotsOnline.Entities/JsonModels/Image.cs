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
        public virtual string Src { get; set; }

        public virtual string MediumSrc
        {
            get { return GetSrcForSize(1000); }
        }

        public virtual string SmallSrc
        {
            get { return GetSrcForSize(200); }
        }

        public string Width { get; set; }

        public string Height { get; set; }

        public string Size
        {
            get { return (_size / 1024) + "KB"; }
            set
            {
                int size = 0;
                if (!int.TryParse(value, out size))
                {
                    size = 0;
                }
                _size = size;
            }
        }

        public string Type { get; set; }

        private int _size { get; set; }

        private string GetSrcForSize(int size)
        {
            if (!string.IsNullOrEmpty(Src))
            {
                var dotPosition = this.Src.LastIndexOf('.');
                return string.Format("{0}_{1}.{2}",
                    this.Src.Substring(0, dotPosition),
                    size,
                    this.Src.Substring(dotPosition + 1));
            }
            else
            {
                return null;
            }
        }
    }
}
