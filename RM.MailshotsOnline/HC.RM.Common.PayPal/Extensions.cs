using HC.RM.Common.PayPal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PplLinks = PayPal.Api.Links;
using PplTransaction = PayPal.Api.Transaction;
using PplRelatedResources = PayPal.Api.RelatedResources;
using PplItemList = PayPal.Api.ItemList;

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

        internal static IEnumerable<RelatedResource> ToRelatedResources(this List<PplRelatedResources> resources)
        {
            var relatedResources = new List<RelatedResource>();

            foreach (var resource in resources)
            {
                if (resource.order != null)
                {
                    relatedResources.Add(new Order(resource.order));
                }

                if (resource.authorization != null)
                {
                    relatedResources.Add(new Authorization(resource.authorization));
                }
            }
            
            return relatedResources;
        }

        internal static List<PplRelatedResources> ToPplRelatedResources(this IEnumerable<RelatedResource> resources)
        {
            var result = new List<PplRelatedResources>();
            if (resources.Any())
            {
                
                foreach (var resource in resources)
                {
                    var pplResource = new PplRelatedResources();
                    if (resource is Order)
                    {
                        pplResource.order = ((Order)resource).ToPaypalOrder();
                    }
                    else if (resource is Authorization)
                    {
                        pplResource.authorization = ((Authorization)resource).ToPaypalAuthorization();
                    }

                    result.Add(pplResource);
                }
            }

            return result;
        }

        internal static IEnumerable<PurchaseItem> ToPurchaseItems(this PplItemList itemList)
        {
            if (itemList.items == null)
            {
                return null;
            }

            return itemList.items.Select(i => new PurchaseItem(i));
        }

        internal static PplItemList ToPplItemList(this IEnumerable<PurchaseItem> purchaseItems)
        {
            var itemList = new PplItemList();
            itemList.items = purchaseItems.Select(pi => pi.ToPaypalItem()).ToList();

            return itemList;
        }
    }
}
