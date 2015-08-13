using System.Collections.Generic;
using RM.MailshotsOnline.PCL.Models;

namespace RM.MailshotsOnline.PCL.Services
{
    public interface IImageLibraryService
    {
        IEnumerable<string> GetTags();

        IEnumerable<IMedia> GetImages();

        IEnumerable<IMedia> GetImages(string tag);

        IEnumerable<IMedia> GetImages(IMember member);

        bool AddImage(IMedia image, IMember member);

        bool DeleteImage(IMedia image);

        bool RenameImage(IMedia image, string name);
    }
}