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
    public class Image :IImage
    {
        [UmbracoPropertyValue("name", "")]
        public string Name { get; set; }

        [UmbracoPropertyValue("umbracoFile", "")]
        public string Src { get; set; }

        [UmbracoPropertyValue("umbracoWIdth", "")]
        public string Width { get; set; }

        [UmbracoPropertyValue("umbracoHeight", "")]
        public string Height { get; set; }

        [UmbracoPropertyValue("umbracoBytes", "")]
        public string Size
        {
            get { return (_size / 1024) + "KB"; }
            set { _size = Convert.ToInt32(value); }
        }

        [UmbracoPropertyValue("umbracoExtension", "")]
        public string Type { get; set; }

        private int _size { get; set; }
    }
}
