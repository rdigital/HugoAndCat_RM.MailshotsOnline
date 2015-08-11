using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RM.MailshotsOnline.PCL.Models
{
    public interface ITaggable
    {
        IEnumerable<string> Tags { get; set; }
    }
}
