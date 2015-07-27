using HC.RM.Common.Azure;

[assembly: WebActivatorEx.PostApplicationStartMethod(typeof(RM.MailshotsOnline.Web.AzureWeb), "PostStart")]
namespace RM.MailshotsOnline.Web
{
    public static class AzureWeb
    {
		public static void PostStart()
		{
            TelemetryConfig.SetupTelemetry();
		}
    }
}