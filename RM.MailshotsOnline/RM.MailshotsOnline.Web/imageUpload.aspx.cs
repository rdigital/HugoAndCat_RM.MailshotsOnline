using HC.RM.Common.Azure;
using HC.RM.Common.PCL.Helpers;
using RM.MailshotsOnline.Data.Services;
using RM.MailshotsOnline.Entities.JsonModels;
using RM.MailshotsOnline.PCL.Models;
using RM.MailshotsOnline.PCL.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RM.MailshotsOnline.Web
{
    public partial class imageUpload : System.Web.UI.Page
    {
        internal IMember _loggedInMember;
        private ILogger _logger;
        private IImageLibraryService _imageLibrary;
        private ICmsImageService _cmsImageService;
        private IMembershipService _membershipService;

        protected void Page_Load(object sender, EventArgs e)
        {
            _logger = new Logger();
            _membershipService = new MembershipService(new CryptographicService());
            _cmsImageService = new CmsImageService();
            _imageLibrary = new ImageLibraryService(_logger, _cmsImageService);

            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                this.loginError.Visible = true;
                this.form1.Visible = false;
            }
            else
            {
                _loggedInMember = _membershipService.GetCurrentMember();
                if (this.IsPostBack)
                {
                    SaveImage();
                }
            }

            //this.saveButton.ServerClick += SaveButton_ServerClick;
        }

        //private void SaveButton_ServerClick(object sender, EventArgs e)
        //{
        //    SaveImage();
        //}

        private void SaveImage()
        {
            bool valid = true;
            string errorMessage = string.Empty;

            if (string.IsNullOrEmpty(nameInput.Value))
            {
                valid = false;
                errorMessage = "You must enter a name";
            }

            if (valid && fileUpload.PostedFile.ContentLength == 0)
            {
                valid = false;
                errorMessage = "You must post a file";
            }

            if (valid && !fileUpload.PostedFile.ContentType.Contains("image"))
            {
                valid = false;
                errorMessage = "You must upload an image file";
            }

            if (valid)
            {
                // Attempt to save the image
                string name = this.nameInput.Value;
                byte[] bytes = null;
                using (MemoryStream ms = new MemoryStream())
                {
                    fileUpload.PostedFile.InputStream.CopyTo(ms);
                    bytes = ms.ToArray();
                }

                PCL.Models.IMedia media = null;
                try
                {
                    media = _imageLibrary.AddImage(bytes, name, _loggedInMember);
                }
                catch (Exception ex)
                {
                    _logger.Exception(this.GetType().Name, "UploadImage", ex);
                    _logger.Error(this.GetType().Name, "UploadImage", "Unable to save image to Umbraco.");
                }

                if (media == null)
                {
                    errorMessageParagraph.InnerText = "Error saving image";
                }
                var privateImage = media as PrivateLibraryImage;
                success.Visible = true;
                formArea.Visible = false;
                imageResult.Value = privateImage.Src;
            }
            else
            {
                errorMessageParagraph.InnerText = errorMessage;
            }
        }
    }
}