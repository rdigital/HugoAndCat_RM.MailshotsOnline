using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RM.MailshotsOnline.Entities.PageModels;
using RM.MailshotsOnline.Entities.PageModels.Settings;

namespace RM.MailshotsOnline.Entities.ViewModels
{
    public class ModifyListConfirmFieldsModel
    {
        public Guid DistributionListId { get; set; }
        [Required]
        public string ListName { get; set; }

        public ListCreate PageModel { get; set; }

        [Required]
        public bool? FirstRowIsHeader { get; set; }

        public List<Tuple<string, string, string>> FirstTwoRowsWithGuessedMappings { get; set; }
    }
}
