using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HC.RM.Common.PayPal.Models
{
    public abstract class RelatedResource
    {
        public string Id { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime UpdateTime { get; set; }

        public string ParentPaymentId { get; set; }

        public IEnumerable<HateoasLink> Links { get; set; }

        public decimal AmountTotal { get; set; }

        public string AmountCurrency { get; set; }
    }
}
