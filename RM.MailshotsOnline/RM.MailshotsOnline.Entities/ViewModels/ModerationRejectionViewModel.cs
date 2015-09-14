using RM.MailshotsOnline.Entities.PageModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.MailshotsOnline.Entities.ViewModels
{
    public class ModerationRejectionViewModel
    {
        public ModerationPage PageModel { get; set; }

        [Required]
        public string FeedbackMessage { get; set; }

        public Guid ModerationId { get; set; }
    }
}
