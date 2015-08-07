using Glass.Mapper.Umb.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.MailshotsOnline.Entities.PageModels
{
    /// <summary>
    /// Base web page properties
    /// </summary>
    [UmbracoType(AutoMap = true)]
    public class BasePage : Item
    {
        /// <summary>
        /// Page should display in navigation
        /// </summary>
        [UmbracoPropertyValue("displayInNavigation","true")]
        public virtual bool DisplayInNavigation { get; set; }

        /// <summary>
        /// Meta data description
        /// </summary>
        [UmbracoPropertyValue("metaDescription", "")]
        public virtual string MetaDescription { get; set; }

        /// <summary>
        /// Meta data keywords
        /// </summary>
        [UmbracoPropertyValue("metaKeywords", "")]
        public virtual string MetaKeywords { get; set; }

        /// <summary>
        /// Meta data page title
        /// </summary>
        [UmbracoPropertyValue("metaPageTitle", "")]
        public virtual string MetaPageTitle { get; set; }

        /// <summary>
        /// Navigation title
        /// </summary>
        [UmbracoPropertyValue("navigationTitle", "")]
        public virtual string NavigationTitle { get; set; }
    }
}
