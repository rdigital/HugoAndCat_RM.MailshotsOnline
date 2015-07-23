using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace RM.MailshotsOnline.Web.Attributes
{
    public class Recaptcha : ValidationAttribute
    {
        private readonly string _secret = ConfigurationManager.AppSettings["RecaptchaSecretKey"];

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var captchaResponse = HttpContext.Current.Request["g-recaptcha-response"];

            var url = $"https://www.google.com/recaptcha/api/siteverify?secret={_secret}&response={captchaResponse}";

            var request = WebRequest.Create(url);
            var response = request.GetResponse();
            var reader = new StreamReader(response.GetResponseStream());

            var reply = reader.ReadToEnd().Trim();

            if (reply.Contains("\"success\": true"))
            {
                return ValidationResult.Success;
            }

            return new ValidationResult("Recaptcha invalid");
        }
    }
}
