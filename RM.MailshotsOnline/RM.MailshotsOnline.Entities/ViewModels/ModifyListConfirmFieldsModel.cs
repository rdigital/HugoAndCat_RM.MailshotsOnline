using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using RM.MailshotsOnline.Entities.PageModels;

namespace RM.MailshotsOnline.Entities.ViewModels
{
    public class ModifyListConfirmFieldsModel
    {
        public Guid DistributionListId { get; set; }

        public string ListName { get; set; }

        [Required]
        public bool? FirstRowIsHeader { get; set; }

        public int ColumnCount { get; set; }

        [Required]
        public List<string> Mappings { get; set; }

        public Dictionary<string, string> MappingOptions { get; set; } 

        public ListCreate PageModel { get; set; }

        public List<Tuple<string, string, string>> FirstTwoRowsWithGuessedMappings { get; set; }
    }
}
