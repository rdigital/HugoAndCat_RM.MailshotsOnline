using System.Collections.Generic;
using RM.MailshotsOnline.PCL.Models;

namespace RM.MailshotsOnline.PCL.Services
{
    public interface IImageLibraryService
    {
        IEnumerable<string> GetTags();

        IEnumerable<IImage> GetImages();

        IEnumerable<IImage> GetImages(string tag);

        IEnumerable<IMedia> GetImages(IMember member);

        bool AddImage(IImage image, IMember member);

        bool DeleteImage(IImage image);

        bool RenameImage(IImage image, string name);
    }
}