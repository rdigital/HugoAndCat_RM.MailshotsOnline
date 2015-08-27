using RM.MailshotsOnline.PCL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.MailshotsOnline.Entities.JsonModels
{
    public class CampaignPriceBreakdown : PCL.Models.ICampaignPriceBreakdown
    {
        public bool Complete
        {
            get
            {
                return PostageCost.HasValue 
                    && PrintingCost.HasValue
                    && PrintCount.HasValue;
            }
        }

        public decimal TaxRate { get; set; }

        public decimal? PostageRate { get; set; }

        public decimal? PostageCost
        {
            get
            {
                if (PostageRate.HasValue && PrintCount.HasValue)
                {
                    return PostageRate.Value * PrintCount.Value;
                }

                return null;
            }
        }

        public decimal ServiceFee { get; set; }

        public decimal? PrintingRate { get; set; }

        public decimal? PrintingCost
        {
            get
            {
                if (PrintingRate.HasValue && PrintCount.HasValue)
                {
                    return PrintingRate.Value * PrintCount.Value;
                }

                return null;
            }
        }

        public decimal Total
        {
            get
            {
                return SubTotal + TotalTax;
            }
        }

        public int? PrintCount { get; set; }

        public int? DataRentalCount { get; set; }

        public decimal DataRentalRate { get; set; }

        public decimal DataRentalFlatFee { get; set; }

        public decimal TotalTax
        {
            get
            {
                return SubTotal * TaxRate;
            }
        }

        public decimal DataRentalCost
        {
            get
            {
                if (DataRentalCount.HasValue)
                {
                    return DataRentalRate * DataRentalCount.Value;
                }

                return 0;
            }
        }

        public decimal SubTotal
        {
            get
            {
                var subTotal = ServiceFee;

                if (DataRentalCount.HasValue)
                {
                    subTotal += DataRentalCost;
                    subTotal += DataRentalFlatFee;
                }

                if (PostageCost.HasValue)
                {
                    subTotal += PostageCost.Value;
                }

                if (PrintingCost.HasValue)
                {
                    subTotal += PrintingCost.Value;
                }

                return subTotal;
            }
        }
    }
}
