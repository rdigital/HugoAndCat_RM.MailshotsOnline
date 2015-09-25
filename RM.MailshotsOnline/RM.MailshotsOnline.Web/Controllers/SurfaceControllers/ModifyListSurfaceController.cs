﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Castle.Windsor.Installer;
using Glass.Mapper.Umb;
using HC.RM.Common.PCL.Helpers;
using RM.MailshotsOnline.Data.Helpers;
using RM.MailshotsOnline.Entities.PageModels;
using RM.MailshotsOnline.Entities.PageModels.Settings;
using RM.MailshotsOnline.Entities.ViewModels;
using RM.MailshotsOnline.PCL;
using RM.MailshotsOnline.PCL.Services;
using Umbraco.Web.Mvc;

namespace RM.MailshotsOnline.Web.Controllers.SurfaceControllers
{
    public class ModifyListSurfaceController: SurfaceController
    {
        private readonly ILogger _logger;
        private readonly IDataService _dataService;
        private readonly IMembershipService _membershipService;
        private readonly IUmbracoService _umbracoService;

        public ModifyListSurfaceController(ILogger logger, IDataService dataService, IMembershipService membershipService, IUmbracoService umbracoService)
        {
            _logger = logger;
            _dataService = dataService;
            _membershipService = membershipService;
            _umbracoService = umbracoService;
        }

        [ChildActionOnly]
        public ActionResult ShowCreateListForm(ListCreate model)
        {
            var pageModel = new ModifyListUploadFileModel {PageModel = model};
            return PartialView("~/Views/Lists/Partials/ShowCreateListForm.cshtml", pageModel);
        }

        [ChildActionOnly]
        public ActionResult ShowConfirmFieldsForm(ListCreate model)
        {
            var pageModel = new ModifyListConfirmFieldsModel
                            {
                                PageModel = model,
                                DataMappings =
                                    _umbracoService.CreateType<DataMappingFolder>(
                                                                                  _umbracoService.ContentService
                                                                                                 .GetPublishedVersion(
                                                                                                                      ConfigHelper
                                                                                                                          .DataMappingFolderId),
                                                                                  false, false)
                            };

            // Grab working copy from Blob Store and read the first two rows
            List<string> lines = _dataService.GetFirstTwoLinesOfWorkingFile(model.DistributionList);

            // Check to see if the first row contains anything resembling column headers


            return PartialView("~/Views/Lists/Partials/ShowConfirmFieldsForm.cshtml", pageModel);
        }

        [HttpPost]
        public ActionResult UploadFileToList(ModifyListUploadFileModel model, HttpPostedFileBase uploadCsv)
        {
            if (ModelState.IsValid)
            {
                var loggedInMember = _membershipService.GetCurrentMember();

                // If this is a new list, check that the name is unique
                if (model.DistributionListId == Guid.Empty)
                {
                    bool nameIsNotUnique =
                        _dataService.GetDistributionListsForUser(loggedInMember.Id).Any(d => d.Name == model.ListName);

                    if (nameIsNotUnique)
                    {
                        _logger.Info("ModifyListSurfaceController", "UploadFileToList", "User specified a duplicate name: {0}:{1}", loggedInMember, model.ListName);

                        // TODO: Error from Umbraco.
                        ModelState.AddModelError("ListName", "Your list name is not unique - please supply a unique name.");
                    }
                }

                // Confirm that the file is good too:
                if (ModelState.IsValid && uploadCsv != null && uploadCsv.ContentLength != 0)
                {
                    bool validFile = false;
                    string fileName = Path.GetFileName(uploadCsv.FileName);
                    string mimeType = uploadCsv.ContentType;
                    // Is the MimeType .csv? Not if Excel is installed on the machine (in which case they will be application/vnd.ms-excel, which is the same as an Excel .xls file.)...
                    if (mimeType.Equals("text/csv", StringComparison.InvariantCultureIgnoreCase))
                    {
                        validFile = true;
                        _logger.Info("ModifyListSurfaceController", "UploadFileToList", "User has uploaded a valid .csv file: {0}", fileName);
                    }

                    if (!validFile)
                    {
                        // The mimetype wasn't csv, proceeding with caution:
                        if (!string.IsNullOrEmpty(fileName) && fileName.EndsWith(".csv"))
                        {
                            validFile = true;
                            _logger.Info("ModifyListSurfaceController", "UploadFileToList", "User has uploaded a .csv file with the wrong mime type: {0}:{1}", fileName, mimeType);
                        }
                    }

                    if (validFile)
                    {
                        byte[] csvBytes = new byte[uploadCsv.ContentLength];
                        uploadCsv.InputStream.Read(csvBytes, 0, uploadCsv.ContentLength);

                        // Create new list and move on to mext page...
                        _dataService.CreateDistributionList(loggedInMember, model.ListName, csvBytes, "text/csv",
                                                            Enums.DistributionListFileType.Working);

                        var path = Umbraco.Url(CurrentPage.Id);
                        return Redirect(path + "?listId=" + model.DistributionListId);
                    }

                    ModelState.AddModelError("uploadCsv", "Invalid file uploaded - is it a .csv file?");
                }
                else
                {
                    // TODO: Add error text to Umbraco
                    ModelState.AddModelError("uploadCsv", "You must upload a .csv file");
                }
            }
            return CurrentUmbracoPage();
        }
    }
}
