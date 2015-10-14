using System;
using System.Collections.Generic;
using System.Linq;
using RM.MailshotsOnline.Data.Media_Conversion.Converters;
using RM.MailshotsOnline.Entities.JsonModels;
using Umbraco.Core.Models;
using Umbraco.Web;
using IMedia = RM.MailshotsOnline.PCL.Models.IMedia;
using Media = RM.MailshotsOnline.Entities.JsonModels.Media;

namespace RM.MailshotsOnline.Data.Media_Conversion
{
    public static class MediaFactory
    {
        private static readonly Dictionary<Type, IMediaConverter> MediaConverters =
            new Dictionary<Type, IMediaConverter>();

        private static readonly UmbracoHelper _helper = new UmbracoHelper(UmbracoContext.Current);

        static MediaFactory()
        {
            MediaConverters.Add(typeof(Media), new MediaMediaConverter());
            MediaConverters.Add(typeof(Image), new ImageMediaConverter());
            MediaConverters.Add(typeof(PublicLibraryImage), new PublicLibraryImageMediaConverter());
            MediaConverters.Add(typeof(PrivateLibraryImage), new PrivateLibraryImageMediaConverter());
        }

        /// <summary>
        /// <para>Converts a media item into the given type if a media converter for that type has 
        /// been registered.</para>
        /// 
        /// <para>Converters are registered in the constructor for this class.</para>
        /// 
        /// <para>Conversions for the given type and all its base classes are looked up and applied to
        /// our media one by one (i.e. most-base first, derived last).</para>
        /// </summary>
        /// <param name="content">The content item to convert.</param>
        /// <param name="type">The type to convert the item into.</param>
        /// <returns>The converted media</returns>
        public static IMedia Convert(IPublishedContent content, Type type)
        {
            // Check if the content was found
            if (content == null)
            {
                return null;
            }

            // Create a new instance of our type
            var newMedia = (IMedia) Activator.CreateInstance(type);
            var mediaConverters = GetMediaConverters(type, new Stack<IMediaConverter>());

            // this is awesome:
            // return mediaConverters?.Aggregate(newMedia, (current, m) => m.Convert(content, current));

            if (mediaConverters == null)
            {
                return null;
            }

            foreach (var m in mediaConverters)
            {
                newMedia = m.Convert(content, newMedia);
            }

            return newMedia;
        }

        public static IMedia Convert(Umbraco.Core.Models.IMedia content, Type type)
        {
            // Create a new instance of our type
            var newMedia = (IMedia) Activator.CreateInstance(type);
            var mediaConverters = GetMediaConverters(type, new Stack<IMediaConverter>());

            // this is awesome:
            // return mediaConverters?.Aggregate(newMedia, (current, m) => m.Convert(content, current));

            if (mediaConverters == null)
            {
                return null;
            }

            foreach (var m in mediaConverters)
            {
                newMedia = m.Convert(content, newMedia);
            }

            return newMedia;
        }

        /// <summary>
        /// <para>Converts a media item into the given type if a media converter for that type has 
        /// been registered.</para>
        /// 
        /// <para>Converters are registered in the constructor for this class.</para>
        /// 
        /// <para>Conversions for the given type and all its base classes are looked up and applied to
        /// our media one by one (i.e. most-base first, most-derived last).</para>
        /// </summary>
        /// <param name="contentId">The ID of the content item to convert.</param>
        /// <param name="type">The type to convert the item into.</param>
        /// <returns>The converted media</returns>
        public static IMedia Convert(int contentId, Type type)
        {
            var content = _helper.TypedContent(contentId);

            return Convert(content, type);
        }

        /// <summary>
        /// Recursively get the registered media converters for the given type.
        /// </summary>
        /// <param name="t">The type.</param>
        /// <param name="mediaConverters">The current stack of conversion methods that applies to this type.</param>
        /// <returns>The complete set of converters that should be used for the given type.</returns>
        private static Stack<IMediaConverter> GetMediaConverters(Type t, Stack<IMediaConverter> mediaConverters)
        {
            if (MediaConverters.ContainsKey(t))
            {
                mediaConverters.Push(MediaConverters.First(x => x.Key == t).Value);
            }

            if (t.BaseType == null)
            {
                return null;
            }

            GetMediaConverters(t.BaseType, mediaConverters);

            return mediaConverters;
        }
    }
}