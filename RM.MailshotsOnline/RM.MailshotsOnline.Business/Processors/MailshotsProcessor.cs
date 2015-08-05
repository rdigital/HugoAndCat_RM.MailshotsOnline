using Newtonsoft.Json;
using RM.MailshotsOnline.Entities.JsonModels;
using RM.MailshotsOnline.Entities.PageModels.Settings;
using RM.MailshotsOnline.PCL.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RM.MailshotsOnline.Business.Processors
{
    public class MailshotsProcessor
    {
        private XNamespace _foNamespace;

        public MailshotsProcessor()
        {
            _foNamespace = "http://www.w3.org/1999/XSL/Format";
        }

        /// <summary>
        /// Gets the XML and XSLT output for a mailshot to send to the SparQ service
        /// </summary>
        /// <param name="mailshot">Mailshot to be generated</param>
        /// <param name="xslOverride">Specify the XSL to use as an override, rather than generating XSL from the Mailshot format / template / theme</param>
        /// <returns>Item 1: XML content; Item 2: XSLT transform file</returns>
        public Tuple<string, string> GetXmlAndXslForMailshot(IMailshot mailshot, string xslOverride = null)
        {
            if (mailshot == null)
            {
                throw new ArgumentNullException("mailshot");
            }

            // Generate the XSL

            //TODO: Get this another way?
            /*var finalXsl = $@"<?xml version=""1.0"" encoding=""utf-8""?>
<xsl:stylesheet version=""1.0"" xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:fo=""http://www.w3.org/1999/XSL/Format"" xmlns:rx=""http://www.renderx.com/XSL/Extensions"">
  
  <xsl:output method=""xml"" version=""1.0"" encoding=""UTF-8"" />

  <!-- From Theme: -->
  {theme.XslData}

  <xsl:template match=""/page"">
    <!--This sets up pages, dimensions, backgrounds, headers and footers. Background images can also be applied within this section.-->
    <fo:root>
      <fo:layout-master-set>
        {format.XslData}

        <fo:page-sequence-master master-name=""document"">
          <fo:repeatable-page-master-alternatives>
            <fo:conditional-page-master-reference page-position=""any"" odd-or-even=""odd"" master-reference=""FrontMaster"" />
            <fo:conditional-page-master-reference page-position=""any"" odd-or-even=""even"" master-reference=""BackMaster"" />
          </fo:repeatable-page-master-alternatives>
        </fo:page-sequence-master>
        
      </fo:layout-master-set>

      <fo:page-sequence master-reference=""FrontMaster"">
        <fo:flow flow-name=""front"">
          <!--This is the template containing the main body layout.-->
          <xsl:call-template name=""pageContentFront"" />
        </fo:flow>
      </fo:page-sequence>
      <fo:page-sequence master-reference=""BackMaster"">
        <fo:flow flow-name=""back"">
          <xsl:call-template name=""pageContentBack"" />
        </fo:flow>
      </fo:page-sequence>
	  
    </fo:root>
  </xsl:template>

  <!-- page templates-->
  {layout.XslData}

</xsl:stylesheet>";*/

            // Generate the XML
            if (mailshot.Content == null || string.IsNullOrEmpty(mailshot.Content.Content))
            {
                throw new ArgumentException("Mailshot content cannot be null", "mailshot");
            }

            MailshotEditorContent content = JsonConvert.DeserializeObject<MailshotEditorContent>(mailshot.Content.Content);
            var finalXml = GetContentXmlFromJson(content);

            if (!string.IsNullOrEmpty(xslOverride))
            {
                return Tuple.Create<string, string>(finalXml, xslOverride);
            }

            //TODO: Compile the XSL properly
            var finalXsl = File.ReadAllText("C:\\Projects\\RoyalMail\\MSOL\\RM.MailshotsOnline\\RM.MailshotsOnline\\XML\\Formats\\A4PageComplete.xsl");
            return Tuple.Create<string, string>(finalXml, finalXsl);
        }

        /// <summary>
        /// Generates the appropriate XML content to send through to SparQ based on the editor content
        /// </summary>
        /// <param name="jsonContent">Parsed JSON content from the editor interface</param>
        /// <returns>XML string</returns>
        private string GetContentXmlFromJson(MailshotEditorContent jsonContent)
        {
            var contentXml = new XDocument();
            var ticket = new XElement("page", new XAttribute(XNamespace.Xmlns + "fo", _foNamespace));

            foreach (var jsonElement in jsonContent.Elements)
            {
                if (!string.IsNullOrEmpty(jsonElement.Content))
                {
                    var contentElement = new XElement(jsonElement.Name.Replace(" ", string.Empty));
                    if (!jsonElement.Content.StartsWith("data:image"))
                    {
                        contentElement.Add(ProcessHtmlContent(jsonElement.Content.Replace("<br>", "<br />")));
                    }
                    else
                    {
                        //contentElement.Value = jsonElement.Content;
                        contentElement.Add(new XCData(jsonElement.Content));
                    }

                    ticket.Add(contentElement);
                }
            }

            contentXml.Add(ticket);
            return contentXml.ToString(SaveOptions.None);
        }

        /// <summary>
        /// Processes the HTML from the editable divs of the editor and converts it into XSL:FO XML snippets
        /// </summary>
        /// <param name="htmlContent">HTML content from the editor</param>
        /// <returns>XSL:FO XML snippets</returns>
        private IEnumerable<XNode> ProcessHtmlContent(string htmlContent)
        {
            var htmlAsXml = XDocument.Parse(string.Format("<root>{0}</root>", htmlContent));

            var outputRoot = new XElement("root");

            foreach (XNode node in htmlAsXml.Root.Nodes())
            {
                // Do substitutions?
                var newElement = ProcessSubNode(node);
                // Add to outputRoot
                if (newElement != null)
                {
                    outputRoot.Add(newElement);
                }
            }

            return outputRoot.Nodes();
        }

        private XNode ProcessSubNode(XNode rootNode)
        {
            if (rootNode.NodeType == System.Xml.XmlNodeType.Text)
            {
                return rootNode;
            }
            else if (rootNode.NodeType == System.Xml.XmlNodeType.Element)
            {
                XElement rootElement = (XElement)rootNode;
                XElement returnElement;

                if (rootElement.Name == "div")
                {
                    returnElement = new XElement(_foNamespace + "block");
                }
                else if (rootElement.Name == "br")
                {
                    return new XText(Environment.NewLine);
                }
                else
                {
                    returnElement = new XElement(_foNamespace + "inline");
                }

                if (rootElement.Name == "span")
                {
                    var styleAttribute = rootElement.Attribute("style");
                    if (styleAttribute != null)
                    {
                        var styleValues = styleAttribute.Value.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string value in styleValues)
                        {
                            if (!string.IsNullOrWhiteSpace(value))
                            {
                                var valueParts = value.Split(new char[] { ':' });
                                if (valueParts.Length == 2)
                                {
                                    returnElement.SetAttributeValue(valueParts[0].Trim(), valueParts[1].Trim());
                                }
                            }
                        }
                    }
                }

                if (rootElement.Name == "b" || rootElement.Name == "strong")
                {
                    returnElement.SetAttributeValue("font-weight", "bold");
                }

                if (rootElement.Name == "em" || rootElement.Name == "i")
                {
                    returnElement.SetAttributeValue("font-style", "italic");
                }

                foreach (var subNode in rootElement.Nodes())
                {
                    returnElement.Add(ProcessSubNode(subNode));
                }

                return returnElement;
            }
            else
            {
                return null;
            }
        }
    }
}
