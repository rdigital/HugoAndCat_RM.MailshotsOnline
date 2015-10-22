using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.MailshotsOnline.Entities.JsonModels
{
    public class ReportGenerationParameters
    {
        public string Type { get; set; }

        public string Token { get; set; }

        public string Service { get; set; }

        public DateTime start { get; set; }

        public DateTime end { get; set; }
    }
}
