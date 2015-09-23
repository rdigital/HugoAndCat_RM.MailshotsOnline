namespace RM.MailshotsOnline.PCL.Models.Reporting
{
    public interface IMembershipReportEntity
    {
        string UserReference { get; set; }

        string EmailAddress { get; set; }

        string Title { get; set; }

        string FirstName { get; set; }

        string LastName { get; set; }

        string OrgName { get; set; }

        string OrgPosition { get; set; }

        string OrgFlatNumber { get; set; }

        string OrgBuildingNumber { get; set; }

        string OrgBuildingName { get; set; }

        string OrgAddress1 { get; set; }

        string OrgAddress2 { get; set; }

        string OrgTown { get; set; }

        string OrgPostcode { get; set; }

        string OrgCountry { get; set; }

        string OrgPhone { get; set; }

        string OrgMobile { get; set; }

        string RoyalMailContactPost { get; set; }

        string RoyalMailContactEmail { get; set; }

        string RoyalMailContactPhone { get; set; }

        string RoyalMailContactSms { get; set; }

        string ThirdPartyContactPost { get; set; }

        string ThirdPartyContactEmail { get; set; }

        string ThirdPartyContactPhone { get; set; }

        string ThirdPartyContactSms { get; set; }

        string Disabled { get; set; }

        string Updated { get; set; }
    }
}
