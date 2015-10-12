﻿using Glass.Mapper.Umb;
using HC.RM.Common.PCL.Helpers;
using RM.MailshotsOnline.Entities.PageModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Web.Mvc;
using RM.MailshotsOnline.Business.Processors;
using RM.MailshotsOnline.PCL;
using RM.MailshotsOnline.PCL.Models;
using RM.MailshotsOnline.PCL.Services;
using RM.MailshotsOnline.Web.Models;
using Umbraco.Web.Models;

namespace RM.MailshotsOnline.Web.Controllers
{
    [Authorize]
    public class ListDetailController : GlassController
    {
        private readonly IDataService _dataService;
        private readonly IMembershipService _membershipService;
        private readonly DistributionListProcessor _listProcessor;


        public ListDetailController(IUmbracoService umbracoService, ILogger logger, IDataService dataService, IMembershipService membershipService)
            : base(umbracoService, logger)
        {
            _membershipService = membershipService;
            _dataService = dataService;
            _listProcessor = new DistributionListProcessor(_logger);
        }

        // GET: ListDetail
        public ActionResult Index(RenderModel model, Guid distributionListId)
        {
            var loggedInMember = _membershipService.GetCurrentMember();

            var list = validateDistributionListId(loggedInMember, distributionListId);

            if (list == null)
            {
                return HttpNotFound("No distribution list found for that user.");
            }

            // Fetch the Glass model of the page
            var pageModel = GetModel<ListDetail>();

            pageModel.DistributionListId = distributionListId;

            return View("~/Views/ListDetail.cshtml", pageModel);
        }

        public ActionResult ListDetailDownload(RenderModel model, Guid distributionListId, Enums.DistributionListFileType fileType = Enums.DistributionListFileType.Final)
        {
            var loggedInMember = _membershipService.GetCurrentMember();

            var list = validateDistributionListId(loggedInMember, distributionListId);

            if (list == null)
            {
                return HttpNotFound("No distribution list found for that user.");
            }

            List<DistributionContact> contacts = null;
            string fileName = string.Empty;

            switch (fileType)
            {
                case Enums.DistributionListFileType.Final:
                    contacts = _dataService.GetFinalContacts<DistributionContact>(list);
                    fileName = list.BlobFinal;
                    break;
                case Enums.DistributionListFileType.Errors:
                    var csm = _dataService.CreateSummaryModel<DistributionContact>(list);
                    contacts = csm.InvalidContacts.ToList();
                    fileName = list.BlobErrors;
                    break;
            }

            if (contacts != null && contacts.Any())
            {
                fileName = Path.GetFileNameWithoutExtension(fileName);

                var cd = new ContentDisposition
                         {
                             FileName = fileName + ".csv",
                             Inline = false,
                         };

                byte[] contactsCsv = _listProcessor.BuildCsvFromContacts(contacts);

                Response.AppendHeader("Content-Disposition", cd.ToString());
                return File(contactsCsv, Constants.MimeTypes.Csv);
            }

            return new HttpNotFoundResult("No contacts of that type found on the list.");
        }

        public ActionResult ListDetailEmpty(RenderModel model)
        {
            // Fetch the Glass model of the page
            var pageModel = GetModel<ListDetail>();

            return View("~/Views/ListDetailEmpty.cshtml", pageModel);
        }

        private IDistributionList validateDistributionListId(IMember loggedInMember, Guid distributionListId)
        {
            if (distributionListId == Guid.Empty)
            {
                return null;
            }

            return _dataService.GetDistributionListForUser(loggedInMember.Id, distributionListId);
        }

    }
}