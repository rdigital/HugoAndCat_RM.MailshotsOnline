﻿using System;

namespace RM.MailshotsOnline.Entities.ViewModels
{
    public class ModifyListFinishModel
    {
        public Guid DistributionListId { get; set; }

        public string ListName { get; set; }

        public string Command { get; set; }
    }
}
