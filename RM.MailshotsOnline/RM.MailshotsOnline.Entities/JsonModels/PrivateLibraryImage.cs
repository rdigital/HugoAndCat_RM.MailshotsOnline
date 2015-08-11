using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RM.MailshotsOnline.Entities.MemberModels;

namespace RM.MailshotsOnline.Entities.JsonModels
{
    public class PrivateLibraryImage : Image
    {
        public string Username { get; set; }
    }
}
