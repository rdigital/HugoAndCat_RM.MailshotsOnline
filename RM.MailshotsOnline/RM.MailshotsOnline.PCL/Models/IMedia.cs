using System.Xml;

namespace RM.MailshotsOnline.PCL.Models
{
    public interface IMedia
    {
        /// <summary>
        /// Gets or sets the Umbraco ID of the media item
        /// </summary>
        int MediaId { get; set; }

        /// <summary>
        /// Gets or sets the name of the media item
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets the number of times the media item has been used in a mailshot
        /// </summary>
        int MailshotUses { get; set; }
    }
}