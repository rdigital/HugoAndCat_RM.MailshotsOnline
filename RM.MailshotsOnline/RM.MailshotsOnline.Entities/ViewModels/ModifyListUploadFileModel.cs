using System;
using System.ComponentModel.DataAnnotations;
using System.Web;
using RM.MailshotsOnline.Entities.PageModels;

namespace RM.MailshotsOnline.Entities.ViewModels
{
    public class ModifyListUploadFileModel
    {
        public Guid DistributionListId { get; set; }

        [Required]
        public string ListName { get; set; }

        public ListCreate PageModel { get; set; }

        [Required]
        public HttpPostedFileBase UploadCsv { get; set; }

        public byte[] CsvData { get; set; } 
    }
}
