using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Mvc;
using Glass.Mapper.Umb;
using HC.RM.Common.PCL.Helpers;
using RM.MailshotsOnline.Business.Processors;
using RM.MailshotsOnline.Data.Helpers;
using RM.MailshotsOnline.Entities.DataModels;
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

            if (model.DistributionListId == Guid.Empty && _dataService.ListNameIsAlreadyInUse(_loggedInMember.Id, model.ListName))
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
                HttpResponseMessage listResult = validateDistributionListId(model.DistributionListId, out list);

                if (listResult != null)
                {
                    return listResult;
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
            if (list == null)
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
            var authResult = Authenticate();

            if (authResult != null)
            {
                return authResult;
            }

            IDistributionList list;
            HttpResponseMessage listResult = validateDistributionListId(distributionListId, out list);

            if (listResult != null)
            {
                return listResult;
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

        /// <summary>
        /// Takes the confirmed field list and attempts to map the CSV to distribution contacts.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage PostConfirmFields(ModifyListConfirmFieldsModel model)
        {
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

            // Check we don't have any duplicate mappings:
            var hashset = new HashSet<string>();
            if (model.Mappings.Where(m => !string.IsNullOrEmpty(m)).Any(mapping => !hashset.Add(mapping)))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest,
                              new
                              {
                                  error = "Can only specify each mapping once.",
                                  param = "Mappings",
                                  statusCode = HttpStatusCode.BadRequest
                              });
            }

            IDistributionList list;
            HttpResponseMessage listResult = validateDistributionListId(model.DistributionListId, out list);

            if (listResult != null)
            {
                return listResult;
            }

            if (!string.IsNullOrEmpty(model.ListName) && list.Name != model.ListName)
            {
                if (_dataService.ListNameIsAlreadyInUse(_loggedInMember.Id, model.ListName))
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                                                  new
                                                  {
                                                      error = "List name is not unique.",
                                                      param = "ListName",
                                                      statusCode = HttpStatusCode.BadRequest
                                                  });
                }

                list.Name = model.ListName;
            }

            byte[] data = _dataService.GetDataFile(list, Enums.DistributionListFileType.Working);

            ModifyListMappedFieldsModel<DistributionContact> mappedContacts = _listProcessor.BuildListsFromFieldMappings<DistributionContact>(list,
                                                                                             model.Mappings, model.ColumnCount, model.FirstRowIsHeader ?? false, data);

            // Could all be errors/duplicates
            if (mappedContacts.ValidContacts.Any())
            {
                list = _dataService.CreateWorkingXml(list, mappedContacts.ValidContactsCount,
                                                                 mappedContacts.ValidContacts);
            }

            if (mappedContacts.InvalidContacts.Any() || mappedContacts.DuplicateContacts.Any())
            {
                list = _dataService.CreateErrorXml(list, mappedContacts.InvalidContactsCount,
                                                               mappedContacts.InvalidContacts,
                                                               mappedContacts.DuplicateContactsCount,
                                                               mappedContacts.DuplicateContacts);
            }

            var summaryModel = new ModifyListSummaryModel<DistributionContact>
            {
                DistributionListId = list.DistributionListId,
                ListName = list.Name,
                ValidContactCount = mappedContacts.ValidContactsCount,
                ValidContactsAdded = mappedContacts.ValidContactsCount,
                InvalidContactCount = mappedContacts.InvalidContactsCount,
                InvalidContactsAdded = mappedContacts.InvalidContactsCount,
                InvalidContacts = mappedContacts.InvalidContacts,
                DuplicateContactCount = mappedContacts.DuplicateContactsCount,
                DuplicateContactsAdded = mappedContacts.DuplicateContactsCount,
                DuplicateContacts = mappedContacts.DuplicateContacts
            };

            summaryModel.TotalContactCount = list.RecordCount + summaryModel.ValidContactCount;

            return Request.CreateResponse(HttpStatusCode.OK, summaryModel);
        }

        /// <summary>
        /// Gets the summary details if the page is loaded for a specific in-progress list.
        /// </summary>
        /// <param name="distributionListId">The distribution list identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetListSummary(Guid distributionListId)
        {
            var authResult = Authenticate();

            if (authResult != null)
            {
                return authResult;
            }

            IDistributionList list;
            HttpResponseMessage listResult = validateDistributionListId(distributionListId, out list);

            if (listResult != null)
            {
                return listResult;
            }

            return Request.CreateResponse(HttpStatusCode.OK, _dataService.CreateSummaryModel<DistributionContact>(list));
        }

        [HttpPost]
        public HttpResponseMessage PostAddContactsToList(ModifyListAddContactModel<DistributionContact> model)
        {
            string methodName = "PostUploadCsv";

            var authResult = Authenticate();

            if (authResult != null)
            {
                return authResult;
            }

            if (model.DistributionListId == Guid.Empty && _dataService.ListNameIsAlreadyInUse(_loggedInMember.Id, model.ListName))
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

            if (model.Contacts == null || !model.Contacts.Any())
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest,
                                              new
                                              {
                                                  error = "You must supply a contact.",
                                                  param = "Contact",
                                                  statusCode = HttpStatusCode.BadRequest
                                              });
            }

            IDistributionList list = null;

            if (model.DistributionListId != Guid.Empty)
            {
                HttpResponseMessage listResult = validateDistributionListId(model.DistributionListId, out list);

                if (listResult != null)
                {
                    return listResult;
                }
            }

            IModifyListSummaryModel<DistributionContact> summaryModel;

            if (list == null)
            {
                // Creating a new list
                list = new DistributionList
                       {
                           Name = model.ListName,
                           UserId = _loggedInMember.Id,
                           ListState = Enums.DistributionListState.AddNewContacts
                       };

                list = _dataService.CreateWorkingXml(list, model.Contacts.Count(), model.Contacts);

                summaryModel = new ModifyListSummaryModel<DistributionContact>
                               {
                                   DistributionListId = list.DistributionListId,
                                   ListName = list.Name,
                                   ValidContactCount = 1,
                                   TotalContactCount = 1
                               };
            }
            else
            {
                // Adding to an existing list - could be "in progress" or "complete"
                summaryModel = _dataService.UpdateWorkingXml(list, model.Contacts.ToList());
            }

            return Request.CreateResponse(HttpStatusCode.OK, summaryModel);
        }

        /// <summary>
        /// Finishes editing the list - either adds the working list to the existing list, or cleans up the in-progress parts.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage PostFinishList(ModifyListFinishModel model)
        {
            var authResult = Authenticate();

            if (authResult != null)
            {
                return authResult;
            }

            IDistributionList list;
            HttpResponseMessage listResult = validateDistributionListId(model.DistributionListId, out list);

            if (listResult != null)
            {
                return listResult;
            }

            if (!string.IsNullOrEmpty(model.ListName) && list.Name != model.ListName)
            {
                if (_dataService.ListNameIsAlreadyInUse(_loggedInMember.Id, model.ListName))
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                                                  new
                                                  {
                                                      error = "List name is not unique.",
                                                      param = "ListName",
                                                      statusCode = HttpStatusCode.BadRequest
                                                  });
                }

                list.Name = model.ListName;
            }

            switch (model.Command.ToLower())
            {
                case "finish":
                    // TODO: Merge with existing
                    if (!string.IsNullOrEmpty(list.BlobWorking))
                    {
                        _dataService.CompleteContactEdits(list);
                    }
                    else
                    {
                        _dataService.AbandonContactEdits(list);
                    }
                    break;
                case "cancel":
                    _dataService.AbandonContactEdits(list);
                    break;
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        private HttpResponseMessage validateDistributionListId(Guid distributionListId, out IDistributionList list)
        {
            list = null;

            if (distributionListId == Guid.Empty)
            {
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                                                         new
                                                         {
                                                             error = "You must supply a List Id.",
                                                             param = "DistributionListId",
                                                             statusCode = HttpStatusCode.BadRequest
                                                         });
                }
            }

            list = _dataService.GetDistributionListForUser(_loggedInMember.Id, distributionListId);

            if (list == null)
            {
                _logger.Info(_controllerName, "validateDistributionList",
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

            return null;
        }
    }
}
