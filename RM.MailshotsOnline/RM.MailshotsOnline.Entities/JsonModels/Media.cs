using Newtonsoft.Json;
using RM.MailshotsOnline.PCL.Models;

namespace RM.MailshotsOnline.Entities.JsonModels
{
    public class Media : IMedia
    {
        /// <summary>
        /// Gets or sets the Umbraco ID of the media item
        /// </summary>
        [JsonIgnore]
        public int MediaId { get; set; }

        /// <summary>
        /// Gets or sets the name of the media item
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the number of times the media item has been used in a mailshot
        /// </summary>
        public int MailshotUses { get; set; }
    }
}