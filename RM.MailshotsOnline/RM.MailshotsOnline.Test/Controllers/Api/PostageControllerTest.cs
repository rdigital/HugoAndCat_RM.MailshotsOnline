﻿using HC.RM.Common.PCL.Helpers;
using NUnit.Framework;
using RM.MailshotsOnline.Data.Services;
using RM.MailshotsOnline.PCL.Services;
using RM.MailshotsOnline.Test.Mocks;
using RM.MailshotsOnline.Web.Controllers.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Tests.TestHelpers;
using Umbraco.Web;

namespace RM.MailshotsOnline.Test.Controllers.Api
{
    [TestFixture]
    public class PostageControllerTest : BaseDatabaseFactoryTest
    {
        private IMembershipService _membershipService;

        private IPricingService _pricingService;

        private PostageController _controller;

        private ILogger _logger;

        private UmbracoContext _umbracoContext;

        [SetUp]
        public void SetupPostage()
        {
            var umbracoHelper = new Helpers.UmbracoHelper();
            _membershipService = new MockMembershipService();
            //_pricingService = new PricingService("StorageContextEF");
            _pricingService = new MockPricingService();
            _logger = new MockLogger();
            _umbracoContext = umbracoHelper.GetMockContext();
            _controller = new PostageController(_membershipService, _pricingService, _umbracoContext, _logger);
        }

        [TearDown]
        public override void TearDown()
        {
            _membershipService = null;
            _pricingService = null;
            _controller = null;
            base.TearDown();
        }

        [Test]
        public void PostalOptionControllerReturnsValues()
        {
            // Act
            var postageOptions = _controller.Get();

            // Assert
            Assert.That(postageOptions, Is.Not.Null);
            Assert.That(postageOptions.Any(), Is.True);
        }

        //[Test]
        //public void PostalOptionControllerReturnsFilteredValues()
        //{
        //    // Setup
        //    int formatId = 1;

        //    // Act
        //    var postageOptions = _controller.GetForFormat(formatId);

        //    // Assert
        //    Assert.That(postageOptions, Is.Not.Null);
        //    Assert.That(postageOptions.Any(), Is.True);
        //    Assert.That(postageOptions.Any(p => p.FormatId != formatId), Is.False);
        //}
    }
}
