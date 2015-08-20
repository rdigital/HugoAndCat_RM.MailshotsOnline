using Glass.Mapper.Umb;
using HC.RM.Common.Azure;
using HC.RM.Common.PCL.Helpers;
using RM.MailshotsOnline.Web.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;

namespace RM.MailshotsOnline.Web.Controllers
{
    /// <summary>
    /// Base controller when wishing to use Glass models
    /// </summary>
    [RequireSecureConnectionFilter]
    public abstract class GlassController : RenderMvcController
    {
        /// <summary>
        /// The Application Insights telemetry client
        /// </summary>
        protected readonly ILogger _logger;

        /// <summary>
        /// The Umbraco Glass service
        /// </summary>
        protected readonly IUmbracoService _umbracoService;

        /// <summary>
        /// Creates a new instance of the GlassController class
        /// </summary>
        /// <param name="umbracoService">The Glass Umbraco service</param>
        public GlassController(IUmbracoService umbracoService, ILogger logger)
        {
            _umbracoService = umbracoService;
            _logger = logger;
        }

        /// <summary>
        /// Gets the Glass model from the normal Umbraco model
        /// </summary>
        /// <typeparam name="TModel">Glass model type to fetch</typeparam>
        /// <returns>Umbraco content model</returns>
        public TModel GetModel<TModel>() where TModel : class
        {
            return _umbracoService.CreateType<TModel>(_umbracoService.ContentService.GetPublishedVersion(this.Umbraco.AssignedContentItem.Id), false, false);
        }
    }
}