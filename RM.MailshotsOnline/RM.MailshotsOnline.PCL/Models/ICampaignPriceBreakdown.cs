using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RM.MailshotsOnline.PCL.Models
{
    public interface ICampaignPriceBreakdown
    {
        bool Complete { get; }

        int? PrintCount { get; set; }

        decimal DataRentalFlatFee { get; set; }

        decimal DataRentalCost { get; }

        int? DataRentalCount { get; set; }

        decimal DataRentalRate { get; set; }

        decimal TaxRate { get; set; }

        decimal TotalTax { get; }

        decimal? PostageCost { get; }

        decimal? PostageRate { get; set; }

        decimal ServiceFee { get; set; }

        decimal? PrintingRate { get; set; }

        decimal? PrintingCost { get; }

        decimal SubTotal { get; }

        decimal Total { get; }
    }
}
