using System.Web.Routing;
using Umbraco.Web.Mvc;

namespace RM.MailshotsOnline.Web.Handlers
{
    /// <summary>
    /// An Umbraco route handler that allows the retrieval of the template alias name from the route action. 
    /// </summary>
    public abstract class TemplatedUmbracoVirtualNodeRouteHandler : UmbracoVirtualNodeRouteHandler
    {
        /// <summary>
        /// Get the template from the current route.
        /// </summary>
        /// <param name="requestContext">
        /// The <see cref="RequestContext"/> containing information about the current HTTP request and route. 
        /// </param>
        /// <returns>
        /// The <see cref="string"/> representing the template alias.
        /// </returns>
        protected virtual string GetTemplateAlias(RequestContext requestContext)
        {
            return requestContext.RouteData.GetRequiredString("action");
        }
    }
}
