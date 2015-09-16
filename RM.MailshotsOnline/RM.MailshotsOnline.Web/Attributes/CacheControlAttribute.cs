using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Filters;

namespace RM.MailshotsOnline.Web.Attributes
{
    public class CacheControlAttribute : System.Web.Http.Filters.ActionFilterAttribute
    {
        public int MaxAge { get; set; }

        public CacheControlAttribute()
        {
            MaxAge = 3600;
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            actionExecutedContext.Response.Headers.CacheControl = new System.Net.Http.Headers.CacheControlHeaderValue()
            {
                Public = true,
                MaxAge = TimeSpan.FromSeconds(MaxAge)
            };

            base.OnActionExecuted(actionExecutedContext);
        }
    }
}
