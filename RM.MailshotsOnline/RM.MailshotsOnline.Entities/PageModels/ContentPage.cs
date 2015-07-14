using Glass.Mapper.Umb.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.MailshotsOnline.Entities.PageModels
{
    /// <summary>
    /// A normal content page
    /// </summary>
    [UmbracoType(AutoMap = true)]
    public class ContentPage : BasePage
    {
        /// <summary>
        /// Main content
        /// </summary>
        public string MainContent { get; set; }
    }
}
