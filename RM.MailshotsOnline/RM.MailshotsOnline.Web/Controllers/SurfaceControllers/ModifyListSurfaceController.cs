using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Web.Mvc;
using System.Xml.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using Glass.Mapper.Umb;
using HC.RM.Common;
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
        private readonly string _attributeListName = "listName";
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
            var pageModel = new ModifyListSummaryModel
                            {
                                DistributionListId = model.DistributionList.DistributionListId,
                                PageModel = model,
                            };

            // Grab the files
            if (!string.IsNullOrEmpty(model.DistributionList.BlobWorking))
            {
                byte[] validData = _dataService.GetDataFile(model.DistributionList,
                                                            Enums.DistributionListFileType.Working);

                using (var validStream = new MemoryStream(validData))
                {
                    using (var validReader = new StreamReader(validStream))
                    {
                        var validXml = XDocument.Load(validReader);

                        var distributionListElement = validXml.Element(_elementDistributionList);

                        if (distributionListElement == null)
                        {
                            _logger.Critical(GetType().Name, "ShowSummaryListForm",
                                             "Unable to load working XML document for user list: {0}:{1} - {2} ",
                                             model.DistributionList.UserId, model.DistributionList.DistributionListId,
                                             model.DistributionList.BlobWorking);
                            throw new ArgumentException();
                        }

                        pageModel.ValidContactCount = (int)distributionListElement.Attribute("count");
                    }
                }
            }

            if (!string.IsNullOrEmpty(model.DistributionList.BlobErrors))
            {
                byte[] errorData = _dataService.GetDataFile(model.DistributionList, Enums.DistributionListFileType.Errors);

                using (var errorStream = new MemoryStream(errorData))
                {
                    using (var errorReader = new StreamReader(errorStream))
                    {
                        var errorXml = XDocument.Load(errorReader);

                        var errorElement = errorXml.Element(_elementErrors);

                        if (errorElement == null)
                        {
                            _logger.Critical(GetType().Name, "ShowSummaryListForm",
                                             "Unable to load error XML document for user list: {0}:{1} - {2} ",
                                             model.DistributionList.UserId, model.DistributionList.DistributionListId,
                                             model.DistributionList.BlobErrors);
                            throw new ArgumentException();
                        }

                        var invalidElement = errorElement.Element(_elementInvalid);

                        if (invalidElement != null && invalidElement.Descendants().Any())
                        {
                            pageModel.InvalidContactCount =
                                (int)invalidElement.Attribute(_attributeCount);

                            var invalidContacts = new List<DistributionContact>(pageModel.InvalidContactCount);

                            foreach (var invalidContact in invalidElement.Elements())
                            {
                                using (var invalidXeReader = invalidContact.CreateReader())
                                {
                                    invalidContacts.Add((DistributionContact)_serialiser.ReadObject(invalidXeReader));
                                }
                            }

                            pageModel.InvalidContacts = invalidContacts;
                        }

                        var duplicateElement = errorElement.Element(_elementDuplicates);

                        if (duplicateElement != null && duplicateElement.Descendants().Any())
                        {
                            pageModel.DuplicateContactCount =
                                (int)duplicateElement.Attribute(_attributeCount);

                            var duplicateContacts = new List<DistributionContact>(pageModel.DuplicateContactCount);

                            foreach (var duplicateContact in duplicateElement.Elements())
                            {
                                using (var duplicateXeReader = duplicateContact.CreateReader())
                                {
                                    duplicateContacts.Add((DistributionContact)_serialiser.ReadObject(duplicateXeReader));
                                }
                            }

                            pageModel.DuplicateContacts = duplicateContacts;
                        }
                    }
                }
            }

            pageModel.TotalContactCount = model.DistributionList.RecordCount + pageModel.ValidContactCount;

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

            var validContacts = new Dictionary<string, DistributionContact>();
            var duplicateContacts = new List<DistributionContact>();
            var errorContacts = new Dictionary<Guid, DistributionContact>();

            // Build CSV Map
            var contactMap = new DefaultCsvClassMap<DistributionContact>();

            // mapping holds the Property - csv column mapping 
            for (int mappingIndex = 0; mappingIndex < model.ColumnCount; mappingIndex++)
            {
                var columnName = model.Mappings[mappingIndex];
                if (!string.IsNullOrEmpty(columnName))
                {
                    var propertyInfo = typeof (DistributionContact).GetProperty(columnName);
                    var newMap = new CsvPropertyMap(propertyInfo);
                    newMap.Index(mappingIndex);
                    contactMap.PropertyMaps.Add(newMap);
                }
            }

            // Should already have returned if FirstRowIsHeader is null.
            var csvConfig = new CsvConfiguration {HasHeaderRecord = model.FirstRowIsHeader ?? false};

            csvConfig.RegisterClassMap(contactMap);
            
            using (var stream = new MemoryStream(data))
            {
                using (var sr = new StreamReader(stream))
                {
                    // Assume No Header Row to start with.
                    using (var csv = new CsvReader(sr, csvConfig))
                    {
                        while (csv.Read())
                        {
                            var contact = csv.GetRecord<DistributionContact>();

                            if (contact != null)
                            {
                                contact.DistributionListId = Guid.NewGuid();
                                ICollection<ValidationResult> results;
                                bool isValid = contact.TryValidate(out results);

                                // TODO: Dedupe against existing list as well
                                if (isValid && !validContacts.ContainsKey(contact.AddressRef))
                                {
                                    validContacts.Add(contact.AddressRef, contact);
                                }
                                else if (!isValid)
                                {
                                    errorContacts.Add(contact.DistributionListId, contact);
                                }
                                else
                                {
                                    duplicateContacts.Add(contact);
                                }
                            }
                        }
                    }
                }
            }

            distributionList.ListState = Enums.DistributionListState.FixIssues;


            // Could all be errors/duplicates
            if (validContacts.Any())
            {
                // Convert Successful items into an XML doc
                var successfulXml = new XDocument();
                var distributionListElement = new XElement(_elementDistributionList,
                                                           new XAttribute(_attributeListName, distributionList.Name),
                                                           new XAttribute(_attributeCount, validContacts.Count));
                
                using (var successWriter = distributionListElement.CreateWriter())
                {
                    foreach (var contact in validContacts.Select(c => c.Value))
                    {
                        _serialiser.WriteObject(successWriter, contact);
                    }
                }

                successfulXml.Add(distributionListElement);


                using (var successfulStream = new MemoryStream())
                {
                    successfulXml.Save(successfulStream);
                    successfulStream.Position = 0;

                    _dataService.UpdateDistributionList(distributionList, successfulStream.ToArray(), "text/xml",
                                                        Enums.DistributionListFileType.Working);
                }
            }

            if (errorContacts.Any() || duplicateContacts.Any())
            {

                var errorsXml = new XDocument();
                var errorElement = new XElement(_elementErrors);
                errorsXml.Add(errorElement);
                if (errorContacts.Any())
                {
                    var invalidElement = new XElement(_elementInvalid, new XAttribute(_attributeListName, distributionList.Name),
                                                     new XAttribute(_attributeCount, errorContacts.Count));

                    using (var errorWriter = invalidElement.CreateWriter())
                    {
                        foreach (var contact in errorContacts.Select(c => c.Value))
                        {
                            _serialiser.WriteObject(errorWriter, contact);
                        }
                    }

                    errorElement.Add(invalidElement);
                }

                if (duplicateContacts.Any())
                {
                    var duplicateElement = new XElement(_elementDuplicates, new XAttribute(_attributeListName, distributionList.Name),
                                                     new XAttribute(_attributeCount, duplicateContacts.Count));

                    using (var dupWriter = duplicateElement.CreateWriter())
                    {
                        foreach (var contact in duplicateContacts)
                        {
                            _serialiser.WriteObject(dupWriter, contact);
                        }
                    }

                    errorElement.Add(duplicateElement);
                }

                using (var errorsStream = new MemoryStream())
                {
                    errorsXml.Save(errorsStream);
                    errorsStream.Position = 0;

                    _dataService.UpdateDistributionList(distributionList, errorsStream.ToArray(), "text/xml",
                                                        Enums.DistributionListFileType.Errors);
                }
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
                        byte[] data = _dataService.GetDataFile(distributionList, Enums.DistributionListFileType.Working);

                        using (var validStream = new MemoryStream(data))
                        {
                            using (var validReader = new StreamReader(validStream))
                            {
                                var validXml = XDocument.Load(validReader);

                                var distributionListElement = validXml.Element(_elementDistributionList);

                                if (distributionListElement == null)
                                {
                                    _logger.Critical(GetType().Name, "ShowSummaryListForm",
                                                     "Unable to load working XML document for user list: {0}:{1} - {2} ",
                                                     loggedInMember.Id, distributionList.DistributionListId,
                                                     distributionList.BlobWorking);
                                    throw new ArgumentException();
                                }

                                distributionList.RecordCount = (int)distributionListElement.Attribute("count");
                            }
                        }

                        _dataService.UpdateDistributionList(distributionList, data, "text/xml",
                                                            Enums.DistributionListFileType.Final);
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
