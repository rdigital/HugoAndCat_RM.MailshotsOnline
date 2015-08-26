using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Order = HC.RM.Common.PayPal.Models.Order;
using Payment = HC.RM.Common.PayPal.Models.Payment;
using Authorization = HC.RM.Common.PayPal.Models.Authorization;
using Capture = HC.RM.Common.PayPal.Models.Capture;
using PplOrder = PayPal.Api.Order;
using PplPayment = PayPal.Api.Payment;
using PplPaymentExecution = PayPal.Api.PaymentExecution;
using PplCapture = PayPal.Api.Capture;

namespace HC.RM.Common.PayPal
{
    /// <summary>
    /// Represents the PayPal API service
    /// </summary>
    public class Service
    {
        private readonly Dictionary<string, string> _config = ConfigManager.Instance.GetProperties();
        private readonly OAuthTokenCredential _oauthTokenCred;

        /// <summary>
        /// Creates a new PayPal service wrapper
        /// </summary>
        public Service()
        {
            _oauthTokenCred = new OAuthTokenCredential(_config);
        }

        private string AccessToken
        {
            get
            {
                // Should cache/reuse and manage access tokens for us.
                return _oauthTokenCred.GetAccessToken();
            }
        }

        private APIContext ApiContext
        {
            get
            {
                return new APIContext(AccessToken) { Config = _config };
            }
        }

        /// <summary>
        /// Gets a Payment from PayPal based on the payment ID
        /// </summary>
        /// <param name="paymentId">ID of the payment from PayPal</param>
        /// <returns>Payment object</returns>
        public Payment GetPayment(string paymentId)
        {
            var context = ApiContext;

            var pplPayment = PplPayment.Get(context, paymentId);

            if (pplPayment == null)
            {
                return null;
            }

            var result = new Payment(pplPayment);

            return result;
        }

        /// <summary>
        /// Gets a PayPal Order based on the Order ID
        /// </summary>
        /// <param name="orderId">Order ID</param>
        /// <returns>The resulting Order object</returns>
        public Order GetOrder(string orderId)
        {
            var context = ApiContext;
            var pplOrder = PplOrder.Get(context, orderId);
            if (pplOrder == null)
            {
                return null;
            }

            var result = new Order(pplOrder);
            return result;
        }

        /// <summary>
        /// Authorizes an order
        /// </summary>
        /// <param name="order">Order to Authorize</param>
        /// <returns>Authorization object</returns>
        public Authorization AuthorizeOrder(Order order)
        {
            var context = ApiContext;
            var pplOrder = order.ToPaypalOrder();
            var pplAuth = PplOrder.Authorize(context, pplOrder);
            var result = new Authorization(pplAuth);
            return result;
        }

        /// <summary>
        /// Captures the full amount for an Order
        /// </summary>
        /// <param name="order">The Order to capture</param>
        /// <returns>Capture object</returns>
        public Capture CaptureFullAmountForOrder(Order order)
        {
            var context = ApiContext;
            var pplOrder = order.ToPaypalOrder();

            var pplCapture = new PplCapture()
            {
                is_final_capture = true
            };

            var capture = PplOrder.Capture(context, order.Id, pplCapture);
            var result = new Capture(capture);
            return result;
        }

        /// <summary>
        /// Creates a Paypal Payment using the details specified in the Payment object
        /// </summary>
        /// <param name="payment">The Payment to create</param>
        /// <returns>The resulting Payment object</returns>
        public Payment CreatePayment(Payment payment)
        {
            // Get the API context
            var context = ApiContext;

            // Create the Paypal Payment request object 
            var pplPayment = payment.ToPaypalPayment();

            // Send the request object and get the result
            var pplResult = PplPayment.Create(context, pplPayment);

            // Convert the result to the public type
            var result = new Payment(pplResult);
            return result;
        }

        /// <summary>
        /// Executes a Paypal Payment, meaning the user has successfully entered their details and is ready to finalise the transaction
        /// </summary>
        /// <param name="payerId">ID of the payer (appended to the Return URL)</param>
        /// <param name="payment">The Payment object</param>
        /// <returns>New Payment object with attached related resources</returns>
        public Payment ExecutePayment(string payerId, Payment payment)
        {
            // Get the API context
            var context = ApiContext;

            // Create the Paypal Payment request object 
            var pplPayment = payment.ToPaypalPayment();
            var execution = new PplPaymentExecution();
            execution.payer_id = payerId;
            execution.transactions = pplPayment.transactions; // Unsure if we need this?

            // Send the request object and get the result
            var pplResult = PplPayment.Execute(context, payment.Id, execution);

            // Convert the result to the public type
            var result = new Payment(pplResult);
            return result;
        }
    }
}
