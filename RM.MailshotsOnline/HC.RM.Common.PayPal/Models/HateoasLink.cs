using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PplLinks = PayPal.Api.Links;

namespace HC.RM.Common.PayPal.Models
{
    /// <summary>
    /// Hypermedia as the Engine of Application State
    /// </summary>
    public class HateoasLink
    {
        internal HateoasLink (PplLinks links)
        {
            this.Href = links.href;
            this.Rel = links.rel;
            this.Method = links.method;
        }

        public string Href { get; set; }

        public string Rel { get; set; }

        public string Method { get; set; }

        internal PplLinks ToPaypalLink()
        {
            PplLinks link = new PplLinks();

            link.href = Href;
            link.method = Method;
            link.rel = Rel;

            return link;
        }
    }
}
