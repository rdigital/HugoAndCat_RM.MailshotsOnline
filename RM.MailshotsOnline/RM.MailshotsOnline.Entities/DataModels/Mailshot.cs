using Newtonsoft.Json;
using RM.MailshotsOnline.PCL;
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
    public class Mailshot : IMailshot
    {
        private MailshotContent _content;

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid MailshotId { get; set; }

        [JsonIgnore]
        public Guid MailshotContentId { get; set; }

        [JsonIgnore]
        [ForeignKey("MailshotContentId")]
        public virtual MailshotContent Content
        {
            get { return _content; }
            set { _content = value; }
        }

        [NotMapped]
        public string ContentText { get; set; }

        [JsonIgnore]
        [Required]
        public int UserId { get; set; }

        [Required]
        public string Name { get; set; }

        public DateTime UpdatedDate { get; set; }

        public DateTime CreatedDate { get { return CreatedUtc; } }

        [JsonIgnore]
        [Required, DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedUtc { get; private set; }

        public bool Draft { get; set; }

        [Required]
        public int FormatId { get; set; }

        [Required]
        public int TemplateId { get; set; }

        [Required]
        public int ThemeId { get; set; }

        [JsonIgnore]
        public string ProofPdfBlobId { get; set; }

        [JsonIgnore]
        public string ProofPdfUrl { get; set; }

        [JsonIgnore]
        public Enums.PdfRenderStatus ProofPdfStatus { get; set; }

        #region Explicit Interface Implementations

        [JsonIgnore]
        IMailshotContent IMailshot.Content
        {
            get { return _content; }
            set { _content = (MailshotContent)value; }
        }

        #endregion
    }
}
