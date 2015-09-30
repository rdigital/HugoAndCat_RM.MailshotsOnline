using HC.RM.Common.PayPal.Models;
using HC.RM.Common.PCL.Helpers;
using RM.MailshotsOnline.PCL.Models;
using RM.MailshotsOnline.PCL.Services;
using RM.MailshotsOnline.Web.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PayPalService = HC.RM.Common.PayPal.Service;

namespace RM.MailshotsOnline.Web.Helpers
{
    public class InvoiceHelper
    {
        private ICampaignService _campaignService;
        private IInvoiceService _invoiceService;
        private ILogger _logger;

        public InvoiceHelper(ICampaignService campaignService, IInvoiceService invoiceService, ILogger logger)
        {
            _campaignService = campaignService;
            _invoiceService = invoiceService;
            _logger = logger;
        }

        public bool CancelInvoice(ICampaign campaign, out List<string> errorMessages)
        {
            bool success = true;
            errorMessages = new List<string>();

            // Confirm that the appropriate invoice is in a state that can be cancelled.
            var cancelableCampaignStatuses = new List<PCL.Enums.CampaignStatus>()
            {
                PCL.Enums.CampaignStatus.PendingModeration
            };

            if (!cancelableCampaignStatuses.Contains(campaign.Status))
            {
                errorMessages.Add("Campaign cannot be cancelled");
                return false;
            }

            // Get the invoice
            var invoice = campaign.LatestInvoice();
            if (invoice == null)
            {
                errorMessages.Add("No invoice to be cancelled");
                return false;
            }

            // Check the invoice can be cancelled
            var cancelableInvoiceStatuses = new List<PCL.Enums.InvoiceStatus>()
            {
                PCL.Enums.InvoiceStatus.Submitted,
                PCL.Enums.InvoiceStatus.Processing,
                PCL.Enums.InvoiceStatus.Draft
            };

            if (!cancelableInvoiceStatuses.Contains(invoice.Status))
            {
                errorMessages.Add("The invoice cannot be cancelled.");
                return false;
            }

            // Cancel PayPal Order
            if (!string.IsNullOrEmpty(invoice.PaypalOrderId))
            {
                // Void PayPal order
                var paypalService = new PayPalService();
                Order order = null;
                try
                {
                    order = paypalService.GetOrder(invoice.PaypalOrderId);
                    paypalService.VoidOrder(order);
                }
                catch (Exception ex)
                {
                    _logger.Exception(this.GetType().Name, "CancelInvoice", ex);
                    _logger.Error(this.GetType().Name, "CancelInvoice", "Unable to cancel PayPal order {0}.", invoice.PaypalOrderId);
                    errorMessages.Add("Unable to cancel PayPal Order.");
                    return false;
                }
            }

            // Update invoice
            invoice.Status = PCL.Enums.InvoiceStatus.Cancelled;
            _invoiceService.Save(invoice);

            // Update Campaign
            campaign.Status = PCL.Enums.CampaignStatus.Cancelled;
            campaign.CancelledDate = DateTime.UtcNow;
            _campaignService.SaveCampaign(campaign);

            return success;
        }
    }
}
