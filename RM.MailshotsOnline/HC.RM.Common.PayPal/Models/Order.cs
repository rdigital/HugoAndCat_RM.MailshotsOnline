using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PplOrder = PayPal.Api.Order;
using PplAmount = PayPal.Api.Amount;

namespace HC.RM.Common.PayPal.Models
{
    public class Order : RelatedResource
    {
        /// <summary>
        /// Creates a new Order object from a PayPal Order
        /// </summary>
        /// <param name="pplOrder">PayPal Order</param>
        internal Order(PplOrder pplOrder)
        {
            this.Id = pplOrder.id;
            this.CreateTime = DateTime.Parse(pplOrder.create_time);
            this.UpdateTime = DateTime.Parse(pplOrder.update_time);
            this.ParentPaymentId = pplOrder.parent_payment;
            if (pplOrder.amount != null)
            {
                this.AmountCurrency = pplOrder.amount.currency;
                this.AmountTotal = decimal.Parse(pplOrder.amount.total);
            }
            this.Links = pplOrder.links.ToHateoasLinks();
            this.State = (OrderState)Enum.Parse(typeof(OrderState), pplOrder.state);
        }

        /// <summary>
        /// Gets the State of the Order
        /// </summary>
        public OrderState State { get; private set; }

        /// <summary>
        /// Converts the Order to a PayPal Order object
        /// </summary>
        /// <returns>PayPal Order object</returns>
        internal PplOrder ToPaypalOrder()
        {
            PplOrder result = new PplOrder();
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

            return result;
        }
    }

    /// <summary>
    /// Order State values
    /// </summary>
    public enum OrderState
    {
        pending,
        completed,
        refunded,
        partially_refunded
    }
}
