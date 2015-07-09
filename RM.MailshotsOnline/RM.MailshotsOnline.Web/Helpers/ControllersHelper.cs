using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RM.MailshotsOnline.Web.Helpers
{
    /// <summary>
    /// Contains helpers for controllers
    /// </summary>
    public class ControllersHelper
    {
        /// <summary>
        /// Checks to see if the controller is an Umbraco controller
        /// </summary>
        /// <see cref="http://www.wearesicc.com/getting-started-with-umbraco-7-and-structuremap-v3/"/>
        public static bool IsUmbracoController(Type controllerType)
        {
            return controllerType.Namespace != null
               && controllerType.Namespace.StartsWith("umbraco", StringComparison.InvariantCultureIgnoreCase)
               && !controllerType.Namespace.StartsWith("umbraco.extensions", StringComparison.InvariantCultureIgnoreCase);
        }
    }
}