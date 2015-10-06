using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Web.Mvc;
using System.Xml.Linq;
using Glass.Mapper.Umb;
using HC.RM.Common.PCL.Helpers;
using RM.MailshotsOnline.Business.Processors;
using RM.MailshotsOnline.Data.Helpers;
using RM.MailshotsOnline.Entities.PageModels;
using RM.MailshotsOnline.Entities.PageModels.Settings;
using RM.MailshotsOnline.Entities.ViewModels;
using RM.MailshotsOnline.PCL;
using RM.MailshotsOnline.PCL.Services;
using RM.MailshotsOnline.Web.Models;
using Umbraco.Web.Mvc;

namespace RM.MailshotsOnline.Web.Controllers.SurfaceControllers
{
    public class ModifyListSurfaceController: SurfaceController
    {
        private readonly ILogger _logger;
        private readonly IDataService _dataService;
        private readonly IMembershipService _membershipService;
        private readonly IUmbracoService _umbracoService;
        private readonly DistributionListProcessor _listProcessor;

        private readonly string _elementDistributionList = "distributionList";
        private readonly string _elementErrors = "errors";
        private readonly string _elementInvalid = "invalid";
        private readonly string _elementDuplicates = "duplicates";
        private readonly string _attributeCount = "count";

        private readonly DataContractSerializer _serialiser = new DataContractSerializer(typeof(DistributionContact));

        public ModifyListSurfaceController(ILogger logger, IDataService dataService, IMembershipService membershipService, IUmbracoService umbracoService)
        {
            _logger = logger;
            _dataService = dataService;
            _membershipService = membershipService;
            _umbracoService = umbracoService;
            _listProcessor = new DistributionListProcessor(_logger);
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

            var dataMappings = _umbracoService.CreateType<DataMappingFolder>(
                                                                 _umbracoService.ContentService
                                                                                .GetPublishedVersion(
                                                                                                     ConfigHelper
                                                                                                         .DataMappingFolderId));

            ViewBag.DataMappings = dataMappings.Mappings.Select(m => new SelectListItem { Value = m.FieldName, Text = m.Name });

            // Grab working copy from Blob Store and read the first two rows
            byte[] data = _dataService.GetDataFile(model.DistributionList, Enums.DistributionListFileType.Working);

            ModifyListConfirmFieldsModel pageModel = _listProcessor.AttemptToMapDataToColumns(model.DistributionList, dataMappings, data);

            pageModel.PageModel = model;

            return PartialView("~/Views/Lists/Partials/ShowConfirmFieldsForm.cshtml", pageModel);
        }

