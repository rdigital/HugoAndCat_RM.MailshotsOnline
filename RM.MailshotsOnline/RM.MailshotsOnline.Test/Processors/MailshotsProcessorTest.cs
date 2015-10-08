using NUnit.Framework;
using RM.MailshotsOnline.Business.Processors;
using RM.MailshotsOnline.Entities.DataModels;
using RM.MailshotsOnline.Entities.DataModels.MailshotSettings;
using RM.MailshotsOnline.Entities.ViewModels;
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
            var output = GetMailshotsOutput();

            Assert.NotNull(output);
            Assert.NotNull(output.XmlData);
            Assert.NotNull(output.XslStylesheet);
        }

        [Test]
        public void MailshotsProcessorOutputIsValid()
        {
            var output = GetMailshotsOutput();

            // Check that XML is valid
            var xml = output.XmlData;
            XDocument xmlDoc = XDocument.Parse(xml);
            Assert.NotNull(xmlDoc);
            Assert.NotNull(xmlDoc.Root);

            // Check that the XSL transforms the XML
            var xmlStringReader = new StringReader(xml);
            var xmlReader = XmlReader.Create(xmlStringReader);
            var xsl = output.XslStylesheet;
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

        private XmlAndXslData GetMailshotsOutput()
        {
            string input = File.ReadAllText("newcontent.json");
            var format = new Format()
            {
                Name = "A4 Page",
                XslData = @"<!--A4 Portrait-->
        <fo:simple-page-master master-name=""FrontMaster"" page-width=""210mm"" page-height=""297mm"">
          <fo:region-body region-name=""front"" />
        </fo:simple-page-master>
        <fo:simple-page-master master-name=""BackMaster"" page-width=""210mm"" page-height=""297mm"">
          <fo:region-body region-name=""back"" />
        </fo:simple-page-master>

        <fo:page-sequence-master master-name=""document"">
          <fo:repeatable-page-master-alternatives>
            <fo:conditional-page-master-reference page-position=""any"" odd-or-even=""odd"" master-reference=""FrontMaster"" />
            <fo:conditional-page-master-reference page-position=""any"" odd-or-even=""even"" master-reference=""BackMaster"" />
          </fo:repeatable-page-master-alternatives>
        </fo:page-sequence-master>"
            };

            var template = new Template()
            {
                Name = "A4 Template 1",
                XslData = @"<!-- page templates-->
  <xsl:template name=""pageContentFront"">
    <!-- Header -->
    <fo:block-container position=""fixed"" top=""0mm"" left=""0mm"" height=""18mm"" width=""100%"" display-align=""center"" text-align=""center"" letter-spacing=""-0.02em"">
      <xsl:attribute name=""background-color"">
        <xsl:value-of select=""$headerBackCol""/>
      </xsl:attribute>
      <xsl:attribute name=""font-family"">
        <xsl:value-of select=""$headerFontFamily""/>
      </xsl:attribute>
      <xsl:attribute name=""font-size"">
        <xsl:value-of select=""$headerTextSize""/>
      </xsl:attribute>
      <xsl:attribute name=""line-height"">
        <xsl:value-of select=""$headerTextSize""/>
      </xsl:attribute>
      <xsl:attribute name=""color"">
        <xsl:value-of select=""$headerTextCol""/>
      </xsl:attribute>
      <fo:block margin-left=""0mm"" margin-right=""0mm"" margin-top=""0mm"" margin-bottom=""0mm""><xsl:copy-of select=""heading/node()""/></fo:block>
    </fo:block-container>
    
    <!-- Body content -->
    <fo:block-container position=""fixed"" top=""27mm"" left=""0mm"" height=""86mm"" width=""100%"" display-align=""before"" text-align=""left"" letter-spacing=""0em"" linefeed-treatment=""preserve"">
      <xsl:attribute name=""background-color"">
        <xsl:value-of select=""$bodyBackCol""/>
      </xsl:attribute>
      <xsl:attribute name=""font-family"">
        <xsl:value-of select=""$bodyFontFamily""/>
      </xsl:attribute>
      <xsl:attribute name=""font-size"">
        <xsl:value-of select=""$bodyTextSize""/>
      </xsl:attribute>
      <xsl:attribute name=""line-height"">
        <xsl:value-of select=""$bodyTextSize""/>
      </xsl:attribute>
      <xsl:attribute name=""color"">
        <xsl:value-of select=""$bodyTextCol""/>
      </xsl:attribute>
      <fo:block margin-left=""6mm"" margin-right=""6mm"" margin-top=""8mm"" margin-bottom=""8mm""><xsl:copy-of select=""body/node()""/></fo:block>
    </fo:block-container>
    
    <!-- Image -->
    <fo:block-container position=""fixed"" bottom=""13mm"" right=""0mm"" height=""20mm"" width=""100%"" text-align=""center"" display-align=""after"">
      <fo:block>
        <fo:external-graphic scaling=""uniform"" content-height=""20mm"">
          <xsl:attribute name=""src"">url('<xsl:value-of select=""logo""/>')</xsl:attribute>
        </fo:external-graphic>
      </fo:block>
    </fo:block-container>
  </xsl:template>

  <xsl:template name=""pageContentBack"">
    <!-- Body content (on back)-->
    <fo:block-container position=""fixed"" top=""33mm"" left=""0mm"" bottom=""178mm"" right=""117mm"" display-align=""before"" text-align=""left"" letter-spacing=""0em"" linefeed-treatment=""preserve"">
      <xsl:attribute name=""background-color"">
        <xsl:value-of select=""$bodyBackCol""/>
      </xsl:attribute>
      <xsl:attribute name=""font-family"">
        <xsl:value-of select=""$bodyFontFamily""/>
      </xsl:attribute>
      <xsl:attribute name=""font-size"">
        <xsl:value-of select=""$bodyTextSize""/>
      </xsl:attribute>
      <xsl:attribute name=""line-height"">
        <xsl:value-of select=""$bodyTextSize""/>
      </xsl:attribute>
      <xsl:attribute name=""color"">
        <xsl:value-of select=""$bodyTextCol""/>
      </xsl:attribute>
      <fo:block margin-left=""6mm"" margin-right=""6mm"" margin-top=""8mm"" margin-bottom=""8mm"">
        <xsl:copy-of select=""body2/node()""/>
      </fo:block>
    </fo:block-container>
    
    <!-- Logo on back-->
    <fo:block-container position=""fixed"" bottom=""13mm"" right=""0mm"" height=""20mm"" width=""115mm"" text-align=""center"" display-align=""after"">
      <fo:block>
        <fo:external-graphic scaling=""uniform"" content-height=""20mm"">
          <xsl:attribute name=""src"">
            url('<xsl:value-of select=""logo""/>')
          </xsl:attribute>
        </fo:external-graphic>
      </fo:block>
    </fo:block-container>
  </xsl:template>"
            };

            var theme = new Theme()
            {
                Name = "Test theme",
                XslData = @"<!-- Header colours and text settings -->
  <xsl:variable name=""headerBackCol"" select=""'orange'"" />
  <xsl:variable name=""headerTextCol"" select=""'black'"" />
  <xsl:variable name=""headerTextSize"" select=""'40pt'"" />
  <xsl:variable name=""headerFontFamily"" select=""'Helvetica'"" />

  <!-- Text area colours and text settings -->
  <xsl:variable name=""bodyBackCol"" select=""'black'"" />
  <xsl:variable name=""bodyTextCol"" select=""'white'"" />
  <xsl:variable name=""bodyTextSize"" select=""'12pt'"" />
  <xsl:variable name=""bodyFontFamily"" select=""'Helvetica'"" />"
            };

            var mailshot = new Mailshot()
            {
                Format = format,
                Template = template,
                Theme = theme,
                UserId = 1,
                Draft = true
            };
            mailshot.Content = new MailshotContent();
            mailshot.Content.Content = input;

            //string xsl = File.ReadAllText("A4PageComplete.xsl");
            //var output = _processor.GetXmlAndXslForMailshot(mailshot, xsl);

            var output = _processor.GetXmlAndXslForMailshot(mailshot);
            return output;
        }
    }
}
