using HC.RM.Common.Azure;
using HC.RM.Common.PCL.Helpers;
using RM.MailshotsOnline.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Umbraco.Web.Models.ContentEditing;
using Umbraco.Web.UI;

namespace RM.MailshotsOnline.Web.App_Start
{
    public class UmbracoMessageHandler : System.Net.Http.DelegatingHandler
    {
        private ILogger log = new Logger();

        // Taken from the following:
        // https://our.umbraco.org/forum/umbraco-7/developing-umbraco-7-packages/53699-User-Message-former-Speech-bubble-in-custom-event#comment-197032
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.RequestUri.AbsolutePath.ToLower() == "/umbraco/backoffice/umbracoapi/media/deletebyid")
            {
                return base.SendAsync(request, cancellationToken)
                    .ContinueWith(task =>
                    {
                        var response = task.Result;
                        try
                        {
                            var data = response.Content;
                            
                            var content = ((ObjectContent)(data)).Value as ContentItemDisplay;

                            content.Notifications.Add(new Umbraco.Web.Models.ContentEditing.Notification()
                            {
                                Header = "Test",
                                Message = "TEST The image is currently referenced in a Mailshot.  It cannot be deleted if it is still being used.",
                                NotificationType = SpeechBubbleIcon.Error
                            });

                            //perform any checking (if needed) to ensure you have the right request
                            //for us the cancellation of publish  was only on one content type so we could narrow that down here 
                            //if (content.ContentTypeAlias.Equals(ConfigHelper.PublicLibraryImageContentTypeAlias) || content.ContentTypeAlias.Equals(ConfigHelper.PrivateImageContentTypeAlias))
                            //{
                            //    if (content.Notifications.Count > 0)
                            //    {
                            //        foreach (var notification in content.Notifications)
                            //        {
                            //            if (notification.Header.Equals("Delete") && notification.Message.ToLower().Contains("cancel"))
                            //            {
                            //                //change the default notification to our custom message
                            //                notification.Header = "Unable to delete image";
                            //                notification.Message = "The image is currently referenced in a Mailshot.  It cannot be deleted if it is still being used.";
                            //                notification.NotificationType = SpeechBubbleIcon.Error;
                            //            }
                            //        }
                            //    }

                                
                            //}
                        }
                        catch (Exception ex)
                        {
                            log.Exception(this.GetType().Name, "SendAsync", ex);
                            log.Error(this.GetType().Name, "SendAsync", "Error displaying custom message in Umbraco");
                        }

                        return response;
                    });
            }

            return base.SendAsync(request, cancellationToken);
        }
    }
}
