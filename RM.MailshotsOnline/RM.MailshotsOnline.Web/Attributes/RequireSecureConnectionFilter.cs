using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace RM.MailshotsOnline.Web.Attributes
{
    public class RequireSecureConnectionFilter : RequireHttpsAttribute
    {
        /// <summary>
        /// Forces HTTPS unless the request is local
        /// </summary>
        /// <see cref="http://tech.trailmax.info/2014/02/implemnting-https-everywhere-in-asp-net-mvc-application/"/>
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }

            if (filterContext.HttpContext.Request.IsLocal)
            {
                return;
            }

            base.OnAuthorization(filterContext);
        }
    }
}
