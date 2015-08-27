using HC.RM.Common.PayPal;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Payer = HC.RM.Common.PayPal.Models.Payer;
using Payment = HC.RM.Common.PayPal.Models.Payment;
using Transaction = HC.RM.Common.PayPal.Models.Transaction;

namespace RM.MailshotsOnline.Test
{
    public class PaypalServiceTests
    {
        private Service _service;

        [SetUp]
        public void SetupPaypalService()
        {
            _service = new Service();
        }

        private Payment CreatePaymentObject()
        {
            var payer = new Payer(HC.RM.Common.PayPal.Models.PayerPaymentMethod.paypal);

            var transaction = new Transaction("GBP", 132.00m, 110.00m, 22.00m, "Invoice number");
            transaction.CustomField = "Custom field";
            transaction.Description = "Description";

            var payment = new Payment(HC.RM.Common.PayPal.Models.PaymentIntent.order, payer, transaction, "http://localhost/", "http://localhost/");

            return payment;
        }

        [Test]
        public void CreatePaymentTest()
        {
            Assert.NotNull(_service);

            var payment = CreatePaymentObject();
            var result = _service.CreatePayment(payment);

            Assert.NotNull(result);
            Assert.IsNotNullOrEmpty(result.ApprovalUrl);
        }

        [Test]
        public void ExecutePaymentTest()
        {
            Assert.NotNull(_service);

            var payment = CreatePaymentObject();
            var createResult = _service.CreatePayment(payment);
            Assert.IsNotNullOrEmpty(createResult.ApprovalUrl);
            // TODO: Authorise this autmagically
            var executeResult = _service.ExecutePayment(createResult.Payer.Id, createResult);
            Assert.NotNull(executeResult);
        }
    }
}