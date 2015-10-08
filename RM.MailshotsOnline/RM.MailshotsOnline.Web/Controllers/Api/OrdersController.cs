using Glass.Mapper.Umb;
using HC.RM.Common.Network;
using HC.RM.Common.Orders;
using HC.RM.Common.PCL.Helpers;
using Newtonsoft.Json;
using RM.MailshotsOnline.Data.Constants;
using RM.MailshotsOnline.Data.Helpers;
using RM.MailshotsOnline.Entities.JsonModels;
using RM.MailshotsOnline.Entities.PageModels;
using RM.MailshotsOnline.Entities.ViewModels;
using RM.MailshotsOnline.PCL.Models;
using RM.MailshotsOnline.PCL.Services;
using RM.MailshotsOnline.Web.Extensions;
using RM.MailshotsOnline.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
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
        private IEmailService _emailService;

        public OrdersController(IUmbracoService umbracoService, IInvoiceService invoiceService, ICampaignService campaignService, ISettingsService settingsService, IMembershipService membershipService, ILogger logger, IEmailService emailService)
            : base (membershipService, logger)
        {
            _umbracoService = umbracoService;
            _campaignService = campaignService;
            _settingsService = settingsService;
            _invoiceService = invoiceService;
            _emailService = emailService;
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
                    InvoiceStatus = invoice.Status.ToString(),
                    Status = campaign.Status.ToString(),
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

        [HttpPost]
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

        [HttpPost]
        public async Task<HttpResponseMessage> InvoicePdfReady(Guid id, OrderResults orderResults)
        {
            _logger.Info(this.GetType().Name, "InvoicePdfReady", "Invoice render complete request recieved for invoice ID {0}.", id);
            var invoice = _invoiceService.GetInvoice(id);
            if (invoice == null)
            {
                _logger.Error(this.GetType().Name, "InvoicePdfReady", "Unable to get invoice with ID {0}.", id);
                return ErrorMessage(HttpStatusCode.BadRequest, "Invoice not found");
            }

            // Save the PDF to blob storage
            Collection<SparqOrderResult> parsedResults = null;
            SparqOrderResult result = null;
            byte[] pdfBytes = null;
            try
            {
                parsedResults = JsonConvert.DeserializeObject<Collection<SparqOrderResult>>(orderResults.Results);
                result = parsedResults.FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.Exception(this.GetType().Name, "InvoicePdfReady", ex);
                _logger.Error(this.GetType().Name, "InvoicePdfReady", "Error parsing information back from render service.");
            }

            if (result != null)
            {
                _logger.Info(this.GetType().Name, "InvoicePdfReady", "Invoice PDF for invoice {0} returned status {1}.", id, result.Status);

                if (result.Errors != null && result.Errors.Any())
                {
                    foreach (var error in result.Errors)
                    {
                        _logger.Warn(this.GetType().Name, "InvoicePdfReady", "Error rendering Invoice PDF for invoice {0}: {1}", id, error.Message);
                    }
                }

                if (!string.IsNullOrEmpty(result.BlobId))
                {
                    invoice.InvoicePdfBlobReference = result.BlobId;
                    _invoiceService.Save(invoice);
                    var serviceLayerBlobHelper = new BlobStorageHelper(ConfigHelper.SparqServiceBlobConnectionString, ConfigHelper.SparqServiceBlobContainer);
                    try
                    {
                        pdfBytes = await serviceLayerBlobHelper.FetchBytesAsync(result.BlobId);
                    }
                    catch (Exception ex)
                    {
                        _logger.Exception(this.GetType().Name, "InvoicePdfReady", ex);
                        _logger.Error(this.GetType().Name, "InvoicePdfReady", "Unable to download PDF from blob storage.  Blob ID {0}", result.BlobId);
                    }
                }
            }

            // Email user to let them know the payment has been captured and attach the PDF
            var navigationPage = Umbraco.Content(Constants.Settings.HeaderNavSettingsId);
            var moderationPage = _umbracoService.GetItem<ModerationPage>((int)navigationPage.ModerationPage);
            if (moderationPage != null)
            {
                var recipient = _membershipService.GetMemberById(invoice.Campaign.UserId);
                var emailBody = moderationPage.CampaignFulfilledEmail
                    .Replace("{user}", recipient.FirstName)
                    .Replace("{campaign}", invoice.Campaign.Name)
                    .Replace("{total}", string.Format("£{0:F2}", invoice.Total));
                var emailSubject = moderationPage.CampaignFulfilledEmailSubject;
                var recipientList = new List<string>() { recipient.EmailAddress };
                if (pdfBytes != null)
                {
                    using (var stream = new MemoryStream(pdfBytes))
                    {
                        await _emailService.SendEmailAsync(recipientList, emailSubject, emailBody, System.Net.Mail.MailPriority.Normal, null, stream, "invoice.pdf");
                    }
                }
                else
                {
                    await _emailService.SendEmailAsync(recipientList, emailSubject, emailBody);
                }
            }

            return Request.CreateResponse(HttpStatusCode.OK);
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