using Glass.Mapper.Umb;
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
    public abstract class GlassController : RenderMvcController
    {
        /// <summary>
        /// The Umbraco Glass service
        /// </summary>
        protected readonly IUmbracoService _umbracoService;

        /// <summary>
        /// Creates a new instance of the GlassController class
        /// </summary>
        /// <param name="umbracoService">The Glass Umbraco service</param>
        public GlassController(IUmbracoService umbracoService)
        {
            _umbracoService = umbracoService;
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