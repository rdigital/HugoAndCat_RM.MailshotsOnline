using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HC.RM.Common;
using HC.RM.Common.PCL.Helpers;
using umbraco;
using umbraco.MacroEngines;
using umbraco.NodeFactory;
using Umbraco.Web;
using Umbraco.Web.Editors;
using Umbraco.Web.WebApi;
using RM.MailshotsOnline.Data.Extensions;
using Newtonsoft.Json;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using IMember = RM.MailshotsOnline.PCL.Models.IMember;

namespace RM.MailshotsOnline.Web.Controllers.Api
{
    [Umbraco.Web.Mvc.PluginController("MemberSearch")]
    public class MemberSearchController : UmbracoAuthorizedApiController
    {
        private static ILogger _logger;

        public MemberSearchController(ILogger logger)
        {
            _logger = logger;
        }

        public IEnumerable<MemberResult> GetSearchResults(string query)
        {
            _logger.Info(GetType().Name, "GetSearchResults", $"Searching membership service for {query}");

            query = query.ToLower();

            var results =
                UmbracoContext.Application.Services.MemberService.GetMembersByMemberType("Member")
                    .Select(x => x.ToMemberEntityModel())
                    .Where(
                        x =>
                            x.FirstName.ToLower().Contains(query) ||
                            x.LastName.ToLower().Contains(query) ||
                            x.EmailAddress.ToLower().Contains(query))
                    .Select(
                        x =>
                            new MemberResult()
                            {
                                EmailAddress = x.EmailAddress,
                                FirstName = x.FirstName,
                                LastName = x.LastName,
                                NodeId = GetNodeId(x)
                            })
                    .OrderBy(x => x.EmailAddress);

            return results;
        }

        private string GetNodeId(IMember member)
        {
            Guid g;
            try
            {
                g =
                    UmbracoContext.Application.Services.EntityService.GetKeyForId(member.Id, UmbracoObjectTypes.Member)
                        .Result;
            }
            catch(Exception e)
            {
                _logger.Error(this.GetType().ToString(), "GetNodeId", $"Unable to get node using Node ID for member {member.EmailAddress}: " + e.Message);
                return "#";
            }

            return g.ToString("N");
        }
    }

    public class MemberResult
    {
        public string EmailAddress { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string NodeId { get; set; }
    }
}
