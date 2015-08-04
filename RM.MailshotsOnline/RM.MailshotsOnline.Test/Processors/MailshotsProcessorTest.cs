using NUnit.Framework;
using RM.MailshotsOnline.Business.Processors;
using RM.MailshotsOnline.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Xml.Xsl;

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
        public void MailshotsProcessorReturnsOutput()
        {
            var output = GetMailshotsOutout();

            Assert.NotNull(output);
            Assert.NotNull(output.Item1);
            Assert.NotNull(output.Item2);
        }

        [Test]
        public void MailshotsProcessorOutputIsValid()
        {
            var output = GetMailshotsOutout();

            // Check that XML is valid
            var xml = output.Item1;
            XDocument xmlDoc = XDocument.Parse(xml);
            Assert.NotNull(xmlDoc);
            Assert.NotNull(xmlDoc.Root);

            // Check that the XSL transforms the XML
            var xmlStringReader = new StringReader(xml);
            var xmlReader = XmlReader.Create(xmlStringReader);
            var xsl = output.Item2;
            var xslStringReader = new StringReader(xsl);
            var xslReader = XmlReader.Create(xslStringReader);
            XslCompiledTransform xslTransform = new XslCompiledTransform();
            xslTransform.Load(xslReader);
            StringBuilder transformBuilder = new StringBuilder();
            XmlWriter writer = XmlWriter.Create(transformBuilder, xslTransform.OutputSettings);
            xslTransform.Transform(xmlReader, writer);
            writer.Close();
            Assert.Greater(transformBuilder.Length, 0);
        }

        private Tuple<string, string> GetMailshotsOutout()
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

            string xsl = File.ReadAllText("A4PageComplete.xsl");

            var output = _processor.GetXmlAndXslForMailshot(mailshot, xsl);
            return output;
        }
    }
}
