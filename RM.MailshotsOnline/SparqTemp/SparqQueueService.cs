using RM.MailshotsOnline.PCL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RM.MailshotsOnline.PCL.Models;
using RM.MailshotsOnline.Business.Processors;
using System.Web;
using System.Net;
using System.Net.Http;
using SparqRenderJob.Models;
using System.IO;
using System.Threading;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace SparqTemp
{
    public class SparqQueueService : ISparqQueueService
    {
        private IMailshotsService _mailshotsService;

        public SparqQueueService(IMailshotsService mailshotsService)
        {
            _mailshotsService = mailshotsService;
        }

        public async Task<bool> SendRenderAndPrintJob(IMailshot mailshot)
        {
            throw new NotImplementedException();
        }

        [Obsolete("This is only a test implementation and shouldn't really be used.")]
        public async Task<bool> SendRenderJob(IMailshot mailshot)
        {
            var success = false;
            var mailshotProcessor = new MailshotsProcessor();
            var content = mailshotProcessor.GetXmlAndXslForMailshot(mailshot);

            //TODO: Throw all of this away - will be replaced with the queue!
            var sparqServer = CloudConfigurationManager.GetSetting("SparqServiceUrl");
            var sparqUsername = CloudConfigurationManager.GetSetting("SparqServiceUsername");
            var sparqPassword = CloudConfigurationManager.GetSetting("SparqServicePassword");
            var sparqRenderer = new SparqRenderJob.SparqRenderJob(sparqServer, sparqUsername, sparqPassword);
            SparqJob job = new SparqJob() { priority = SparqRenderJob.Common.Enums.SparqPriorityEnum.MEDIUM, systemId = "http://www.hugoandcat.com/data/" };

            var startedJob = sparqRenderer.SubmitRenderJob(job, Encoding.UTF8.GetBytes(content.Item1), Encoding.UTF8.GetBytes(content.Item2));
            var jobId = startedJob.key;

            mailshot.ProofPdfStatus = RM.MailshotsOnline.PCL.Enums.PdfRenderStatus.Pending;
            _mailshotsService.SaveMailshot(mailshot);

            if (JobReady(jobId, sparqRenderer))
            {
                var pdfBytes = sparqRenderer.GetJobAsset(jobId, SparqRenderJob.Common.Enums.SparqAssetEnum.PDF);
                if (pdfBytes != null && pdfBytes.Length > 0)
                {
                    // Save PDF to blob storage
                    var blobAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("SparqBlobStorage"));
                    var containerName = CloudConfigurationManager.GetSetting("SparqBlobContainer");

                    // create Blob Client and return reference to the container
                    CloudBlobClient blobClient = blobAccount.CreateCloudBlobClient();

                    CloudBlobContainer container = blobClient.GetContainerReference(containerName);

                    // Check it exists before returning it:
                    if (container.CreateIfNotExists())
                    {
                        // Permissions can be set against the blob container:
                        var permissions = new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob };
                        container.SetPermissions(permissions);
                    }

                    var blobFilename = string.Format("{0}.pdf", mailshot.MailshotId.ToString("D"));
                    var blobMediaType = "application/pdf";

                    // Retrieve reference to a blob
                    var blob = container.GetBlockBlobReference(blobFilename);

                    // Set the blob content type
                    blob.Properties.ContentType = blobMediaType;

                    // Upload byte array into blob storage
                    using (var stream = new MemoryStream(pdfBytes, writable: false))
                    {
                        await blob.UploadFromStreamAsync(stream);
                    }

                    var pdfUrl = blob.Uri;

                    if (pdfUrl != null)
                    {
                        mailshot.ProofPdfUrl = pdfUrl.ToString();
                        mailshot.ProofPdfStatus = RM.MailshotsOnline.PCL.Enums.PdfRenderStatus.Complete;
                        success = true;
                    }
                    else
                    {
                        mailshot.ProofPdfStatus = RM.MailshotsOnline.PCL.Enums.PdfRenderStatus.Failed;
                        success = false;
                    }

                    _mailshotsService.SaveMailshot(mailshot);
                }
            }
            else
            {
                success = false;
            }

            #region Really low-fi way of doing things
            /*var requestContent = $@"--ThisIsATest
Content-Type: application/json
Content-Disposition: form-data; name=""job""

{{ ""created"":null,""assets"":null,""key"":null,""status"":null,""failedAttempts"":0,""groupId"":null,""systemId"":null, ""priority"":""MEDIUM"",""errorEntries"":null}}
--ThisIsATest
Content - Type: application / xml
Content - Disposition: form - data; filename = ""xsl_testing.xml""; name = ""xml""

{content.Item1}
--ThisIsATest
Content - Type: application / xml
Content - Disposition: form - data; filename = ""xsl_testing.xsl""; name = ""xsl""

{content.Item2}
--ThisIsATest--";

            // Send render job content
            using (var client = new HttpClient())
            {
                try
                {
                    var postContent = new StringContent(requestContent, Encoding.UTF8);
                    postContent.Headers.Add("Authorization", "Basic aHVnb2NhdDpzdHJpcGVkNjZSb2Jpbg==");
                    postContent.Headers.Add("Content-Type", "multipart/form-data; boundary=ThisIsATest");
                    var postResponse = await client.PostAsync("https://re-sparq.sp-systems.co.uk/sparq/job", postContent);
                    if (postResponse.IsSuccessStatusCode)
                    {
                        var guidString = await postResponse.Content.ReadAsStringAsync();

                        mailshot.ProofPdfStatus = RM.MailshotsOnline.PCL.Enums.PdfRenderStatus.Pending;
                        _mailshotsService.SaveMailshot(mailshot);

                        var getStatusResponse = await client.GetAsync($"https://re-sparq.sp-systems.co.uk/sparq/job/{guidString}");
                        var getStatusContent = await getStatusResponse.Content.ReadAsStringAsync();
                    }
                    else
                    {
                        //TODO: Capture more information
                        success = false;
                    }
                }
                catch
                {
                    //TODO: Capture errors and report on them properly
                    success = false;
                }
            }*/
            #endregion

            return success;
        }

        private bool JobReady(string jobId, SparqRenderJob.SparqRenderJob renderer)
        {
            Thread.Sleep(2500);
            var job = renderer.GetJob(jobId);
            if (job.status == SparqRenderJob.Common.Enums.SparqStatusEnum.Done)
            {
                return true;
            }
            else if (job.status == SparqRenderJob.Common.Enums.SparqStatusEnum.Deleted || job.status == SparqRenderJob.Common.Enums.SparqStatusEnum.Failed)
            {
                return false;
            }
            else
            {
                return JobReady(jobId, renderer);
            }
        }
    }
}
