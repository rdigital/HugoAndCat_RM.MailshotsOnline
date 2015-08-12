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
    public static class MediaConverter
    {
        private static readonly Dictionary<Type, Func<IPublishedContent, object, IMedia>> ConversionMethods =
            new Dictionary<Type, Func<IPublishedContent, object, IMedia>>();


        static MediaConverter()
        {
            ConversionMethods.Add(typeof(Image), Image);
            ConversionMethods.Add(typeof(PrivateLibraryImage), PrivateLibraryImage);
            ConversionMethods.Add(typeof(PublicLibraryImage), PublicLibraryImage);
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
            var conversionMethods = GetConversionMethods(type, new Stack<Func<IPublishedContent, object, IMedia>>());

            // this is awesome:
            // return conversionMethods?.Aggregate(newMedia, (current, f) => f(content, current));

            if (conversionMethods == null)
            {
                return null;
            }

            foreach (var f in conversionMethods)
            {
                newMedia = f(content, newMedia);
            }

            return newMedia;
        }

        /// <summary>
        /// Recursively get the registered conversion methods for the given type.
        /// </summary>
        /// <param name="t">The type to use for searching registered conversions methods</param>
        /// <param name="funcs">The current stack of conversion methods that applies to this type</param>
        /// <returns></returns>
        private static Stack<Func<IPublishedContent, object, IMedia>> GetConversionMethods(Type t, Stack<Func<IPublishedContent, object, IMedia>> funcs)
        {
            if (ConversionMethods.ContainsKey(t))
            {
                funcs.Push(ConversionMethods.First(x => x.Key == t).Value);
            }

            if (t.BaseType == null)
            {
                return null;
            }

            GetConversionMethods(t.BaseType, funcs);

            return funcs;
        }

        private static IMedia Image(IPublishedContent content, object current)
        {
            var media = (Image) current;
            media.Src = content.GetPropertyValue("umbracoFile")?.ToString();
            media.Width = content.GetPropertyValue("umbracoWidth")?.ToString();
            media.Height = content.GetPropertyValue("umbracoHeight")?.ToString();
            media.Size = content.GetPropertyValue("umbracoBytes")?.ToString();
            media.Type = content.GetPropertyValue("umbracoExtension")?.ToString();
            media.Name = content.GetPropertyValue("umbracoName")?.ToString();

            return media;
        }

        private static IMedia PrivateLibraryImage(IPublishedContent content, object current)
        {
            var media = (PrivateLibraryImage) current;
            media.Username = content.GetPropertyValue("username")?.ToString();

            return media;
        }

        private static IMedia PublicLibraryImage(IPublishedContent content, object current)
        {
            var media = (PublicLibraryImage) current;
            media.Tags = content.GetPropertyValue("tags")?.ToString().Split(',');

            return media;
        }

        public static void RegisterConversionMethod(Action<IPublishedContent, IMedia> action)
        {
            
        }
    }

    //public interface IConverter
    //{
    //    Action<IPublishedContent, IMedia> Convert(IPublishedContent content, IMedia media);
    //}

    //public abstract class ImageConverter<T> : IConverter where T : IMedia
    //{
    //    protected abstract T Media { get; set; }

    //    public abstract Action<IPublishedContent, IMedia> Convert(IPublishedContent content, IMedia media);
    //}

    //public class ImageConverter : ImageConverter<Image>
    //{
    //    protected override Image Media { get; set; }

    //    public override IMedia> Convert(IPublishedContent content, IMedia media)
    //    {
    //        Media.Src = content.GetPropertyValue("umbracoFile")?.ToString();
    //        Media.Width = content.GetPropertyValue("umbracoWidth")?.ToString();
    //        Media.Height = content.GetPropertyValue("umbracoHeight")?.ToString();
    //        Media.Size = content.GetPropertyValue("umbracoBytes")?.ToString();
    //        Media.Type = content.GetPropertyValue("umbracoExtension")?.ToString();
    //        Media.Name = content.GetPropertyValue("umbracoName")?.ToString();

    //        return Media;
    //    }
    //}

    //public class PrivateLibraryImageConverter : ImageConverter<PrivateLibraryImage>
    //{
    //    public override Action<IPublishedContent, IMedia> Convert(IPublishedContent content, IMedia media)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}