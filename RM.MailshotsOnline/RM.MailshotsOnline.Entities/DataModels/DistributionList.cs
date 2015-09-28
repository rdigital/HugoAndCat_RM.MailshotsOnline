﻿using RM.MailshotsOnline.PCL.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using Newtonsoft.Json;

namespace RM.MailshotsOnline.Entities.DataModels
{
    [Table("DistributionLists")]
    public class DistributionList : IDistributionList
    {
        public DistributionList()
        {
            fillSalt();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid DistributionListId { get; set; }

        public string Name { get; set; }

        [JsonIgnore]
        public int UserId { get; set; }

        public DateTime CreatedDate => DateTime.SpecifyKind(CreatedUtc, DateTimeKind.Utc);

        [Required, DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [JsonIgnore]
        public DateTime CreatedUtc { get; private set; }

        [JsonIgnore]
        public string BlobFinal { get; set; }

        [JsonIgnore]
        public string BlobWorking { get; set; }

        [JsonIgnore]
        public string BlobErrors { get; set; }

        public int RecordCount { get; set; }

        public DateTime UpdatedDate { get; set; }

        [JsonIgnore]
        public byte[] DataSalt { get; private set; }

        private void fillSalt()
        {
            var rngCsp = new RNGCryptoServiceProvider();
            var salt = new byte[16];
            rngCsp.GetBytes(salt);

            DataSalt = salt;
        }
    }
}
