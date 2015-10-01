using Glass.Mapper.Umb;
using HC.RM.Common.PCL.Helpers;
using RM.MailshotsOnline.Data.Constants;
using RM.MailshotsOnline.Entities.JsonModels;
using RM.MailshotsOnline.Entities.PageModels;
using RM.MailshotsOnline.PCL.Models;
using RM.MailshotsOnline.PCL.Services;
using RM.MailshotsOnline.Web.Extensions;
using RM.MailshotsOnline.Web.Helpers;
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
        private IInvoiceService _invoiceService;
        private ISettingsService _settingsService;
        private IUmbracoService _umbracoService;

        public OrdersController(IUmbracoService umbracoService, IInvoiceService invoiceService, ICampaignService campaignService, ISettingsService settingsService, IMembershipService membershipService, ILogger logger)
            : base (membershipService, logger)
        {
            _umbracoService = umbracoService;
            _campaignService = campaignService;
            _settingsService = settingsService;
            _invoiceService = invoiceService;
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
                    CampaignTitle = campaign.Name,
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

        public HttpResponseMessage Cancel(Guid id)
        {
            // Check the user is logged in
            var authResult = Authenticate();
            if (authResult != null)
            {
                return authResult;
            }

            // Get the campaign
            ICampaign campaign = _campaignService.GetCampaignWithInvoices(id);
            if (campaign == null)
            {
                _logger.Error(this.GetType().Name, "Cancel", "Unable to find campaign with ID {0}.", id);
                return ErrorMessage(HttpStatusCode.NotFound, "Unknown campaign.");
            }

            // Check the user owns it
            if (campaign.UserId != _loggedInMember.Id)
            {
                _logger.Error(this.GetType().Name, "Cancel", "Unauthorised attempt by user with ID {0} to cancel campaign with ID {1}.", _loggedInMember.Id, id);
                return ErrorMessage(HttpStatusCode.Forbidden, "Unauthorised request.");
            }

            // Cancel the campaign
            List<string> errorMessages;
            var invoiceHelper = new InvoiceHelper(_campaignService, _invoiceService, _logger);
            var success = invoiceHelper.CancelInvoice(campaign, out errorMessages);
            if (success)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { success = true });
            }
            else
            {
                string errorMessage = "Error cancelling campaign.";
                if (errorMessages != null && errorMessages.Any())
                {
                    errorMessage = errorMessages.FirstOrDefault();
                }

                return Request.CreateResponse(HttpStatusCode.BadRequest, new { success = false, error = errorMessage });
            }
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