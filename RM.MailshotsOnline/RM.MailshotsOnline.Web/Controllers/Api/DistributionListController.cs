using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
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
using RM.MailshotsOnline.Entities.PageModels.Settings;
using RM.MailshotsOnline.Entities.ViewModels;
using RM.MailshotsOnline.PCL;
using RM.MailshotsOnline.PCL.Models;
using RM.MailshotsOnline.PCL.Services;
using RM.MailshotsOnline.Web.Models;

namespace RM.MailshotsOnline.Web.Controllers.Api
{
    public class DistributionListController: ApiBaseController
    {
        private readonly string _controllerName;
        private readonly IDataService _dataService;
        private readonly IUmbracoService _umbracoService;
        private readonly DistributionListProcessor _listProcessor;

        private readonly string _elementDistributionList = "distributionList";
        private readonly string _elementErrors = "errors";
        private readonly string _elementInvalid = "invalid";
        private readonly string _elementDuplicates = "duplicates";
        private readonly string _attributeListName = "listName";
        private readonly string _attributeCount = "count";

        private readonly DataContractSerializer _serialiser = new DataContractSerializer(typeof(DistributionContact));

        public DistributionListController(IMembershipService membershipService, ILogger logger, IDataService dataService, IUmbracoService umbracoService) : base(membershipService, logger)
        {
            _controllerName = GetType().Name;
            _dataService = dataService;
            _umbracoService = umbracoService;
            _listProcessor = new DistributionListProcessor(_logger);
        }

        /// <summary>
        /// Gets all lists for the currently logged in user.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetMyLists()
        {
            var authResult = Authenticate();

            if (authResult != null)
            {
                return authResult;
            }

            var results = _dataService.GetDistributionListsForUser(_loggedInMember.Id);

            return Request.CreateResponse(HttpStatusCode.OK, results);
        }

        /// <summary>
        /// Gets all rented lists for the currently logged in user.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetMyRentedLists()
        {
            var authResult = Authenticate();

            if (authResult != null)
            {
                return authResult;
            }

            var results = _dataService.GetDistributionLists(l => l.UserId == -1);

            return Request.CreateResponse(HttpStatusCode.OK, results);
        }

        /// <summary>
        /// Checks to see if the supplied list name is unique for the currently logged in user.
        /// </summary>
        /// <param name="listName">Name of the list.</param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetCheckListNameIsUnique(string listName)
        {
            var authResult = Authenticate();

            if (authResult != null)
            {
                return authResult;
            }

            bool nameIsNotUnique = _dataService.ListNameIsAlreadyInUse(_loggedInMember.Id, listName);

            if (nameIsNotUnique)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { error = "List name is not unique.", statusCode = HttpStatusCode.BadRequest });
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        /// <summary>
        /// Uploads a CSV and attempts to map the fields onto our contact fields.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage PostUploadCsv(ModifyListUploadFileModel model)
        {
            string methodName = "PostUploadCsv";

            var authResult = Authenticate();

            if (authResult != null)
            {
                return authResult;
            }

            if (model.DistributionListId == Guid.Empty &&
                _dataService.ListNameIsAlreadyInUse(_loggedInMember.Id, model.ListName))
            {
                _logger.Info(_controllerName, methodName,
                             "User specified a duplicate name: {0}:{1}", _loggedInMember.Id, model.ListName);

                return Request.CreateResponse(HttpStatusCode.BadRequest,
                                              new
                                              {
                                                  error = "List name is not unique.",
                                                  param = "ListName",
                                                  statusCode = HttpStatusCode.BadRequest
                                              });
            }

            IDistributionList list = null;
            if (model.DistributionListId != Guid.Empty)
            {
                list = _dataService.GetDistributionListForUser(_loggedInMember.Id, model.DistributionListId);

                if (list == null)
                {
                    _logger.Warn(_controllerName, methodName,
                                 "User specified a list that does not belong to them: {0}:{1}", _loggedInMember.Id,
                                 model.DistributionListId);

                    return Request.CreateResponse(HttpStatusCode.NotFound,
                                                  new
                                                  {
                                                      error = "List Id was not found.",
                                                      param = "DistributionListId",
                                                      statusCode = HttpStatusCode.NotFound
                                                  });
                }
            }

            if (string.IsNullOrEmpty(model.CsvString))
            {
                _logger.Warn(_controllerName, methodName, "User did not upload a file: {0}:{1}", _loggedInMember.Id,
                             model.ListName);

                return Request.CreateResponse(HttpStatusCode.BadRequest,
                                              new
                                              {
                                                  error = "Please upload a CSV.",
                                                  param = "CsvString",
                                                  statusCode = HttpStatusCode.BadRequest
                                              });
            }

            byte[] csvBytes = null;

            try
            {
                var base64String = model.CsvString;
                if (base64String.StartsWith("data:"))
                {
                    var stringParts = base64String.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries);
                    if (stringParts.Length == 2)
                    {
                        base64String = stringParts[1];
                    }
                }
                csvBytes = Convert.FromBase64String(base64String);
            }
            catch (Exception ex)
            {
                _logger.Error(_controllerName, methodName, "User did not upload a valid file: {0}:{1}",
                              _loggedInMember.Id, model.ListName);
                _logger.Exception(_controllerName, methodName, ex);
            }

