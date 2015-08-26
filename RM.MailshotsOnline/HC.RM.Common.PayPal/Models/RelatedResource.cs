using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HC.RM.Common.PayPal.Models
{
    /// <summary>
    /// Base class for related resources
    /// </summary>
    public abstract class RelatedResource
    {
        /// <summary>
        /// ID of the resource
        /// </summary>
        public string Id { get; protected set; }

        /// <summary>
        /// Gets the Create time
        /// </summary>
        public DateTime CreateTime { get; protected set; }

        /// <summary>
        /// Gets the Update time
        /// </summary>
        public DateTime UpdateTime { get; protected set; }

        /// <summary>
        /// Gets the Parent Payment ID
        /// </summary>
        public string ParentPaymentId { get; protected set; }

        /// <summary>
        /// Gets the HATEOS Links
        /// </summary>
        public IEnumerable<HateoasLink> Links { get; protected set; }

        /// <summary>
        /// Gets the Amount total
        /// </summary>
        public decimal AmountTotal { get; protected set; }

        /// <summary>
        /// Gets the three-letter Amount Currency code
        /// </summary>
        public string AmountCurrency { get; protected set; }
    }
}
