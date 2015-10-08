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
        /// <param name="member">The member who owns the campaign</param>
        /// <returns>Invoice object</returns>
        public IInvoice CreateInvoiceForCampaign(ICampaign campaign, IMember member)
        {
            var existingDraftInvoice = _context.Invoices.FirstOrDefault(i => i.CampaignId == campaign.CampaignId && i.Status == PCL.Enums.InvoiceStatus.Draft);
            List<PCL.Enums.InvoiceStatus> acceptableStatuses = new List<PCL.Enums.InvoiceStatus>()
            {
                PCL.Enums.InvoiceStatus.Cancelled,
                PCL.Enums.InvoiceStatus.Refunded,
                PCL.Enums.InvoiceStatus.Draft
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

            Invoice invoice;
            if (existingDraftInvoice != null)
            {
                var existingLineItems = _context.InvoiceLineItems.Where(li => li.InvoiceId == existingDraftInvoice.InvoiceId);
                foreach (var lineItem in existingLineItems)
                {
                    _context.InvoiceLineItems.Remove(lineItem);
                }

                existingDraftInvoice.LineItems = new List<InvoiceLineItem>();
                _context.SaveChanges();
                invoice = existingDraftInvoice;
            }
            else
            {
                invoice = new Invoice();
            }

            // Create order number
            var invoiceCount = _context.Invoices.Count(i => i.Campaign.UserId == campaign.UserId);
            var orderNumber = string.Format("{0}{1}{2:D5}", DateTime.UtcNow.Year, campaign.UserId, invoiceCount + 1);
            invoice.OrderNumber = orderNumber;
            invoice.CampaignId = campaign.CampaignId;
            invoice.UpdatedDate = DateTime.UtcNow;
            invoice.Status = PCL.Enums.InvoiceStatus.Draft;
            invoice.BillingAddress = new Address()
            {
                Title = member.Title,
                FirstName = member.FirstName,
                LastName = member.LastName,
                FlatNumber = member.FlatNumber,
                BuildingNumber = member.BuildingNumber,
                BuildingName = member.BuildingName,
                Address1 = member.Address1,
                Address2 = member.Address2,
                City = member.City,
                Country = member.Country,
                Postcode = member.Postcode
            };

            var savedInvoice = Save(invoice);

            if (savedInvoice != null)
            {
                // TODO: Get the names from the CMS
                var lineItems = new List<InvoiceLineItem>();

                lineItems.Add(new InvoiceLineItem()
                {
                    ProductSku = Constants.Constants.Products.MsolServiceFeeSku,
                    Quantity = 1,
                    UnitCost = priceBreakdown.ServiceFee,
                    TaxRate = priceBreakdown.TaxRate,
                    InvoiceId = savedInvoice.InvoiceId
                });

                lineItems.Add(new InvoiceLineItem()
                {
                    ProductSku = Constants.Constants.Products.YourDataSku,
                    Quantity = campaign.OwnDataRecipientCount,
                    UnitCost = 0,
                    TaxRate = 0,
                    InvoiceId = savedInvoice.InvoiceId
                });

                if (priceBreakdown.DataRentalCount.HasValue && priceBreakdown.DataRentalCount.Value > 0)
                {
                    // Add data costs
                    lineItems.Add(new InvoiceLineItem()
                    {
                        ProductSku = Constants.Constants.Products.OurDataSku,
                        Quantity = priceBreakdown.DataRentalCount.Value,
                        UnitCost = priceBreakdown.DataRentalRate,
                        TaxRate = priceBreakdown.TaxRate,
                        InvoiceId = savedInvoice.InvoiceId
                    });

                    lineItems.Add(new InvoiceLineItem()
                    {
                        ProductSku = Constants.Constants.Products.DataSearchFeeSku,
                        Quantity = 1,
                        UnitCost = priceBreakdown.DataRentalFlatFee,
                        TaxRate = priceBreakdown.TaxRate,
                        InvoiceId = savedInvoice.InvoiceId
                    });
                }

                if (priceBreakdown.PrintCount.HasValue && priceBreakdown.PrintCount.Value > 0)
                {
                    if (priceBreakdown.PrintingRate.HasValue)
                    {
                        // Add printing costs
                        lineItems.Add(new InvoiceLineItem()
                        {
                            ProductSku = Constants.Constants.Products.PrintSku,
                            Quantity = priceBreakdown.PrintCount.Value,
                            UnitCost = priceBreakdown.PrintingRate.Value,
                            TaxRate = priceBreakdown.TaxRate,
                            InvoiceId = savedInvoice.InvoiceId
                        });
                    }

                    if (priceBreakdown.PostageRate.HasValue)
                    {
                        // Add postage
                        lineItems.Add(new InvoiceLineItem()
                        {
                            ProductSku = Constants.Constants.Products.PostSku,
                            SubTitle = campaign.PostalOption.Name,
                            Quantity = priceBreakdown.PrintCount.Value,
                            UnitCost = priceBreakdown.PostageRate.Value,
                            TaxRate = priceBreakdown.TaxRate,
                            InvoiceId = savedInvoice.InvoiceId
                        });
                    }
                }

                savedInvoice.LineItems = lineItems.ToList<IInvoiceLineItem>();//.Cast<IInvoiceLineItem>();
                savedInvoice = Save(savedInvoice);
            }

            return savedInvoice;
        }

        /// <summary>
        /// Gets an invoice
        /// </summary>
        /// <param name="invoiceId">The ID of the invoice</param>
        /// <returns>The specified invoice</returns>
        public IInvoice GetInvoice(Guid invoiceId)
        {
            return _context.Invoices
                .Include("LineItems")
                .Include("LineItems.Product")
                .Include("Campaign")
                .FirstOrDefault(i => i.InvoiceId == invoiceId);
        }

        /// <summary>
        /// Gets any existing invoices for a given campaign
        /// </summary>
        /// <param name="campaign">The campaign to check</param>
        /// <returns>Collection of invoices</returns>
        public IEnumerable<IInvoice> GetInvoicesForCampaign(ICampaign campaign)
        {
            return _context.Invoices
                .Include("LineItems")
                .Include("LineItems.Product")
                .Include("Campaign")
                .Where(i => i.CampaignId == campaign.CampaignId);
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

            if (invoice.LineItems != null)
            {
                foreach (var lineItem in invoice.LineItems)
                {
                    if (lineItem.InvoiceLineItemId == Guid.Empty)
                    {
                        _context.InvoiceLineItems.Add((InvoiceLineItem)lineItem);
                    }
                }
            }
            _context.SaveChanges();
            return invoice;
        }

        public IEnumerable<IInvoice> GetPaidInvoices(DateTime startDate, DateTime endDate)
        {
            return _context.Invoices
                .Include("LineItems")
                .Include("LineItems.Product")
                .Include("Campaign")
                .Where(i => i.PaidDate.HasValue && i.PaidDate.Value <= endDate && i.PaidDate.Value >= startDate);
        }
    }
}
