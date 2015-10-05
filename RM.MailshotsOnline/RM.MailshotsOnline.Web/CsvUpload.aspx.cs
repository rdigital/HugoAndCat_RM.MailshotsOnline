using System;
using System.IO;
using System.Web;
using System.Web.UI;
using HC.RM.Common.Azure;
using HC.RM.Common.PCL.Helpers;
using RM.MailshotsOnline.Data.Services;
using RM.MailshotsOnline.PCL;
using RM.MailshotsOnline.PCL.Models;
using RM.MailshotsOnline.PCL.Services;

namespace RM.MailshotsOnline.Web
{
    public partial class CsvUpload : Page
    {
        private ILogger _logger;
        private IDataService _dataService;
        private IMembershipService _membershipService;

        private IMember _loggedInMember;
        private Guid _listId = Guid.Empty;
        private IDistributionList _list;

        protected void Page_Load(object sender, EventArgs e)
        {
            _logger = new Logger();
            _membershipService = new MembershipService(new CryptographicService());
            _dataService = new DataService(_logger);

            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                pnlLoginError.Visible = true;
                pnlFormArea.Visible = false;
            }
            else
            {
                _loggedInMember = _membershipService.GetCurrentMember();

                string rawListId = Request.QueryString["DistributionListId"];

                if (string.IsNullOrEmpty(hdnDistributionListId.Value) && (!string.IsNullOrEmpty(rawListId) && !Guid.TryParse(rawListId, out _listId)))
                {
                    ltlErrorMessages.Visible = true;
                    ltlErrorMessages.Text = "Invalid Distribution List Id Supplied.<br />";
                }

                if (_listId != Guid.Empty)
                {
                    _list = _dataService.GetDistributionListForUser(_loggedInMember.Id, _listId);

                    if (_list == null)
                    {
                        ltlErrorMessages.Visible = true;
                        ltlErrorMessages.Text += "Invalid Distribution List Id Supplied.<br />";
                    }
                }

                if (IsPostBack)
                {
                    saveCsv();
                }
            }
        }

        private void saveCsv()
        {
            bool valid = true;
            string errorMessage = string.Empty;

            if (fuUploadCsv.PostedFile.ContentLength == 0)
            {
                valid = false;
                errorMessage = "You must upload a CSV file.";
            }

            if (valid &&
                (!fuUploadCsv.PostedFile.ContentType.Equals(Constants.MimeTypes.Csv, StringComparison.InvariantCultureIgnoreCase) &&
                 !fuUploadCsv.PostedFile.FileName.EndsWith(".csv")))
            {
                valid = false;
                errorMessage = "You must upload a CSV file.";
            }

            byte[] bytes = null;
            if (valid)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    fuUploadCsv.PostedFile.InputStream.CopyTo(ms);
                    bytes = ms.ToArray();
                }
            }

            if (valid && _list == null)
            {
                // Creating a new list
                if (string.IsNullOrEmpty(hdnListName.Value))
                {
                    errorMessage = "You must supply a list name.<br />";
                    valid = false;
                }

                if (valid)
                {
                    _list = _dataService.CreateDistributionList(_loggedInMember, hdnListName.Value,
                                                               Enums.DistributionListState.ConfirmFields, bytes,
                                                               Constants.MimeTypes.Csv, Enums.DistributionListFileType.Working);
                }
            }
            else if (valid)
            {
                _list.ListState = Enums.DistributionListState.ConfirmFields;
                _list = _dataService.UpdateDistributionList(_list, bytes, Constants.MimeTypes.Csv,
                                                           Enums.DistributionListFileType.Working);
            }

            if (valid && _list != null)
            {
                hdnDistributionListId.Value = _list.DistributionListId.ToString("D");
                pnlFormArea.Visible = false;
                pnlSuccess.Visible = true;
                hdnSuccessState.Value = "success";
            }
            else
            {
                pnlSuccess.Visible = true;
                hdnSuccessState.Value = "failed";
                ltlErrorMessages.Text = errorMessage;
            }

        }
    }
}