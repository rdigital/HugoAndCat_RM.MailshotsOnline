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

        public static string Url(this BasePage basePage)
        {
            var helper = new UmbracoHelper(UmbracoContext.Current);
            return Url(basePage, helper);
        }

        public static string GetParentUrl(this BasePage basePage)
        {
            var helper = new UmbracoHelper(UmbracoContext.Current);
            var parentId = -1;
            var pathIds = basePage.Path.ToIntList();
            if (pathIds.Count > 2)
            {
                parentId = pathIds[pathIds.Count - 2];
            }

            if (parentId > 0)
            {
                return helper.NiceUrl(parentId);
            }

            return null;
        }
    }
}
