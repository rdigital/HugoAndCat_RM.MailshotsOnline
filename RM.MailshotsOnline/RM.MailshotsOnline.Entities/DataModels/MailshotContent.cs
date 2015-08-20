using RM.MailshotsOnline.PCL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.MailshotsOnline.Entities.DataModels
{
    /// <summary>
    /// Model for storing mailshot JSON content
    /// </summary>
    [Table("MailshotContents")]
    public class MailshotContent : IMailshotContent
    {
        /// <summary>
        /// Gets or sets the ID of the mailshot content
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid MailshotContentId { get; set; }

        /// <summary>
        /// Gets or sets the mailshot JSON content
        /// </summary>
        [Column(TypeName = "text")]
        public string Content { get; set; }
    }
}