            if (csvBytes == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest,
                                              new
                                              {
                                                  error = "Please upload a csv.",
                                                  param = "CsvString",
                                                  statusCode = HttpStatusCode.BadRequest
                                              });
            }

            bool created = false;
            // Save CSV to storage:
            if (model.DistributionListId == Guid.Empty)
            {
                list = _dataService.CreateDistributionList(_loggedInMember, model.ListName,
                                                           Enums.DistributionListState.ConfirmFields, csvBytes,
                                                           Constants.MimeTypes.Csv,
                                                           Enums.DistributionListFileType.Working);
                created = true;
            }
            else
            {
                // If model.DistributionListId isn't empty, we've already returned 
                // if we couldn't find a list for this user.
                // ReSharper disable once PossibleNullReferenceException
                list.ListState = Enums.DistributionListState.ConfirmFields;
                list = _dataService.UpdateDistributionList(list, csvBytes, Constants.MimeTypes.Csv,
                                                           Enums.DistributionListFileType.Working);
            }

            var dataMappings = _umbracoService.CreateType<DataMappingFolder>(
                                                                 _umbracoService.ContentService
                                                                                .GetPublishedVersion(
                                                                                                     ConfigHelper
                                                                                                         .DataMappingFolderId));

            var confirmFieldsModel = _listProcessor.AttemptToMapDataToColumns(list, dataMappings, csvBytes);

