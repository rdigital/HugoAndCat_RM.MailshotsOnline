using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using RM.MailshotsOnline.PCL.Models;

namespace RM.MailshotsOnline.Entities.JsonModels
{
    public class Image :IImage
    {
        public string Src { get; set; }

        public string Width { get; set; }

        public string Height { get; set; }

        public string Size
        {
            get { return (_size / 1024) + "KB"; }
            set { _size = Convert.ToInt32(value); }
        }

        public string Type { get; set; }

        private int _size { get; set; }
    }
}
