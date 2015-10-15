using RM.MailshotsOnline.PCL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.MailshotsOnline.Entities.JsonModels
{
    public class CampaignSummaryModel
    {
        public CampaignSummaryModel(ICampaign campaignModel)
        {
            this.CampaignId = campaignModel.CampaignId;
            this.Name = campaignModel.Name;
            this.UpdatedDate = campaignModel.UpdatedDate;
            this.CreatedDate = campaignModel.CreatedDate;
            if (campaignModel.Mailshot != null && campaignModel.Mailshot.Format != null)
            {
                this.Format = campaignModel.Mailshot.Format.Name;
            }

            this.OwnDataRecipientCount = campaignModel.OwnDataRecipientCount;
            this.RentedDataRecipientCount = campaignModel.RentedDataRecipientCount;
            this.TotalRecipientCount = campaignModel.TotalRecipientCount;
            this.PreviewImageUrl = string.Empty;
        }

        public Guid CampaignId { get; set; }

        public string Name { get; set; }

        public string CampaignHubUrl { get; set; }

        public string StatusTitle { get; set; }

        public string StatusSubtitle { get; set; }

        public DateTime UpdatedDate { get; set; }

        public DateTime CreatedDate { get; set; }

        public string Format { get; set; }

        public int OwnDataRecipientCount { get; set; }

        public int RentedDataRecipientCount { get; set; }

        public int TotalRecipientCount { get; set; }

        public string PreviewImageUrl { get; set; }
    }
}
