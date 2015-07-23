namespace RM.MailshotsOnline.Entities.MemberModels
{
    public interface IContactable
    {
        ContactPreferences RoyalMailMarketingPreferences { get; set; }

        ContactPreferences ThirdPartyMarketingPreferencess { get; set; }
    }
}