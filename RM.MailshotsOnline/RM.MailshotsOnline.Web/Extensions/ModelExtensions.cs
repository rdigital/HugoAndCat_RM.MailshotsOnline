using RM.MailshotsOnline.Data.Helpers;
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

        public static DateTime GetDeliveryEstimate(this ICampaign campaign, ISettingsFromCms settings)
        {
            int deliveryDays = 2;
            if (campaign.PostalOption != null)
            {
                deliveryDays = campaign.PostalOption.DeliveryTime;
            }

            return campaign.GetDespatchEstimate(settings).AddBusinessDays(deliveryDays, settings.PublicHolidays.ToDateList("\n"));
        }

        public static DateTime GetDespatchEstimate(this ICampaign campaign, ISettingsFromCms settings)
        {
            DateTime actionDate = campaign.UpdatedDate;
            int extraDays = 0;
            if (campaign.Status == PCL.Enums.CampaignStatus.Fulfilled)
            {
                if (campaign.OrderDespatchedDate.HasValue)
                {
                    actionDate = campaign.OrderDespatchedDate.Value;
                }
            }
            else if (campaign.Status == PCL.Enums.CampaignStatus.PendingModeration)
            {
                extraDays = settings.ModerationTimeEstimate + settings.PrintingTimeEstimate;
                if (campaign.OrderPlacedDate.HasValue)
                {
                    actionDate = campaign.OrderPlacedDate.Value;
                }
            }
            else if (campaign.Status == PCL.Enums.CampaignStatus.ReadyForFulfilment 
                || campaign.Status == PCL.Enums.CampaignStatus.SentForFulfilment)
            {
                extraDays = settings.PrintingTimeEstimate;
            }
            else
            {
                extraDays = settings.ModerationTimeEstimate + settings.PrintingTimeEstimate;
                actionDate = DateTime.UtcNow;
            }

            return actionDate.AddBusinessDays(extraDays, settings.PublicHolidays.ToDateList("\n"));
        }

        public static string GetStatusText(this MyOrders myOrdersPage, ICampaign campaign)
        {
            string result = myOrdersPage.PendingModerationStatusText;
            switch (campaign.Status)
            {
                case PCL.Enums.CampaignStatus.Cancelled:
                    result = myOrdersPage.CancelledStatusText;
                    break;
                case PCL.Enums.CampaignStatus.PendingModeration:
                case PCL.Enums.CampaignStatus.Draft:
                case PCL.Enums.CampaignStatus.ReadyToCheckout:
                    result = myOrdersPage.PendingModerationStatusText;
                    break;
                case PCL.Enums.CampaignStatus.ReadyForFulfilment:
                case PCL.Enums.CampaignStatus.SentForFulfilment:
                    result = myOrdersPage.PendingPrintingStatusText;
                    break;
                case PCL.Enums.CampaignStatus.Exception:
                    result = myOrdersPage.FailedChecksStatusText;
                    break;
                case PCL.Enums.CampaignStatus.Fulfilled:
                    result = myOrdersPage.DespatchedStatusText;
                    break;
                case PCL.Enums.CampaignStatus.PaymentFailed:
                    result = myOrdersPage.PaymentFailedStatusText;
                    break;
            }

            return result;
        }

        public static string GetStatusDescription(this MyOrders myOrdersPage, ICampaign campaign)
        {
            string result = string.Empty;
            var deliveryEstimate = campaign.GetDeliveryEstimate(myOrdersPage.Settings).Date;
            var despatchEstimate = campaign.GetDespatchEstimate(myOrdersPage.Settings).Date;
            switch (campaign.Status)
            {
                case PCL.Enums.CampaignStatus.Cancelled:
                    result = string.Format(myOrdersPage.CancelledStatusDescription, campaign.CancelledDate.HasValue ? campaign.CancelledDate.Value.Date.ToString("dd/MM/yy") : campaign.UpdatedDate.Date.ToString("dd/MM/yy"));
                    break;
                case PCL.Enums.CampaignStatus.Fulfilled:
                    if (deliveryEstimate.Date <= DateTime.UtcNow.Date)
                    {
                        result = string.Format(myOrdersPage.DeliveredStatusDescription, deliveryEstimate.ToString("dd/MM/yy"));
                    }
                    else
                    {
                        result = string.Format(myOrdersPage.DespatchedStatusDescription, despatchEstimate.ToString("dd/MM/yy"));
                    }
                    break;
                case PCL.Enums.CampaignStatus.ReadyForFulfilment:
                case PCL.Enums.CampaignStatus.SentForFulfilment:
                    result = string.Format(myOrdersPage.PendingPrintingStatusDescription, deliveryEstimate.ToString("dd/MM/yy"));
                    break;
                case PCL.Enums.CampaignStatus.ReadyToCheckout:
                case PCL.Enums.CampaignStatus.Draft:
                case PCL.Enums.CampaignStatus.PendingModeration:
                    result = string.Format(myOrdersPage.PendingModerationStatusDescription, deliveryEstimate.ToString("dd/MM/yy"));
                    break;
                case PCL.Enums.CampaignStatus.Exception:
                    result = myOrdersPage.FailedChecksStatusDescription;
                    break;
                case PCL.Enums.CampaignStatus.PaymentFailed:
                    result = myOrdersPage.PaymentFailedStatusDescription;
                    break;
            }

            return result;
        }

        public static string GetStatusText(this CampaignHub campaignHubPage, ICampaign campaign, ISettingsFromCms settings)
        {
            string result = campaignHubPage.DraftStatusTitle;
            var deliveryEstimate = campaign.GetDeliveryEstimate(settings).Date;
            switch (campaign.Status)
            {
                case PCL.Enums.CampaignStatus.Cancelled:
                    result = campaignHubPage.CancelledStatusTitle;
                    break;
                case PCL.Enums.CampaignStatus.Draft:
                case PCL.Enums.CampaignStatus.ReadyToCheckout:
                    result = campaignHubPage.DraftStatusTitle;
                    break;
                case PCL.Enums.CampaignStatus.PendingModeration:
                case PCL.Enums.CampaignStatus.ReadyForFulfilment:
                case PCL.Enums.CampaignStatus.SentForFulfilment:
                    result = campaignHubPage.ProcessingStatusTitle;
                    break;
                case PCL.Enums.CampaignStatus.Exception:
                    result = campaignHubPage.FailedChecksStatusTitle;
                    break;
                case PCL.Enums.CampaignStatus.Fulfilled:
                    if (deliveryEstimate.Date <= DateTime.UtcNow.Date)
                    {
                        result = campaignHubPage.DeliveredStatusTitle;
                    }
                    else
                    {
                        result = campaignHubPage.DespatchedStatusTitle;
                    }
                    break;
                case PCL.Enums.CampaignStatus.PaymentFailed:
                    result = campaignHubPage.PaymentDeclinedStatusTitle;
                    break;
            }

            return result;
        }

        public static string GetStatusDescription(this CampaignHub campaignHubPage, ICampaign campaign, ISettingsFromCms settings)
        {
            string result = string.Empty;
            var deliveryEstimate = campaign.GetDeliveryEstimate(settings).Date;
            var despatchEstimate = campaign.GetDespatchEstimate(settings).Date;
            switch (campaign.Status)
            {
                case PCL.Enums.CampaignStatus.Draft:
                case PCL.Enums.CampaignStatus.ReadyToCheckout:
                    string daysAgoText = "today";
                    TimeSpan campaignAge = DateTime.UtcNow - campaign.CreatedDate;
                    int daysOld = (int)campaignAge.TotalDays;
                    if (daysOld > 0)
                    {
                        if (daysOld > 1)
                        {
                            daysAgoText = string.Format("{0} days ago", daysOld);
                        }
                        else
                        {
                            daysAgoText = string.Format("{0} day ago", daysOld);
                        }
                    }
                    result = string.Format(campaignHubPage.DraftStatusDescription, daysAgoText, campaign.CreatedDate.ToString("dd/MM/yyyy"));
                    break;
                case PCL.Enums.CampaignStatus.Fulfilled:
                    if (deliveryEstimate.Date <= DateTime.UtcNow.Date)
                    {
                        result = string.Format(campaignHubPage.DeliveredStatusDescription, deliveryEstimate.ToString("dd/MM/yy"));
                    }
                    else
                    {
                        result = string.Format(campaignHubPage.DespatchedStatusDescription, despatchEstimate.ToString("dd/MM/yy"));
                    }
                    break;
                case PCL.Enums.CampaignStatus.ReadyForFulfilment:
                case PCL.Enums.CampaignStatus.SentForFulfilment:
                case PCL.Enums.CampaignStatus.PendingModeration:
                    result = string.Format(campaignHubPage.ProcessingStatusDescription, deliveryEstimate.ToString("dd/MM/yy"));
                    break;
                case PCL.Enums.CampaignStatus.Cancelled:
                case PCL.Enums.CampaignStatus.Exception:
                case PCL.Enums.CampaignStatus.PaymentFailed:
                case PCL.Enums.CampaignStatus.Refunded:
                    result = string.Format(
                        "{0} <a href=\"{1}?campaignId={3}\">{2}</a>", 
                        campaignHubPage.MoreInformationStatusPrompt, 
                        campaignHubPage.OrderDetailsPage.Url(), 
                        campaignHubPage.OrderDetailsPageLinkText, 
                        campaign.CampaignId);
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
