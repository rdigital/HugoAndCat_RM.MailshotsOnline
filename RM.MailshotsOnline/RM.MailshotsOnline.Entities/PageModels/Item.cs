using Glass.Mapper.Umb.Configuration;
using Glass.Mapper.Umb.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RM.MailshotsOnline.PCL.Models;

namespace RM.MailshotsOnline.Entities.PageModels
{
    /// <summary>
    /// Base Umbraco content item
    /// </summary>
    [UmbracoType(AutoMap = true)]
    public class Item : IItem
    {
        /// <summary>
        /// Page ID
        /// </summary>
        [UmbracoId]
        public virtual int Id { get; set; }

        /// <summary>
        /// Document Type name
        /// </summary>
        [UmbracoInfo(UmbracoInfoType.ContentTypeName)]
        public virtual string DocumentType { get; set; }

        /// <summary>
        /// Page name
        /// </summary>
        [UmbracoInfo(UmbracoInfoType.Name)]
        public virtual string Name { get; set; }

        /// <summary>
        /// Creator
        /// </summary>
        [UmbracoInfo(UmbracoInfoType.Creator)]
        public virtual string Creator { get; set; }

        /// <summary>
        /// Item path
        /// </summary>
        [UmbracoInfo(UmbracoInfoType.Path)]
        public virtual string Path { get; set; }

        /// <summary>
        /// Created date
        /// </summary>
        [UmbracoInfo(UmbracoInfoType.CreateDate)]
        public virtual DateTime CreatedDate { get; set; }

        /// <summary>
        /// Collection of child items
        /// </summary>
        [UmbracoChildren(InferType = true)]
        public virtual IEnumerable<IItem> Children { get; set; }

    }
}
