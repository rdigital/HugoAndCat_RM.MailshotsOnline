using HC.RM.Common.Azure;

[assembly: WebActivatorEx.PostApplicationStartMethod(typeof(RM.MailshotsOnline.WorkerRole.AzureWeb), "PostStart")]
namespace RM.MailshotsOnline.WorkerRole
{
    public static class AzureWeb
    {
		public static void PostStart()
		{
            TelemetryConfig.SetupTelemetry();
		}
    }
}