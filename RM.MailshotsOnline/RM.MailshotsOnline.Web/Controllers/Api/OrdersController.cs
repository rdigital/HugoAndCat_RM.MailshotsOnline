using Glass.Mapper.Umb;
using HC.RM.Common.PCL.Helpers;
using RM.MailshotsOnline.Data.Constants;
using RM.MailshotsOnline.Entities.JsonModels;
using RM.MailshotsOnline.Entities.PageModels;
using RM.MailshotsOnline.PCL.Models;
using RM.MailshotsOnline.PCL.Services;
using RM.MailshotsOnline.Web.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace RM.MailshotsOnline.Web.Controllers.Api
{
    [Authorize]
    public class OrdersController : ApiBaseController
    {
        private ICampaignService _campaignService;
        private ISettingsService _settingsService;
        private IUmbracoService _umbracoService;

        public OrdersController(IUmbracoService umbracoService, ICampaignService campaignService, ISettingsService settingsService, IMembershipService membershipService, ILogger logger)
            : base (membershipService, logger)
        {
            _umbracoService = umbracoService;
            _campaignService = campaignService;
            _settingsService = settingsService;
        }

        [HttpGet]
        public HttpResponseMessage GetAll()
        {
            // Check the user is logged in
            var authResult = Authenticate();
            if (authResult != null)
            {
                return authResult;
            }

            // Get Orders (campaigns that have invoices) from the service
            var campaigns = _campaignService.GetOrdersForUser(_loggedInMember.Id);

            // Get My Orders page
            var settings = _settingsService.GetCurrentSettings();
            var navSettings = Umbraco.Content(Constants.Settings.HeaderNavSettingsId);
            var myOrdersPage = _umbracoService.GetItem<MyOrders>((int)navSettings.MyAccountsPage);
            myOrdersPage.Settings = settings;
            var orderDetailsPage = Umbraco.Content(myOrdersPage.OrderDetailsPage).Url();

            // Create the order list response
            var orderList = new List<Order>();
            foreach (var campaign in campaigns)
            {
                var invoice = campaign.LatestInvoice();
                orderList.Add(new Order()
                {
                    CampaignId = campaign.CampaignId,
                    OrderNumber = invoice.OrderNumber ?? invoice.PaypalOrderId,
                    OrderPlaced = campaign.OrderPlacedDate.HasValue ? campaign.OrderPlacedDate.Value : invoice.CreatedDate,
                    Status = invoice.Status.ToString(),
                    InvoiceUrl = GetInvoicePdfUrl(invoice),
                    ShowCancelLink = campaign.Status == PCL.Enums.CampaignStatus.PendingModeration,
                    StatusText = myOrdersPage.GetStatusText(campaign),
                    StatusDescription = myOrdersPage.GetStatusDescription(campaign),
                    Total = invoice.Total,
                    OrderDetailsUrl = $"{orderDetailsPage}?campaignId={campaign.CampaignId}"
                });
            }

            return Request.CreateResponse(HttpStatusCode.OK, orderList);
        }

        private string GetInvoicePdfUrl(IInvoice invoice)
        {
            if (invoice.Status == PCL.Enums.InvoiceStatus.Paid)
            {
                return "#invoice";
            }

            return null;
        }
    }
}