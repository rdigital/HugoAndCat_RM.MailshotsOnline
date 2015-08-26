using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PplLinks = PayPal.Api.Links;

namespace HC.RM.Common.PayPal.Models
{
    /// <summary>
    /// Hypermedia as the Engine of Application State Link
    /// </summary>
    public class HateoasLink
    {
        /// <summary>
        /// Creates a HateosLink from a PayPal Links object
        /// </summary>
        /// <param name="links">PayPal Links object</param>
        internal HateoasLink (PplLinks links)
        {
            this.Href = links.href;
            this.Rel = links.rel;
            this.Method = links.method;
        }

        /// <summary>
        /// The HREF / URL of the link
        /// </summary>
        public string Href { get; set; }

        /// <summary>
        /// The purpose of the link
        /// </summary>
        public string Rel { get; set; }

        /// <summary>
        /// The HTTP verb used to access the link
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// Converts the HateosLink to a PayPal Links object
        /// </summary>
        /// <returns>PayPal Links object</returns>
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