            return Request.CreateResponse(created ? HttpStatusCode.Created : HttpStatusCode.OK, confirmFieldsModel);
        }

        /// <summary>
        /// If you have to upload the CSV separately (IE9 and below), this will allow you to grab the mappings.
        /// </summary>
        /// <param name="distributionListId">The distribution list identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetListMappings(Guid distributionListId)
        {
            string methodName = "GetListMappings";

            var authResult = Authenticate();

            if (authResult != null)
            {
                return authResult;
            }

            if (distributionListId == Guid.Empty)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest,
                                              new
                                              {
                                                  error = "You must supply a List Id.",
                                                  param = "DistributionListId",
                                                  statusCode = HttpStatusCode.BadRequest
                                              });
            }

            IDistributionList list = _dataService.GetDistributionListForUser(_loggedInMember.Id, distributionListId);

            if (list == null)
            {
                _logger.Info(_controllerName, methodName,
                             "User specified a list that does not belong to them: {0}:{1}", _loggedInMember.Id,
                             distributionListId);

                return Request.CreateResponse(HttpStatusCode.NotFound,
                                              new
                                              {
                                                  error = "List Id was not found.",
                                                  param = "DistributionListId",
                                                  statusCode = HttpStatusCode.NotFound
                                              });
            }

            var dataMappings = _umbracoService.CreateType<DataMappingFolder>(
                                                     _umbracoService.ContentService
                                                                    .GetPublishedVersion(
                                                                                         ConfigHelper
                                                                                             .DataMappingFolderId));

            // Grab working copy from Blob Store and read the first two rows
            byte[] data = _dataService.GetDataFile(list, Enums.DistributionListFileType.Working);

            ModifyListConfirmFieldsModel confirmFieldsModel = _listProcessor.AttemptToMapDataToColumns(list, dataMappings, data);

            return Request.CreateResponse(HttpStatusCode.OK, confirmFieldsModel);
        }

        [HttpPost]
        public HttpResponseMessage PostConfirmFields(ModifyListConfirmFieldsModel model)
        {
            string methodName = "PostConfirmFields";

            var authResult = Authenticate();

            if (authResult != null)
            {
                return authResult;
            }

            if (model.FirstRowIsHeader == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest,
                              new
                              {
                                  error = "You need to specify whether we should import the first row.",
                                  param = "FirstRowIsHeader",
                                  statusCode = HttpStatusCode.BadRequest
                              });
            }

            if (model.Mappings == null || !model.Mappings.Any())
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest,
                              new
                              {
                                  error = "You need to select some mappings.",
                                  param = "Mappings",
                                  statusCode = HttpStatusCode.BadRequest
                              });
            }

            if (
                !((model.Mappings.Contains("FirstName") || model.Mappings.Contains("Surname")) &&
                  model.Mappings.Contains("Address1") && model.Mappings.Contains("PostCode")))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest,
                              new
                              {
                                  error = "You need to select at least one name field, as well as Address 1 and Post Code fields.",
                                  param = "Mappings",
                                  statusCode = HttpStatusCode.BadRequest
                              });
            }

            if (model.DistributionListId == Guid.Empty)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest,
                              new
                              {
                                  error = "You need to supply an existing list id.",
                                  param = "DistributionListId",
                                  statusCode = HttpStatusCode.BadRequest
                              });
            }

            IDistributionList distributionList = null;
            if (model.DistributionListId != Guid.Empty)
            {
                distributionList = _dataService.GetDistributionListForUser(_loggedInMember.Id, model.DistributionListId);

                if (distributionList == null)
                {
                    _logger.Warn(_controllerName, methodName,
                                 "User specified a list that does not belong to them: {0}:{1}", _loggedInMember.Id,
                                 model.DistributionListId);

                    return Request.CreateResponse(HttpStatusCode.NotFound,
                                                  new
                                                  {
                                                      error = "List Id was not found.",
                                                      param = "DistributionListId",
                                                      statusCode = HttpStatusCode.NotFound
                                                  });
                }
            }

            byte[] data = _dataService.GetDataFile(distributionList, Enums.DistributionListFileType.Working);

            ModifyListMappedFieldsModel<DistributionContact> mappedContacts = _listProcessor.BuildListsFromFieldMappings<DistributionContact>(distributionList,
                                                                                             model.Mappings, model.ColumnCount, model.FirstRowIsHeader ?? false, data);

            // We should have already bailed out if we weren't able to populate distributionList
            // ReSharper disable once PossibleNullReferenceException
            distributionList.ListState = Enums.DistributionListState.FixIssues;

            // Could all be errors/duplicates
            if (mappedContacts.ValidContacts.Any())
            {
                // Convert Successful items into an XML doc
                var successfulXml = new XDocument();
                var distributionListElement = new XElement(_elementDistributionList,
                                                           new XAttribute(_attributeListName, distributionList.Name),
                                                           new XAttribute(_attributeCount, mappedContacts.ValidContactsCount));

                using (var successWriter = distributionListElement.CreateWriter())
                {
                    foreach (var contact in mappedContacts.ValidContacts)
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

            if (mappedContacts.InvalidContacts.Any() || mappedContacts.DuplicateContacts.Any())
            {

                var errorsXml = new XDocument();
                var errorElement = new XElement(_elementErrors);
                errorsXml.Add(errorElement);
                if (mappedContacts.InvalidContacts.Any())
                {
                    var invalidElement = new XElement(_elementInvalid, new XAttribute(_attributeListName, distributionList.Name),
                                                     new XAttribute(_attributeCount, mappedContacts.InvalidContactsCount));

                    using (var errorWriter = invalidElement.CreateWriter())
                    {
                        foreach (var contact in mappedContacts.InvalidContacts)
                        {
                            _serialiser.WriteObject(errorWriter, contact);
                        }
                    }

                    errorElement.Add(invalidElement);
                }

                if (mappedContacts.DuplicateContacts.Any())
                {
                    var duplicateElement = new XElement(_elementDuplicates, new XAttribute(_attributeListName, distributionList.Name),
                                                     new XAttribute(_attributeCount, mappedContacts.DuplicateContactsCount));

                    using (var dupWriter = duplicateElement.CreateWriter())
                    {
                        foreach (var contact in mappedContacts.DuplicateContacts)
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

            var summaryModel = new ModifyListSummaryModel<DistributionContact>
            {
                DistributionListId = distributionList.DistributionListId,
                ListName = distributionList.Name,
                ValidContactCount = mappedContacts.ValidContactsCount,
                InvalidContactCount = mappedContacts.InvalidContactsCount,
                InvalidContacts = mappedContacts.InvalidContacts,
                DuplicateContactCount = mappedContacts.DuplicateContactsCount,
                DuplicateContacts = mappedContacts.DuplicateContacts,
            };

            summaryModel.TotalContactCount = distributionList.RecordCount + summaryModel.ValidContactCount;

            return Request.CreateResponse(HttpStatusCode.OK, summaryModel);
        }
    }
}
