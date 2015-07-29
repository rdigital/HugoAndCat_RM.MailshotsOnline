using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RM.MailshotsOnline.Entities.PageModels;
using Umbraco.Web;

namespace RM.MailshotsOnline.Web.Extensions
{
    public static class BasePageExtensions
    {
        public static string Url(this BasePage basePage, UmbracoHelper umbracoHelper)
        {
            return umbracoHelper.NiceUrl(basePage.Id);
        }
    }
}
