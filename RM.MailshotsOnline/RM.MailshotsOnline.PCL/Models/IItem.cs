using System;
using System.Collections.Generic;

namespace RM.MailshotsOnline.PCL.Models
{
    /// <summary>
    /// Base Umbraco content item
    /// </summary>
    public interface IItem
    {
        int Id { get; set; }

        string DocumentType { get; set; }

        string Name { get; set; }

        string Creator { get; set; }

        string Path { get; set; }

        DateTime CreatedDate { get; set; }

        IEnumerable<IItem> Children { get; set; }
    }
}
