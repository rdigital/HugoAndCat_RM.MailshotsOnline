using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace RM.MailshotsOnline.Entities.ViewModels
{
    public class ErrorViewModel
    {
        [JsonProperty(PropertyName = "error")]
        public string Error { get; set; }

        [JsonProperty(PropertyName = "fieldErrors")]
        public List<string> FieldErrors { get; set; }
    }
}
