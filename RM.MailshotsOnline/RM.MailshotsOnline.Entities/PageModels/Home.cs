using Glass.Mapper.Umb.Configuration.Attributes;
using Glass.Mapper.Umb.PropertyTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.MailshotsOnline.Entities.PageModels
{
    /// <summary>
    /// Home page
    /// </summary>
    [UmbracoType(AutoMap = true)]
    public class Home : BasePage
    {
        /// <summary>
        /// Main content
        /// </summary>
        [UmbracoPropertyValue("mainContent", "")]
        public virtual string MainContent { get; set; }

        /// <summary>
        /// The banner image
        /// </summary>
        public virtual Image BannerImage { get; set; }

        public virtual Login LoginPage { get; set; }
    }
}
