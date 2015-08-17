using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.MailshotsOnline.Entities.ViewModels
{
    public class ImageUploadViewModel
    {
        public string Name { get; set; }

        public string ImageString { get; set; }

        public byte[] ImageData { get; set; }
    }
}
