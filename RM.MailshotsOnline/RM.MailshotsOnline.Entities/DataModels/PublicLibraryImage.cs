using System.Collections.Generic;
using Glass.Mapper.Umb;
using RM.MailshotsOnline.PCL.Models;

namespace RM.MailshotsOnline.Entities.DataModels
{
    public class PublicLibraryImage : ITaggable
    {
        public IEnumerable<string> Tags { get; set; }
    }
}