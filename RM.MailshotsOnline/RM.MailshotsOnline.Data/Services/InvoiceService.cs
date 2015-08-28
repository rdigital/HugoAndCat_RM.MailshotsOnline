using RM.MailshotsOnline.PCL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RM.MailshotsOnline.PCL.Models;
using RM.MailshotsOnline.Data.DAL;
using HC.RM.Common.PCL.Helpers;
using HC.RM.Common.Azure;
using RM.MailshotsOnline.Entities.DataModels;

namespace RM.MailshotsOnline.Data.Services
{
    public class InvoiceService : IInvoiceService
    {
        private StorageContext _context;
        private PricingService _pricingService;
        private ILogger _logger;

        public InvoiceService() 
            : this(new StorageContext())
        { }

        public InvoiceService(StorageContext storageContext)
        {
            _context = storageContext;
            _pricingService = new PricingService();
            _logger = new Logger();
        }

        /// <summary>
        /// Creates a new invoice for a given campaign
        /// </summary>
        /// <param name="campaign">The campaign</param>
        /// <returns>Invoice object</returns>
        public IInvoice CreateInvoiceForCampaign(ICampaign campaign)
        {
            List<PCL.Enums.InvoiceStatus> acceptableStatuses = new List<PCL.Enums.InvoiceStatus>()
            {
                PCL.Enums.InvoiceStatus.Cancelled,
                PCL.Enums.InvoiceStatus.Refunded
            };

            var existingInvoice = _context.Invoices.FirstOrDefault(i => i.CampaignId == campaign.CampaignId && !acceptableStatuses.Contains(i.Status));
            if (existingInvoice != null)
            {
                throw new Exception("Unable to generate new invoice for this campaign - there are outstanding or paid invoices already.");
            }

            if (campaign.Status != PCL.Enums.CampaignStatus.ReadyToCheckout)
            {
                throw new ArgumentException("The campaign is not ready for checkout - a new invoice cannot be created.", "campaign");
            }

            var priceBreakdown = _pricingService.GetPriceBreakdown(campaign);
            if (!priceBreakdown.Complete)
            {
                throw new ArgumentException("The campaign pricing is not complete - a new invoice cannot be created.", "campaign");
            }

            var invoice = new Invoice();
            invoice.CampaignId = campaign.CampaignId;
            invoice.UpdatedDate = DateTime.UtcNow;
            invoice.DataRentalCount = priceBreakdown.DataRentalCount.HasValue ? priceBreakdown.DataRentalCount.Value : 0;
            invoice.DataRentalFlatFee = priceBreakdown.DataRentalFlatFee;
            invoice.DataRentalRate = priceBreakdown.DataRentalRate;
            invoice.PostageRate = priceBreakdown.PostageRate.HasValue ? priceBreakdown.PostageRate.Value : 0;
            invoice.PrintCount = priceBreakdown.PrintCount.HasValue ? priceBreakdown.PrintCount.Value : 0;
            invoice.PrintingRate = priceBreakdown.PrintingRate.HasValue ? priceBreakdown.PrintingRate.Value : 0;
            invoice.ServiceFee = priceBreakdown.ServiceFee;
            invoice.Status = PCL.Enums.InvoiceStatus.Draft;
            invoice.TaxRate = priceBreakdown.TaxRate;

            var savedInvoice = Save(invoice);
            return savedInvoice;
        }

        /// <summary>
        /// Gets any existing invoices for a given campaign
        /// </summary>
        /// <param name="campaign">The campaign to check</param>
        /// <returns>Collection of invoices</returns>
        public IEnumerable<IInvoice> GetInvoicesForCampaign(ICampaign campaign)
        {
            return _context.Invoices.Where(i => i.CampaignId == campaign.CampaignId);
        }

        /// <summary>
        /// Saves an Invoice to the database
        /// </summary>
        /// <param name="invoice">Invoice to save</param>
        /// <returns>Saved invoice object</returns>
        public IInvoice Save(IInvoice invoice)
        {
            invoice.UpdatedDate = DateTime.UtcNow;
            if (invoice.InvoiceId == Guid.Empty)
            {
                _context.Invoices.Add((Invoice)invoice);
            }

            _context.SaveChanges();
            return invoice;
        }
    }
}
