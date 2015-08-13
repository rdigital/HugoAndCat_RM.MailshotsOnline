using System;
using System.Collections.Generic;
using System.Linq;
using RM.MailshotsOnline.Data.Media_Conversion.Converters;
using RM.MailshotsOnline.Entities.JsonModels;
using Umbraco.Core.Models;
using IMedia = RM.MailshotsOnline.PCL.Models.IMedia;

namespace RM.MailshotsOnline.Data.Media_Conversion
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
        /// <returns>The converted media</returns>
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
        /// <param name="t">The type</param>
        /// <param name="mediaConverters">The current stack of conversion methods that applies to this type</param>
        /// <returns>The complete set of conversions to apply to the media of the given type</returns>
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
}