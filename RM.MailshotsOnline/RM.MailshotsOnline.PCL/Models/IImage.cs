using System;

namespace RM.MailshotsOnline.PCL.Models
{
    public interface IImage
    {
        string Src { get; set; }

        string Width { get; set; }

        string Height { get; set; }

        string Size { get; set; }

        string Type { get; set; }
    }
}