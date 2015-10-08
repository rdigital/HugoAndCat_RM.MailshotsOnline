using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RM.MailshotsOnline.Entities.DataModels.Reports;
using RM.MailshotsOnline.PCL.Models.Reporting;
using RM.MailshotsOnline.PCL.Services;
using RM.MailshotsOnline.PCL.Services.Reporting;

namespace RM.MailshotsOnline.Data.Services.Reporting
{
    public class MembershipReportGenerator : IMembershipReportGenerator
    {
        private readonly IMembershipService _membershipService;

        public MembershipReportGenerator(IMembershipService membershipService)
        {
            _membershipService = membershipService;
        }

        public IMembershipReport Generate()
        {
            var members = _membershipService.GetAllActiveMembers().Select(x => new MembershipReportEntity()
            {
                UserReference = x.Username.ToString(),
                EmailAddress = x.EmailAddress,
                Title = x.Title,
                FirstName = x.FirstName,
                LastName = x.LastName,
                OrgName = x.OrganisationName,
                OrgPosition = x.JobTitle,
                OrgFlatNumber = x.FlatNumber,
                OrgBuildingNumber = x.BuildingNumber,
                OrgBuildingName = x.BuildingNumber,
                OrgAddress1 = x.Address1,
                OrgAddress2 = x.Address2,
                OrgTown = x.City,
                OrgPostcode = x.Postcode,
                OrgCountry = x.Country,
                OrgPhone = x.WorkPhoneNumber,
                OrgMobile = x.MobilePhoneNumber,
                RoyalMailContactPost = x.RoyalMailMarketingPreferences.Post.ToString(),
                RoyalMailContactEmail = x.RoyalMailMarketingPreferences.Email.ToString(),
                RoyalMailContactPhone = x.RoyalMailMarketingPreferences.Phone.ToString(),
                RoyalMailContactSms = x.RoyalMailMarketingPreferences.SmsAndOther.ToString(),
                ThirdPartyContactPost = x.ThirdPartyMarketingPreferences.Post.ToString(),
                ThirdPartyContactEmail = x.ThirdPartyMarketingPreferences.Email.ToString(),
                ThirdPartyContactPhone = x.ThirdPartyMarketingPreferences.Phone.ToString(),
                ThirdPartyContactSms = x.ThirdPartyMarketingPreferences.SmsAndOther.ToString(),
                Disabled = (!x.IsApproved).ToString(),
                Updated = x.Updated.ToString("dd/mm/yyyy hh:mm"),
            });

            var report = new MembershipReport {CreatedDate = DateTime.UtcNow, Members = members};

            return report;
        }
    }
}
