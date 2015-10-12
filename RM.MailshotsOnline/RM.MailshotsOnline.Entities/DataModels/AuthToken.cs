using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RM.MailshotsOnline.PCL.Models;

namespace RM.MailshotsOnline.Entities.DataModels
{
    public class AuthToken : IAuthToken
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid AuthTokenId { get; set; }

        public DateTime CreatedUtc { get; set; }

        [MaxLength(256)]
        public string ServiceName { get; set; }
    }
}
