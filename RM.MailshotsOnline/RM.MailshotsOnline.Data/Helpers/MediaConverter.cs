using System;
using System.Collections.Generic;
using System.Linq;
using RM.MailshotsOnline.Entities.JsonModels;
using Umbraco.Core.Models;
using Umbraco.Web;
using IMedia = RM.MailshotsOnline.PCL.Models.IMedia;

namespace RM.MailshotsOnline.Data.Helpers
{
    public static class MediaConverter
    {
        private static readonly Dictionary<Type, Func<IPublishedContent, IMedia>> Conversions =
            new Dictionary<Type, Func<IPublishedContent, IMedia>>();

        static MediaConverter()
        {
            Conversions.Add(typeof (PrivateLibraryImage), PrivateLibraryImage);
            Conversions.Add(typeof (PublicLibraryImage), PublicLibraryImage);
            Conversions.Add(typeof (Image), Image);
        }   

        public static IMedia Convert(IPublishedContent content, Type type)
        {
            var action = Conversions.First(x => x.Key == type);
            var newmedia = action.Value(content);

            return newmedia;
        }

        private static IMedia PrivateLibraryImage(IPublishedContent content)
        {
            var image = Image(content);



            return new PrivateLibraryImage()
            {
                

                Username = content.GetPropertyValue("username")?.ToString()
            };
        }

        private static IMedia PublicLibraryImage(IPublishedContent content)
        {
            return new PublicLibraryImage()
            {
                Src = content.GetPropertyValue("umbracoFile")?.ToString(),
                Width = content.GetPropertyValue("umbracoWidth")?.ToString(),
                Height = content.GetPropertyValue("umbracoHeight")?.ToString(),
                Size = content.GetPropertyValue("umbracoBytes")?.ToString(),
                Type = content.GetPropertyValue("umbracoExtension")?.ToString(),
                Name = content.GetPropertyValue("umbracoName")?.ToString(),
                Tags = content.GetPropertyValue("tags")?.ToString().Split(',')
            };
        }

        private static IMedia Image(IPublishedContent content)
        {
            return new PrivateLibraryImage()
            {
                Username = content.GetPropertyValue("username")?.ToString()
            };
        }

        private static IMedia PrivateLibaryImage(IPublishedContent content)
        {
            throw new NotImplementedException();
        }
    }
}