        [ChildActionOnly]
        public ActionResult ShowSummaryListForm(ListCreate model)
        {
            var pageModel = _dataService.CreateSummaryModel<DistributionContact>(model.DistributionList) as ModifyListSummaryModel<DistributionContact>;

            if (pageModel == null)
            {
                _logger.Critical(GetType().Name, "ShowSummaryListForm", "Unable to cast IModifySummaryListModel as ModifySummaryListModel.");
                throw new ApplicationException("Unable to cast IModifySummaryListModel as ModifySummaryListModel.");
            }

            pageModel.DistributionListId = model.DistributionList.DistributionListId;
            pageModel.PageModel = model;

            return PartialView("~/Views/Lists/Partials/ShowSummaryListForm.cshtml", pageModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadFileToList(ModifyListUploadFileModel model)
        {
            var loggedInMember = _membershipService.GetCurrentMember();

            // If this is a new list, check that the name is unique
            if (model.DistributionListId == Guid.Empty)
            {
                bool nameIsNotUnique = _dataService.ListNameIsAlreadyInUse(loggedInMember.Id, model.ListName);

                if (nameIsNotUnique)
                {
                    _logger.Info("ModifyListSurfaceController", "UploadFileToList",
                                 "User specified a duplicate name: {0}:{1}", loggedInMember.Id, model.ListName);

                    // TODO: Error from Umbraco.
                    ModelState.AddModelError("ListName", "Your list name is not unique - please supply a unique name.");
                }
            }

            if (model.UploadCsv != null)
            {
                // Confirm that the file is good too:
                bool validFile = false;
                string fileName = Path.GetFileName(model.UploadCsv.FileName);
                string mimeType = model.UploadCsv.ContentType;
                // Is the MimeType .csv? Not if Excel is installed on the machine (in which case they will be application/vnd.ms-excel, which is the same as an Excel .xls file.)...
                if (mimeType.Equals(Constants.MimeTypes.Csv, StringComparison.InvariantCultureIgnoreCase))
                {
                    validFile = true;
                    _logger.Info("ModifyListSurfaceController", "UploadFileToList",
                                 "User has uploaded a valid .csv file: {0}", fileName);
                }

                if (!validFile)
                {
                    // The mimetype wasn't csv, proceeding with caution:
                    if (!string.IsNullOrEmpty(fileName) && fileName.EndsWith(".csv"))
                    {
                        validFile = true;
                        _logger.Info("ModifyListSurfaceController", "UploadFileToList",
                                     "User has uploaded a .csv file with the wrong mime type: {0}:{1}", fileName,
                                     mimeType);
                    }
                }

                if (!validFile)
                {
                    ModelState.AddModelError("uploadCsv", "Invalid file uploaded - is it a .csv file?");
                }
            }

            if (!ModelState.IsValid)
            {
                return CurrentUmbracoPage();
            }

            // ReSharper disable once PossibleNullReferenceException
            // It's a required field on the model, and we've already returned if it's null
            byte[] csvBytes = new byte[model.UploadCsv.ContentLength];
            model.UploadCsv.InputStream.Read(csvBytes, 0, model.UploadCsv.ContentLength);

            // Create new list and move on to mext page...
            // TODO: Is this a new list, or are we adding to an existing one?
            var newList = _dataService.CreateDistributionList(loggedInMember, model.ListName,
                                                              Enums.DistributionListState.ConfirmFields, csvBytes,
                                                              Constants.MimeTypes.Csv, Enums.DistributionListFileType.Working);

            var path = Umbraco.Url(CurrentPage.Id);
            return Redirect(path + "?listId=" + newList.DistributionListId);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmFields(ModifyListConfirmFieldsModel model)
        {
            if (!ModelState.IsValid)
            {
                return CurrentUmbracoPage();
            }

            var loggedInMember = _membershipService.GetCurrentMember();

            var distributionList = _dataService.GetDistributionListForUser(loggedInMember.Id, model.DistributionListId);

            byte[] data = _dataService.GetDataFile(distributionList, Enums.DistributionListFileType.Working);

            ModifyListMappedFieldsModel<DistributionContact> mappedContacts = _listProcessor.BuildListsFromFieldMappings<DistributionContact>(distributionList,
                                                                                             model.Mappings, model.ColumnCount, model.FirstRowIsHeader ?? false, data);

            // Could all be errors/duplicates
            if (mappedContacts.ValidContacts.Any())
            {
                distributionList = _dataService.CreateWorkingXml<DistributionContact>(distributionList, mappedContacts.ValidContactsCount,
                                                                 mappedContacts.ValidContacts);
            }

            if (mappedContacts.InvalidContacts.Any() || mappedContacts.DuplicateContacts.Any())
            {
                _dataService.CreateErrorXml<DistributionContact>(distributionList, mappedContacts.InvalidContactsCount,
                                                               mappedContacts.InvalidContacts,
                                                               mappedContacts.DuplicateContactsCount,
                                                               mappedContacts.DuplicateContacts);
            }

            var path = Umbraco.Url(CurrentPage.Id);
            return Redirect(path + "?listId=" + model.DistributionListId);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FinishList(ModifyListFinishModel model, string command)
        {
            var loggedInMember = _membershipService.GetCurrentMember();

            var distributionList = _dataService.GetDistributionListForUser(loggedInMember.Id, model.DistributionListId);

            switch (command.ToLower())
            {
                case "finish":
                    // TODO: Merge with existing
                    if (!string.IsNullOrEmpty(distributionList.BlobWorking))
                    {
                        _dataService.CompleteContactEdits(distributionList);
                    }
                    else
                    {
                        _dataService.AbondonContactEdits(distributionList);
                    }
                    break;
                case "cancel":
                    _dataService.AbondonContactEdits(distributionList);
                    break;
            }

            var path = Umbraco.Url(CurrentPage.Parent.Id);
            return Redirect(path);
        }
    }
}
