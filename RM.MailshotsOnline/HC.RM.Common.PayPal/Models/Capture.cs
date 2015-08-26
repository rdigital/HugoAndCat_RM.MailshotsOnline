using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PplCapture = PayPal.Api.Capture;
using PplAmount = PayPal.Api.Amount;

namespace HC.RM.Common.PayPal.Models
{
    /// <summary>
    /// Represents a funds capture request
    /// </summary>
    public class Capture : RelatedResource
    {
        /// <summary>
        /// Creates a new Capture object from a PayPal Capture
        /// </summary>
        /// <param name="capture">PayPal Capture object</param>
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

        /// <summary>
        /// Gets or sets whether or not this is a final capture
        /// </summary>
        public bool IsFinalCapture { get; set; }

        /// <summary>
        /// Gets the state of the capture
        /// </summary>
        public CaptureState State { get; private set; }

        /// <summary>
        /// Converts the Capture to a PayPal capture object
        /// </summary>
        /// <returns>PayPal Capture object</returns>
        internal PplCapture ToPaypalCapture()
        {
            var result = new PplCapture();
            result.id = this.Id;
            result.create_time = this.CreateTime.ToUniversalTime().ToString("O");
            result.update_time = this.UpdateTime.ToUniversalTime().ToString("O");
            result.parent_payment = this.ParentPaymentId;
            result.amount = new PplAmount()
            {
                total = this.AmountTotal.ToString("F"),
                currency = this.AmountCurrency
            };
            result.links = this.Links.ToPplLinks();
            result.state = this.State.ToString();
            result.is_final_capture = this.IsFinalCapture;

            return result;
        }
    }

    /// <summary>
    /// Capture State
    /// </summary>
    public enum CaptureState
    {
        pending,
        completed,
        refunded,
        partially_refunded
    }
}
