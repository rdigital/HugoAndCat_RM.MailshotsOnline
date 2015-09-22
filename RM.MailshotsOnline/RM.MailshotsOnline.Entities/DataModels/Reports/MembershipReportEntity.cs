using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.MailshotsOnline.Entities.DataModels.Reports
{
    public class MembershipReportEntity
    {
        public string UserReference { get; set; }

        public string EmailAddress { get; set; }

        public string Title { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string OrgName { get; set; }

        public string OrgPosition { get; set; }

        public string OrgFlatNumber { get; set; }

        public string OrgBuildingNumber { get; set; }

        public string OrgBuildingName { get; set; }

        public string OrgAddress1 { get; set; }

        public string OrgAddress2 { get; set; }

        public string OrgTown { get; set; }

        public string OrgPostcode { get; set; }

        public string OrgCountry { get; set; }

        public string OrgPhone { get; set; }

        public string OrgMobile { get; set; }

        public string RoyalMailContactPost { get; set; }

        public string RoyalMailContactEmail { get; set; }

        public string RoyalMailContactPhone { get; set; }

        public string RoyalMailContactSms { get; set; }

        public string ThirdPartyContactPost { get; set; }

        public string ThirdPartyContactEmail { get; set; }

        public string ThirdPartyContactPhone { get; set; }

        public string ThirdPartyContactSms { get; set; }

        public string Disabled { get; set; }

        public string Updated { get; set; }
    }
}
