using Newtonsoft.Json;
using RM.MailshotsOnline.Entities.DataModels.MailshotSettings;
using RM.MailshotsOnline.PCL;
using RM.MailshotsOnline.PCL.Models;
using RM.MailshotsOnline.PCL.Models.MailshotSettings;
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
    /// Model representing Mailshot data
    /// </summary>
    public class Mailshot : IMailshot
    {
        /// <summary>
        /// The concrete Mailshot Content object
        /// </summary>
        private MailshotContent _content;

        /// <summary>
        /// The concrete Format object
        /// </summary>
        private Format _format;

        /// <summary>
        /// The concrete Template object
        /// </summary>
        private Template _template;

        /// <summary>
        /// The concrete Theme object
        /// </summary>
        private Theme _theme;

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

        [JsonIgnore]
        [Required]
        public Guid FormatId { get; set; }

        [JsonIgnore]
        public virtual Format Format
        {
            get { return _format; }
            set { _format = value; }
        }

        [JsonIgnore]
        [Required]
        public Guid TemplateId { get; set; }

        [JsonIgnore]
        public virtual Template Template
        {
            get { return _template; }
            set { _template = value; }
        }

        [JsonIgnore]
        [Required]
        public Guid ThemeId { get; set; }

        [JsonIgnore]
        public virtual Theme Theme
        {
            get { return _theme; }
            set { _theme = value; }
        }

        [JsonIgnore]
        public string ProofPdfBlobId { get; set; }

        public string ProofPdfUrl { get; set; }

        public Guid ProofPdfOrderNumber { get; set; }

        public Enums.PdfRenderStatus ProofPdfStatus { get; set; }

        #region Explicit Interface Implementations

        [JsonIgnore]
        IMailshotContent IMailshot.Content
        {
            get { return _content; }
            set { _content = (MailshotContent)value; }
        }

        [JsonIgnore]
        IFormat IMailshot.Format
        {
            get { return _format; }
            set { _format = (Format)value; }
        }

        [JsonIgnore]
        ITemplate IMailshot.Template
        {
            get { return _template; }
            set { _template = (Template)value; }
        }

        [JsonIgnore]
        ITheme IMailshot.Theme
        {
            get { return _theme; }
            set { _theme = (Theme)value; }
        }

        #endregion
    }
}
