using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using RM.MailshotsOnline.Data.Constants;
using RM.MailshotsOnline.Data.Extensions;
using RM.MailshotsOnline.Entities.JsonModels;
using Umbraco.Core.Models;
using Umbraco.Web;
using IMedia = RM.MailshotsOnline.PCL.Models.IMedia;

namespace RM.MailshotsOnline.Data.Helpers
{
    public static class MediaFactory
    {
        private static readonly Dictionary<Type, IMediaConverter> MediaConverters =
            new Dictionary<Type, IMediaConverter>();


        static MediaFactory()
        {
            MediaConverters.Add(typeof(Image), new ImageMediaConverter());
            MediaConverters.Add(typeof(PublicLibraryImage), new PublicLibraryImageMediaConverter());
            MediaConverters.Add(typeof(PrivateLibraryImage), new PrivateLibraryImageMediaConverter());
        }

        /// <summary>
        /// Converts a media item into the given type if a conversion method has been registered.
        /// 
        /// Conversion methods are held locally and registered in the constructor for this class.
        /// 
        /// Conversions for the given type and all its base classes are looked up recursively and
        /// pushed into a stack, derived-type first (most-base-type last). Conversions are popped off
        /// and applied to our media one by one (i.e. most-base first, derived last).
        /// </summary>
        /// <param name="content">The content item to convert</param>
        /// <param name="type">The type to convert the item into</param>
        /// <returns></returns>
        public static IMedia Convert(IPublishedContent content, Type type)
        {
            // Create a new instance of our type
            var newMedia = (IMedia) Activator.CreateInstance(type);
            var mediaConverters = GetConversionMethods(type, new Stack<IMediaConverter>());

            // this is awesome:
            // return MediaConverters?.Aggregate(newMedia, (current, f) => f.Convert(content, current));

            if (mediaConverters == null)
            {
                return null;
            }

            foreach (var f in mediaConverters)
            {
                newMedia = f.Convert(content, newMedia);
            }

            return newMedia;
        }

        /// <summary>
        /// Recursively get the registered conversion methods for the given type.
        /// </summary>
        /// <param name="t">The type to use for searching registered conversions methods</param>
        /// <param name="mediaConverters">The current stack of conversion methods that applies to this type</param>
        /// <returns></returns>
        private static Stack<IMediaConverter> GetConversionMethods(Type t, Stack<IMediaConverter> mediaConverters)
        {
            if (MediaConverters.ContainsKey(t))
            {
                mediaConverters.Push(MediaConverters.First(x => x.Key == t).Value);
            }

            if (t.BaseType == null)
            {
                return null;
            }

            GetConversionMethods(t.BaseType, mediaConverters);

            return mediaConverters;
        }
    }

    public interface IMediaConverter
    {
        IMedia Convert(IPublishedContent content, object o);
    }

    public abstract class ImageMediaConverter<T> : IMediaConverter where T : IMedia
    {
        public abstract T Media { get; set; }

        public abstract IMedia Convert(IPublishedContent content, object o);
    }

    public class ImageMediaConverter : ImageMediaConverter<Image>
    {
        public override Image Media { get; set; }

        public override IMedia Convert(IPublishedContent content, object o)
        {
            Media = (Image) o;
            Media.Src = content.GetPropertyValue("umbracoFile")?.ToString();
            Media.Width = content.GetPropertyValue("umbracoWidth")?.ToString();
            Media.Height = content.GetPropertyValue("umbracoHeight")?.ToString();
            Media.Size = content.GetPropertyValue("umbracoBytes")?.ToString();
            Media.Type = content.GetPropertyValue("umbracoExtension")?.ToString();
            Media.Name = content.GetPropertyValue("umbracoName")?.ToString();

            return Media;
        }
    }

    public class PrivateLibraryImageMediaConverter : ImageMediaConverter<PrivateLibraryImage>
    {
        public override PrivateLibraryImage Media { get; set; }

        public override IMedia Convert(IPublishedContent content, object o)
        {
            Media = (PrivateLibraryImage) o;
            Media.Username = content.GetPropertyValue("username")?.ToString();

            return Media;
        }
    }

    public class PublicLibraryImageMediaConverter : ImageMediaConverter<PublicLibraryImage>
    {
        public override PublicLibraryImage Media { get; set; }

        public override IMedia Convert(IPublishedContent content, object o)
        {
            Media = (PublicLibraryImage) o;
            Media.Tags = content.GetPropertyValue("tags")?.ToString().Split(',');

            return Media;
        }
    }
}