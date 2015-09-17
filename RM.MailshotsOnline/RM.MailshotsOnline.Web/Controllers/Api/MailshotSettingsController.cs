using HC.RM.Common.PCL.Helpers;
using RM.MailshotsOnline.PCL.Models.MailshotSettings;
using RM.MailshotsOnline.PCL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace RM.MailshotsOnline.Web.Controllers.Api
{
    public class MailshotSettingsController : ApiBaseController
    {
        private IMailshotSettingsService _settingsService;

        public MailshotSettingsController(ILogger logger, IMembershipService membershipService, IMailshotSettingsService settingsService)
            : base(membershipService, logger)
        {
            _settingsService = settingsService;
        }

        [HttpGet]
        public HttpResponseMessage GetFormats()
        {
            var formats = _settingsService.GetFormats();
            var arrayString = string.Join(",", formats.Select(f => f.JsonData));
            return JsonResponse($"[{arrayString}]");
        }

        [HttpGet]
        public HttpResponseMessage GetFormat(int id)
        {
            var format = _settingsService.GetFormatByJsonIndex(id);
            if (format == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Format not found");
            }

            return JsonResponse(format.JsonData);
        }

        [HttpGet]
        public HttpResponseMessage GetTemplatesForFormat(int id)
        {
            var templates = _settingsService.GetTemplatesForFormat(id);
            if (templates == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Templates not found");
            }
            var arrayString = string.Join(",", templates.Select(f => f.JsonData));
            return JsonResponse($"[{arrayString}]");
        }

        [HttpGet]
        public HttpResponseMessage GetThemes()
        {
            var themes = _settingsService.GetThemes().Where(t => !string.IsNullOrEmpty(t.JsonData));
            if (themes == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Themes not found");
            }
            var arrayString = string.Join(",", themes.Select(f => f.JsonData));
            return JsonResponse($"[{arrayString}]");
        }

        private HttpResponseMessage JsonResponse(string value)
        {
            var result = Request.CreateResponse(HttpStatusCode.OK);
            result.Content = new StringContent(value);
            result.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            return result;
        }
    }
}