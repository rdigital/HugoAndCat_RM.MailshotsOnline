using System;
using System.Net;
using System.Net.Http;
using System.Web.Mvc;
using Glass.Mapper.Umb;
using HC.RM.Common.PCL.Helpers;
using RM.MailshotsOnline.Business.Processors;
using RM.MailshotsOnline.Data.Helpers;
using RM.MailshotsOnline.Entities.PageModels.Settings;
using RM.MailshotsOnline.Entities.ViewModels;
using RM.MailshotsOnline.PCL;
using RM.MailshotsOnline.PCL.Models;
using RM.MailshotsOnline.PCL.Services;

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
                    _logger.Info(_controllerName, methodName,
                                 "User specified a list that does not belong to them: {0}:{1}", _loggedInMember.Id,
                                 model.ListName);

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
                _logger.Info(_controllerName, methodName, "User did not upload a file: {0}:{1}", _loggedInMember.Id,
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
                                                           "text/csv",
                                                           Enums.DistributionListFileType.Working);
                created = true;
            }
            else
            {
                // If model.DistributionListId isn't empty, we've already returned 
                // if we couldn't find a list for this user.
                // ReSharper disable once PossibleNullReferenceException
                list.ListState = Enums.DistributionListState.ConfirmFields;
                list = _dataService.UpdateDistributionList(list, csvBytes, "text/csv",
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

    }
}
