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

namespace RM.MailshotsOnline.Web.Controllers.Api
{
    [Umbraco.Web.Mvc.PluginController("Cryptography")]
    public class CryptoApiController : UmbracoAuthorizedJsonController
    {
        private static IMembershipService _membershipService;
        private static ILogger _logger;

        private static readonly string PrivateKey = ConfigurationManager.AppSettings["PrivateKey"];

        public CryptoApiController(IMembershipService membershipService, ILogger logger)
        {
            _membershipService = membershipService;
            _logger = logger;
        }

        /// <summary>
        /// Decrypt the given text using the member that resides at the given nodeId. The nodeId is used to obtain the member to which the encrypted
        /// text pertains, revealing the salt to be used for decryption.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="nodeId"></param>
        /// <returns></returns>
        public string GetDecrypted(string text, string nodeId)
        {
            // the b64 charset contains '/', which is replaced on the front end
            // with '@', so we have to convert them back.
            text = text.Replace('@', '/');

            // get node using nodeId ('uniqueid' in database)
            //var node = uQuery.GetNodesByXPath($"//*[string(uniqueid) = '{nodeId}'").FirstOrDefault();

            // get value of the 'text' property
            var email = "avEhulNMIZhlODcNp6qU6AnauPZZgFOZglUTHx3CqF6kaQOj5Aou0uAwATRPs7cJ";

            //look up that member to find his salt
            var member = Members.GetByEmail(email);

            if (member != null)
            {
                try
                {
                    var salt = member.GetPropertyValue("salt").ToString();
                    var saltBytes = Convert.FromBase64String(salt);

                    var decrypted = Encryption.Decrypt(text, PrivateKey, saltBytes);

                    return decrypted;
                }
                catch (Exception e)
                {
                    _logger.Error(this.GetType().Name, "GetDecrypted", $"Could not decrypt input text: {e.Message}",
                        new {text, nodeId});
                }
            }

            return "Could not decrypt!";
        }
    }
}
