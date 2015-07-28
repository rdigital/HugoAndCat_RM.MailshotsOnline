using RM.MailshotsOnline.Entities.DataModels;
using RM.MailshotsOnline.Entities.ViewModels;
using RM.MailshotsOnline.PCL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.MailshotsOnline.Entities.Extensions
{
    public static class MailshotExtensions
    {
        public static MailshotViewModel ToViewModel(this IMailshot mailshot)
        {
            var viewModel = new MailshotViewModel()
            {
                MailshotId = mailshot.MailshotId,
                Content = mailshot.Content != null ? mailshot.Content.Content : string.Empty,
                Name = mailshot.Name,
                Draft = mailshot.Draft
            };

            return viewModel;
        }
    }
}
