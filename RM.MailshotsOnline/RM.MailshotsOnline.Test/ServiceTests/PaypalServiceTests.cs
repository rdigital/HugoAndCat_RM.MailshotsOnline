using HC.RM.Common.PayPal;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        private HC.RM.Common.PayPal.Models.Payment CreatePaymentObject()
        {
            var address = new HC.RM.Common.PayPal.Models.Address()
            {
                Line1 = "123 Some street",
                City = "London",
                PostalCode = "E1 6LP",
                RecipientName = "Test",
                CountryCode = "GB"
            };

            var payer = new HC.RM.Common.PayPal.Models.Payer()
            {
                FirstName = "Test",
                LastName = "Test",
                Email = "ext-mradford@hugoandcat.com",
                ShippingAddress = address,
                PaymentMethod = HC.RM.Common.PayPal.Models.PayerPaymentMethod.paypal
            };

            var transaction = new HC.RM.Common.PayPal.Models.Transaction()
            {
                Currency = "GBP",
                CustomField = "Custom Field",
                InvoiceNumber = "Invoice number",
                Description = "Description",
                Total = 132.00m,
                SubTotal = 110.00m,
                Tax = 22.00m
            };

            var payment = new HC.RM.Common.PayPal.Models.Payment()
            {
                Intent = HC.RM.Common.PayPal.Models.PaymentIntent.Order,
                Payer = payer,
                CancelUrl = "http://www.example.com/",
                ReturnUrl = "http://www.example.com/",
                Transactions = new List<HC.RM.Common.PayPal.Models.Transaction>() { transaction }
            };

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

            // This should fail
            var payment = CreatePaymentObject();
            var failResult = _service.ExecutePayment(null, payment);

            // This should not fail
            payment = CreatePaymentObject();
            var createResult = _service.CreatePayment(payment);
            Assert.IsNotNullOrEmpty(createResult.ApprovalUrl);
            // TODO: Authorise this autmagically
            var executeResult = _service.ExecutePayment(createResult.Payer.Id, createResult);
            Assert.NotNull(executeResult);
        }
    }
    }
}