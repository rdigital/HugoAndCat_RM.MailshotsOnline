using Newtonsoft.Json;
using RM.MailshotsOnline.Entities.JsonModels;
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

            //TODO: Get the correct XSL
            /*string formatXsl = null;
            switch (mailshot.FormatId)
            {
                case 1:
                    formatXsl = "XSL for Format 1 goes here";
                    break;
                default:
                    throw new ArgumentOutOfRangeException("mailshot", "Currently only supporting Format ID of 1.");
                    break;
            }

            //TODO: Get the correct XSL
            string templateXsl = null;
            switch (mailshot.TemplateId)
            {
                case 1:
                    formatXsl = "XSL for Template 1 goes here";
                    break;
                default:
                    throw new ArgumentOutOfRangeException("mailshot", "Currently only supporting Template ID of 1.");
                    break;
            }

            //TODO: Get the correct XSL
            string themeXsl = null;
            switch (mailshot.ThemeId)
            {
                case 1:
                    themeXsl = "XSL for Theme 1 goes here";
                    break;
                default:
                    throw new ArgumentOutOfRangeException("mailshot", "Currently only supporting Theme ID of 1.");
                    break;
            }*/

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
            var ticket = new XElement("page",
                new XAttribute(XNamespace.Xmlns + "fo", _foNamespace));

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
