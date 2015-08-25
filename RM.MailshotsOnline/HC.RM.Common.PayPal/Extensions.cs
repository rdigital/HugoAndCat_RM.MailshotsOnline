using HC.RM.Common.PayPal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PplLinks = PayPal.Api.Links;
using PplTransaction = PayPal.Api.Transaction;

namespace HC.RM.Common.PayPal
{
    internal static class Extensions
    {
        internal static IEnumerable<HateoasLink> ToHateoasLinks(this List<PplLinks> paypalLinks)
        {
            return paypalLinks.Select(ppl => new HateoasLink(ppl));
        }

        internal static List<PplLinks> ToPplLinks(this IEnumerable<HateoasLink> links)
        {
            return links.Select(link => link.ToPaypalLink()).ToList();
        }

        internal static IEnumerable<Transaction> ToTransactions(this List<PplTransaction> transactions)
        {
            return transactions.Select(ppt => new Transaction(ppt));
        }

        internal static List<PplTransaction> ToPplTransactions(this IEnumerable<Transaction> transactions)
        {
            return transactions.Select(t => t.ToPaypalTransaction()).ToList();
        }
    }
}
