using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core;
using Umbraco.Web.Models.Trees;
using Umbraco.Web.Mvc;
using Umbraco.Web.Trees;

namespace RM.MailshotsOnline.Web.Controllers.BackOffice
{
    [Tree("member", "memberSearch", "Member Search")]
    [PluginController("CustomMember")]
    public class MemberSearchTreeController : TreeController
    {
        protected override TreeNodeCollection GetTreeNodes(string id, FormDataCollection queryStrings)
        {
            //check if we're rendering the root node's children
            if (id == Constants.System.Root.ToInvariantString())
            {
                return new TreeNodeCollection
                {
                    CreateTreeNode("Search", id, queryStrings, "Search", "icon-search")
                };
            }

            //this tree doesn't suport rendering more than 1 level
            throw new NotSupportedException();
        }

        protected override MenuItemCollection GetMenuForNode(string id, FormDataCollection queryStrings)
        {
            return null;
        }
    }
}
