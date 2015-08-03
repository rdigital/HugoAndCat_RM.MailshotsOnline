using NUnit.Framework;
using RM.MailshotsOnline.Business.Processors;
using RM.MailshotsOnline.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.MailshotsOnline.Test.Processors
{
    public class MailshotsProcessorTest
    {
        private MailshotsProcessor _processor;

        [SetUp]
        public void SetupMailshotsProcessor()
        {
            _processor = new MailshotsProcessor();
        }

        [TearDown]
        public void TearDown()
        {
            _processor = null;
        }

        [Test]
        public void GetXmlAndXslTest()
        {
            string input = File.ReadAllText("editorcontent.json");
            var mailshot = new Mailshot()
            {
                TemplateId = 1,
                FormatId = 1,
                ThemeId = 1,
                UserId = 1,
                Draft = true
            };
            mailshot.Content = new MailshotContent();
            mailshot.Content.Content = input;

            var output = _processor.GetXmlAndXslForMailshot(mailshot);

            Assert.NotNull(output);
            Assert.NotNull(output.Item1);
            Assert.NotNull(output.Item2);
        }
    }
}
