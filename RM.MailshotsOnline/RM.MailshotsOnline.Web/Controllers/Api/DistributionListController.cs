using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Mvc;
using CsvHelper;
using CsvHelper.Configuration;
using Glass.Mapper.Umb;
using HC.RM.Common.PCL.Helpers;
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
        private readonly string _controllerName = "DistributionListController";
        private readonly IDataService _dataService;
        private readonly IUmbracoService _umbracoService;


        public DistributionListController(IMembershipService membershipService, ILogger logger, IDataService dataService, IUmbracoService umbracoService) : base(membershipService, logger)
        {
            _dataService = dataService;
            _umbracoService = umbracoService;
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
                                 "User specified a list that does not belong to them: {0}:{1}", _loggedInMember.Id, model.ListName);

                    return Request.CreateResponse(HttpStatusCode.NotFound,
                                                  new
                                                  {
                                                      error = "List Id was not found.",
                                                      param = "DistributionListId",
                                                      statusCode = HttpStatusCode.NotFound
                                                  });

                }
            }

            if (model.UploadCsv == null && model.CsvData == null && string.IsNullOrEmpty(model.CsvString))
            {
                _logger.Info(_controllerName, methodName, "User did not upload a file: {0}:{1}", _loggedInMember.Id,
                             model.ListName);

                return Request.CreateResponse(HttpStatusCode.BadRequest,
                                              new
                                              {
                                                  error = "Please upload a csv.",
                                                  param = "CsvString",
                                                  statusCode = HttpStatusCode.BadRequest
                                              });
            }

            //TODO: Remove this:
            _logger.Info(_controllerName, methodName, "Uploaded Data: uploadCsv: {0}; csvData: {1}; csvString: {2}",
                         model.UploadCsv != null, model.CsvData?.Length.ToString() ?? "null",
                         !string.IsNullOrEmpty(model.CsvString));


            byte[] csvBytes = null;

            if (model.UploadCsv != null)
            {
                bool validFile = false;
                string fileName = Path.GetFileName(model.UploadCsv.FileName);
                string mimeType = model.UploadCsv.ContentType;
                // Is the MimeType .csv? Not if Excel is installed on the machine (in which case they will be application/vnd.ms-excel, which is the same as an Excel .xls file.)...
                if (mimeType.Equals("text/csv", StringComparison.InvariantCultureIgnoreCase))
                {
                    validFile = true;
                    _logger.Info(_controllerName, methodName,
                                 "User has uploaded a valid .csv file: {0}", fileName);
                }

                if (!validFile)
                {
                    // The mimetype wasn't csv, proceeding with caution:
                    if (!string.IsNullOrEmpty(fileName) && fileName.EndsWith(".csv"))
                    {
                        validFile = true;
                        _logger.Info(_controllerName, methodName,
                                     "User has uploaded a .csv file with the wrong mime type: {0}:{1}", fileName,
                                     mimeType);
                    }
                }

                if (!validFile)
                {
                    _logger.Error(_controllerName, methodName, "User did not upload a valid file: {0}:{1}",
                                  _loggedInMember.Id, model.ListName);

                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                                                  new
                                                  {
                                                      error = "Please upload a csv.",
                                                      param = "UploadCsv",
                                                      statusCode = HttpStatusCode.BadRequest
                                                  });
                }

                csvBytes = new byte[model.UploadCsv.ContentLength];
                model.UploadCsv.InputStream.Read(csvBytes, 0, model.UploadCsv.ContentLength);
            }
            else if (model.CsvData != null)
            {
                csvBytes = model.CsvData;
            }
            else
            {
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

            var confirmFieldsModel = new ModifyListConfirmFieldsModel
                                     {
                                         DistributionListId = list.DistributionListId,
                                         FirstRowIsHeader = null,
                                     };

            var dataMappings = _umbracoService.CreateType<DataMappingFolder>(
                                                                             _umbracoService.ContentService
                                                                                            .GetPublishedVersion(
                                                                                                                 ConfigHelper
                                                                                                                     .DataMappingFolderId));

            using (var stream = new MemoryStream(csvBytes))
            {
                using (var sr = new StreamReader(stream))
                {
                    // Assume No Header Row to start with.
                    using (var csv = new CsvReader(sr, new CsvConfiguration { HasHeaderRecord = false }))
                    {
                        int rows = 0;
                        int columns = 0;

                        List<KeyValuePair<string, string>> items = null;

                        while (rows < 2)
                        {

                            try
                            {
                                csv.Read();

                                if (rows == 0)
                                {
                                    columns = csv.CurrentRecord.Length;
                                    confirmFieldsModel.ColumnCount = columns;
                                    confirmFieldsModel.FirstTwoRowsWithGuessedMappings = new List<Tuple<string, string, string>>(columns);
                                    items = new List<KeyValuePair<string, string>>(columns);
                                }

                                for (int column = 0; column < columns; column++)
                                {
                                    if (rows == 0)
                                    {
                                        // First Row
                                        // Grab Value and see if we can find a mapping for it:
                                        var possibleHeading = csv.GetField(column);

                                        var possibleMapping =
                                            dataMappings.Mappings.FirstOrDefault(
                                                                                 m =>
                                                                                     m.FieldMappings.Contains(
                                                                                                              possibleHeading.ToLower().Trim()));

                                        if (possibleMapping != null)
                                        {
                                            // We think we might have a heading row
                                            confirmFieldsModel.FirstRowIsHeader = true;
                                        }

                                        // ReSharper disable once PossibleNullReferenceException
                                        items.Add(new KeyValuePair<string, string>(possibleHeading, possibleMapping?.FieldName));
                                    }
                                    else
                                    {
                                        var value = csv.GetField(column);

                                        // ReSharper disable once PossibleNullReferenceException
                                        confirmFieldsModel.FirstTwoRowsWithGuessedMappings.Add(new Tuple<string, string, string>(items[column].Key, value, items[column].Value));
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                _logger.Exception("ModifyListSurfaceController", "ShowConfirmFiledsForm", ex);
                                throw;
                            }

                            rows++;
                        }
                    }
                }
            }

            return Request.CreateResponse(created ? HttpStatusCode.Created : HttpStatusCode.OK, confirmFieldsModel);
        }
    }
}
