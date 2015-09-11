using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Mvc;
using Umbraco.Core;
using Umbraco.Web;
using Umbraco.Web.Models.Trees;
using Umbraco.Web.Mvc;
using Umbraco.Web.Trees;

namespace RM.MailshotsOnline.Web.Controllers.BackOffice
{
    [Tree("member", "membersDecrypted", "Members (Decrypted)")]
    [PluginController("MembersDecrypted")]
    public class DecryptedMemberListController : MemberTreeController
    {
        protected override TreeNodeCollection GetTreeNodes(string id, FormDataCollection queryStrings)
        {
            var collection = base.GetTreeNodes(id, queryStrings);

            return collection;
        }

        protected override MenuItemCollection GetMenuForNode(string id, FormDataCollection queryStrings)
        {
            return base.GetMenuForNode(id, queryStrings);
        }
    }
}