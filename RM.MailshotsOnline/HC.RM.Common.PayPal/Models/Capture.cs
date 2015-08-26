using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PplCapture = PayPal.Api.Capture;
using PplAmount = PayPal.Api.Amount;

namespace HC.RM.Common.PayPal.Models
{
    public class Capture : RelatedResource
    {
        internal Capture(PplCapture capture)
        {
            this.Id = capture.id;
            this.CreateTime = DateTime.Parse(capture.create_time);
            this.UpdateTime = DateTime.Parse(capture.update_time);
            this.ParentPaymentId = capture.parent_payment;
            if (capture.amount != null)
            {
                this.AmountCurrency = capture.amount.currency;
                this.AmountTotal = decimal.Parse(capture.amount.total);
            }
            this.Links = capture.links.ToHateoasLinks();
            this.IsFinalCapture = capture.is_final_capture.HasValue 
                ? capture.is_final_capture.Value 
                : false;
        }

        public bool IsFinalCapture { get; private set; }

        public CaptureState State { get; private set; }
    }

    public enum CaptureState
    {
        pending,
        completed,
        refunded,
        partially_refunded
    }
}
