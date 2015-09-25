using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HC.RM.Common;
using HC.RM.Common.PCL.Helpers;
using RM.MailshotsOnline.Data.Services;
using RM.MailshotsOnline.PCL.Models;
using RM.MailshotsOnline.PCL.Services;
using umbraco;
using umbraco.MacroEngines;
using umbraco.NodeFactory;
using Umbraco.Web;
using Umbraco.Web.Editors;
using Umbraco.Web.WebApi;
using RM.MailshotsOnline.Data.Constants;
using RM.MailshotsOnline.Data.Extensions;
using Umbraco.Core.Models;

namespace RM.MailshotsOnline.Web.Controllers.Api
{
    [Umbraco.Web.Mvc.PluginController("Cryptography")]
    public class CryptoApiController : UmbracoAuthorizedJsonController
    {
        private static ILogger _logger;
        private static ICryptographicService _cryptographicService;

        public CryptoApiController(ILogger logger, ICryptographicService cryptographicService)
        {
            _logger = logger;
            _cryptographicService = cryptographicService;
        }

        /// <summary>
        /// Decrypt the given text using the member that resides at the given nodeId. The nodeId is used to obtain the member
        /// and thus the cryptographic salt.
        /// </summary>
        /// <param name="text">The input text</param>
        /// <param name="nodeId">The node ID representing the member entity</param>
        /// <returns></returns>
        public string GetDecrypted(string text, string nodeId)
        {
            // the b64 charset contains '/', which is replaced on the front end
            // with '@', so we have to convert them back.
            text = text.Replace('@', '/');

            // get node using nodeId
            var member = GetEntityByNodeId(nodeId);

            if (member == null)
            {
                return "Could not decrypt!";
            }

            return Decrypt(text, member);
        }

        public string GetDecryptedTitle(string id, string nodeId)
        {
            var member = GetEntityByNodeId(nodeId);

            if (member == null)
            {
                return "Could not decrypt!";
            }

            var titleId = Decrypt(id, member);

            try
            {
                return ApplicationContext.Services.DataTypeService.GetPreValues("Title Dropdown").FirstOrDefault(x => x.Key.Equals(titleId)).Value;
            }
            catch (Exception e)
            {
                _logger.Error(this.GetType().Name, "GetDecryptedTitle", $"Could not retrieve title value: {e.Message}",
                    new { id, nodeId });

                return "Could not decrypt!";
            }
        }

        /// <summary>
        /// Decrypt the text using the salt from the given member.
        /// </summary>
        /// <param name="text">The text</param>
        /// <param name="member">The member</param>
        /// <returns></returns>
        private string Decrypt(string text, IPublishedContent member)
        {
            if (member == null)
            {
                return "Could not decrypt!";
            }

            try
            {
                var salt = member.GetPropertyValue<string>("salt");

                return _cryptographicService.Decrypt(text, salt);
            }
            catch (Exception e)
            {
                _logger.Error(this.GetType().Name, "Decrypt", $"Could not decrypt input text: {e.Message}",
                    new { text, member.Id });

                return "Could not decrypt!";
            }
        }

        /// <summary>
        /// Retrieve the umbraco entity using the provided node ID.
        /// </summary>
        /// <param name="nodeId">The node ID</param>
        /// <returns>The entity</returns>
        private IPublishedContent GetEntityByNodeId(string nodeId)
        {
            var node = ApplicationContext.Services.EntityService.GetByKey(new Guid(nodeId));

            if (node == null)
            {
                return null;
            }

            var member = Members.GetByEmail(node.Name);

            return member;
        }

    }
}
