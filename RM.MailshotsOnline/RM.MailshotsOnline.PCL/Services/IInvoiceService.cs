using RM.MailshotsOnline.PCL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RM.MailshotsOnline.PCL.Services
{
    public interface IInvoiceService
    {
        /// <summary>
        /// Creates a new invoice for a given campaign
        /// </summary>
        /// <param name="campaign">The campaign</param>
        /// <param name="member">The member who owns the campaign</param>
        /// <returns>Invoice object</returns>
        IInvoice CreateInvoiceForCampaign(ICampaign campaign, IMember member);

        /// <summary>
        /// Gets any existing invoices for a given campaign
        /// </summary>
        /// <param name="campaign">The campaign to check</param>
        /// <returns>Collection of invoices</returns>
        IEnumerable<IInvoice> GetInvoicesForCampaign(ICampaign campaign);

        /// <summary>
        /// Saves an Invoice to the database
        /// </summary>
        /// <param name="invoice">Invoice to save</param>
        /// <returns>Saved invoice object</returns>
        IInvoice Save(IInvoice invoice);
    }
}
