using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RM.MailshotsOnline.PCL.Models
{
    public interface ITaggable
    {
        /// <summary>
        /// Gets or sets the Tags of the object
        /// </summary>
        IEnumerable<string> Tags { get; set; }
    }
}
