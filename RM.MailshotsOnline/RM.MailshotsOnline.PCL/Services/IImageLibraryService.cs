using System.Collections.Generic;
using RM.MailshotsOnline.PCL.Models;

namespace RM.MailshotsOnline.PCL.Services
{
    public interface IImageLibraryService
    {
        /// <summary>
        /// Get a list of all tags stored in Umbraco.
        /// </summary>
        /// <returns>The list of all tags.</returns>
        IEnumerable<string> GetTags();

        /// <summary>
        /// Get all images in the public library.
        /// </summary>
        /// <returns>The collection of images.</returns>
        IEnumerable<IMedia> GetImages();

        /// <summary>
        /// Gets all images under the given tag in the public library.
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <returns>The collection of images.</returns>
        IEnumerable<IMedia> GetImages(string tag);

        /// <summary>
        /// Get all images belonging to the member.
        /// </summary>
        /// <param name="member">The member.</param>
        /// <returns>The collection of images.</returns>
        IEnumerable<IMedia> GetImages(IMember member);

        /// <summary>
        /// Add an image into the blob store for the given member.
        /// </summary>
        /// <param name="bytes">The bytes representing the image.</param>
        /// <param name="name">The name of the image.</param>
        /// <param name="member">The member creating this image. The image will be stored in this member's personal image store.</param>
        /// <returns></returns>
        IMedia AddImage(byte[] bytes, string name, IMember member);

        /// <summary>
        /// Gets an image
        /// </summary>
        /// <param name="mediaId">The Umbraco media ID of the image to fetch</param>
        /// <param name="publicImage">Indicates if this is a public image</param>
        /// <returns>The Media item</returns>
        IMedia GetImage(int mediaId, bool publicImage);

        /// <summary>
        /// Gets a media item (base class)
        /// </summary>
        /// <param name="mediaId">The Umbraco media ID of the media item to fetch</param>
        /// <returns>The Media item</returns>
        IMedia GetMedia(int mediaId);

        /// <summary>
        /// Finds a private image based on its Blob URL
        /// </summary>
        /// <param name="blobUrl">The Blob URL to search for</param>
        /// <returns>The Media item</returns>
        IMedia GetImageByBlobUrl(string blobUrl);

        /// <summary>
        /// Delete an image 
        /// </summary>
        /// <param name="id">Blob ID</param>
        /// <returns>true on success</returns>
        bool DeleteImage(string id);

        /// <summary>
        /// Deletes a media item
        /// </summary>
        /// <param name="media">The media item to be deleted</param>
        /// <returns>True on success</returns>
        bool DeleteImage(IMedia media);

        /// <summary>
        /// Renames a given image (Not sure if this needs to be used?)
        /// </summary>
        /// <param name="name">Original name of the image</param>
        /// <param name="newName">New name for the image</param>
        /// <returns>True on success</returns>
        bool RenameImage(string name, string newName);
    }
}