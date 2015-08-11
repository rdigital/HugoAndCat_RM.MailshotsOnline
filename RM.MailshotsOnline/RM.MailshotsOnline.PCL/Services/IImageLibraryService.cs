using System.Collections.Generic;
using RM.MailshotsOnline.PCL.Models;

namespace RM.MailshotsOnline.PCL.Services
{
    public interface IImageLibraryService
    {
        IEnumerable<string> GetTags();

        IEnumerable<IImage> GetImages();

        IEnumerable<IImage> GetImages(string tag);

        IEnumerable<IImage> GetImages(IMember member);

    }
}