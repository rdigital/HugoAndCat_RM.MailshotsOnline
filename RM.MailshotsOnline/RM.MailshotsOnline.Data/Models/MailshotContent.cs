using RM.MailshotsOnline.PCL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.MailshotsOnline.Data.Models
{
    [Table("MailshotContents")]
    public class MailshotContent : IMailshotContent
    {
        [Key]
        public Guid MailshotContentId { get; set; }

        [Column(TypeName = "text")]
        public string Content { get; set; }
    }
}
