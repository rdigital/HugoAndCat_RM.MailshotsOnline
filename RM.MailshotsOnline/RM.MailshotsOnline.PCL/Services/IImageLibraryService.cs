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

        IMedia AddImage(byte[] bytes, string name, IMember member);

        IMedia GetImage(int mediaId, bool publicImage);

        bool DeleteImage(string id);

        bool RenameImage(string name, string newName);
    }
}