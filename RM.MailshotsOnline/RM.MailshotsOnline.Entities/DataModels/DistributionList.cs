using RM.MailshotsOnline.PCL.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using Newtonsoft.Json;
using RM.MailshotsOnline.PCL;

namespace RM.MailshotsOnline.Entities.DataModels
{
    [Table("DistributionLists")]
    public class DistributionList : IDistributionList
    {
        Enums.DistributionListState _listStep = Enums.DistributionListState.Unknown;

        public DistributionList()
        {
            fillSalt();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid DistributionListId { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        [JsonIgnore]
        public int UserId { get; set; }

        public DateTime CreatedDate => DateTime.SpecifyKind(CreatedUtc, DateTimeKind.Utc);

        [Required, DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [JsonIgnore]
        public DateTime CreatedUtc { get; private set; }

        [StringLength(500)]
        [JsonIgnore]
        public string BlobFinal { get; set; }

        [StringLength(500)]
        [JsonIgnore]
        public string BlobWorking { get; set; }

        [StringLength(500)]
        [JsonIgnore]
        public string BlobErrors { get; set; }

        public int RecordCount { get; set; }

        public DateTime UpdatedDate { get; set; }

        [MaxLength(32)]
        [JsonIgnore]
        public byte[] DataSalt { get; private set; }

        [MaxLength(20)]
        [Column("ListState")]
        public string CurrentImportState { get; private set; }

        [NotMapped]
        [JsonIgnore]
        public Enums.DistributionListState ListState
        {
            get
            {
                if (_listStep == Enums.DistributionListState.Unknown)
                {
                    Enum.TryParse(CurrentImportState, out _listStep);
                }

                return _listStep;
            }
            set
            {
                _listStep = value;
                CurrentImportState = _listStep.ToString();
            }
        }

        private void fillSalt()
        {
            var rngCsp = new RNGCryptoServiceProvider();
            var salt = new byte[16];
            rngCsp.GetBytes(salt);

            DataSalt = salt;
        }
    }
}
