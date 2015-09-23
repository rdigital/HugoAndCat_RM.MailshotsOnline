﻿using RM.MailshotsOnline.Data.Helpers;
using RM.MailshotsOnline.Entities.PageModels;
using RM.MailshotsOnline.PCL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.MailshotsOnline.Web.Extensions
{
    public static class ModelExtensions
    {
        public static string GetLastEditedSummary(this ICampaign campaign)
        {
            return string.Format("Last edited at {0:dd/MM/yyyy} at {0:hh:mm tt}", campaign.UpdatedDate);
        }

        public static bool HasData(this ICampaign campaign)
        {
            return campaign.HasDataSearches || campaign.HasDistributionLists;
        }

        public static bool NeedsDataAndDesign(this ICampaign campaign)
        {
            return !campaign.HasMailshotSet && !campaign.HasDataSearches && !campaign.HasDistributionLists;
        }

        public static IInvoice LatestInvoice(this ICampaign campaign)
        {
            if (campaign.Invoices != null && campaign.Invoices.Any())
            {
                return campaign.Invoices.OrderByDescending(i => i.UpdatedDate).FirstOrDefault();
            }

            return null;
        }

        public static string GetStatusText(this MyOrders myOrdersPage, ICampaign campaign)
        {
            string result = myOrdersPage.ProcessingStatusText;
            switch (campaign.Status)
            {
                case PCL.Enums.CampaignStatus.Cancelled:
                    result = myOrdersPage.CancelledStatusText;
                    break;
                case PCL.Enums.CampaignStatus.Draft:
                case PCL.Enums.CampaignStatus.ReadyToCheckout:
                case PCL.Enums.CampaignStatus.ReadyForFulfilment:
                    result = myOrdersPage.ProcessingStatusText;
                    break;
                case PCL.Enums.CampaignStatus.SentForFulfilment:
                    result = myOrdersPage.DespatchedStatusText;
                    break;
                case PCL.Enums.CampaignStatus.Exception:
                    result = myOrdersPage.FailedChecksStatusText;
                    break;
            }

            return result;
        }

        public static HC.RM.Common.PayPal.Models.Transaction ToPaypalTransaction(this IInvoiceLineItem lineItem)
        {
            var result = new HC.RM.Common.PayPal.Models.Transaction(
                lineItem.Name,
                ConfigHelper.CurrencyCode,
                lineItem.Total,
                lineItem.SubTotal,
                lineItem.TaxTotal,
                lineItem.InvoiceId.ToString("N")
                );

            return result;
        }
    }
}
