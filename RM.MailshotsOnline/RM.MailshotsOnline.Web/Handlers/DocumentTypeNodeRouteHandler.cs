using System.Linq;
using System.Web.Routing;
using RM.MailshotsOnline.Entities.PageModels;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace RM.MailshotsOnline.Web.Handlers
{
    public class DocumentTypeNodeRouteHandler : TemplatedUmbracoVirtualNodeRouteHandler
    {
        private readonly string _documentTypeAlias;

        public DocumentTypeNodeRouteHandler(string documentTypeAlias)
        {
            _documentTypeAlias = documentTypeAlias;
        }

        protected override IPublishedContent FindContent(RequestContext requestContext, UmbracoContext umbracoContext)
        {
            UmbracoHelper helper = new UmbracoHelper(umbracoContext);
            string templateAlias = GetTemplateAlias(requestContext);

            var result = helper.TypedContentAtRoot()
                .First()
                .Descendants()
                .First(d => d.DocumentTypeAlias.InvariantEquals(_documentTypeAlias)
                         && d.GetTemplateAlias().InvariantEquals(templateAlias));

            return result;
        }
    }
}
