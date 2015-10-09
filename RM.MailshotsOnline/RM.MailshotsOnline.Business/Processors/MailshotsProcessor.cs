using HC.RM.Common.PCL.Helpers;
using HC.RM.Common.PCL.Persistence;
using Newtonsoft.Json;
using RM.MailshotsOnline.Entities.JsonModels;
using RM.MailshotsOnline.Entities.PageModels.Settings;
using RM.MailshotsOnline.Entities.ViewModels;
using RM.MailshotsOnline.PCL.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RM.MailshotsOnline.Business.Processors
{
    public class MailshotsProcessor
    {
        private XNamespace _foNamespace;
        private IBlobService _blobService;
        private ILogger _logger;
        private IPAddress _startIpAddress;
        private IPAddress _endIpAddress;

        /// <summary>
        /// Creates a new instance of the MailshotsProcessor class
        /// </summary>
        /// <param name="logger">The Logger</param>
        /// <param name="blobStorageService">The Blob Storage service</param>
        public MailshotsProcessor(ILogger logger, IBlobService blobStorageService)
        {
            _foNamespace = "http://www.w3.org/1999/XSL/Format";
            _logger = logger;
            _blobService = blobStorageService;
            _startIpAddress = null;
            _endIpAddress = null;
        }

        public async Task<XmlAndXslData> GetXmlAndXslForMailshot(IMailshot mailshot, string xslOverride = null)
        {
            return await GetXmlAndXslForMailshot(mailshot, null, null, xslOverride);
        }

        /// <summary>
        /// Gets the XML and XSLT output for a mailshot to send to the SparQ service
        /// </summary>
        /// <param name="mailshot">Mailshot to be generated</param>
        /// <param name="xslOverride">Specify the XSL to use as an override, rather than generating XSL from the Mailshot format / template / theme</param>
        /// <returns>Item 1: XML content; Item 2: XSLT transform file</returns>
        public async Task<XmlAndXslData> GetXmlAndXslForMailshot(IMailshot mailshot, string ipAddressStart, string ipAddressEnd, string xslOverride = null)
        {
            IPAddress parsedIpAddress = null;
            if (IPAddress.TryParse(ipAddressStart, out parsedIpAddress))
            {
                _startIpAddress = parsedIpAddress;
                if (IPAddress.TryParse(ipAddressEnd, out parsedIpAddress))
                {
                    _endIpAddress = parsedIpAddress;
                }
            }

            XmlAndXslData result = new XmlAndXslData();

            if (mailshot == null)
            {
                throw new ArgumentNullException("mailshot");
            }

            // Generate the XSL

            //TODO: Get this another way?
            var finalXsl = $@"<?xml version=""1.0"" encoding=""utf-8""?>
<xsl:stylesheet version=""1.0"" xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:fo=""http://www.w3.org/1999/XSL/Format"" xmlns:rx=""http://www.renderx.com/XSL/Extensions"">
  
  <xsl:output method=""xml"" version=""1.0"" encoding=""UTF-8"" />

  <!-- From Theme: -->
  {mailshot.Theme.XslData}

  <xsl:template match=""/page"">
    <!--This sets up pages, dimensions, backgrounds, headers and footers. Background images can also be applied within this section.-->
    <fo:root>
      <fo:layout-master-set>
        {mailshot.Format.XslData}

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
  {mailshot.Template.XslData}

</xsl:stylesheet>";

            // Generate the XML
            //if (mailshot.Content == null || string.IsNullOrEmpty(mailshot.Content.Content))
            //{
            //    throw new ArgumentException("Mailshot content cannot be null", "mailshot");
            //}

            if (string.IsNullOrEmpty(mailshot.ContentText))
            {
                throw new ArgumentException("Mailshot content cannot be null", "mailshot");
            }

            MailshotEditorContent content = JsonConvert.DeserializeObject<MailshotEditorContent>(mailshot.ContentText);
            result.UsedImageSrcs = content.Elements.Where(e => !string.IsNullOrEmpty(e.Src)).Select(e => e.Src);

            var finalXml = await GetContentXmlFromJson(content, mailshot);
            result.XmlData = finalXml;

            if (!string.IsNullOrEmpty(xslOverride))
            {
                result.XslStylesheet = xslOverride;
            }
            else
            {
                result.XslStylesheet = finalXsl;
            }

            return result;
        }

        /// <summary>
        /// Generates the appropriate XML content to send through to SparQ based on the editor content
        /// </summary>
        /// <param name="jsonContent">Parsed JSON content from the editor interface</param>
        /// <returns>XML string</returns>
        private async Task<string> GetContentXmlFromJson(MailshotEditorContent jsonContent, IMailshot mailshot)
        {
            var contentXml = new XDocument();
            var ticket = new XElement("page", new XAttribute(XNamespace.Xmlns + "fo", _foNamespace));

            foreach (var jsonElement in jsonContent.Elements)
            {
                var content = jsonElement.Content;
                //if (!string.IsNullOrWhiteSpace(jsonElement.Src))
                //{
                //    content = jsonElement.Src;
                //}

                if (!string.IsNullOrEmpty(content))
                {
                    var contentElement = new XElement(jsonElement.Name.Replace(" ", string.Empty));
                    if (!content.StartsWith("data:image"))
                    {
                        contentElement.Add(ProcessHtmlContent(content
                            .Replace("&nbsp;", "&#160;")
                            .Replace("<br>", "<br />")));
                    }
                    else
                    {
                        // Move the image to blob storage
                        string imageUrl = string.Empty;
                        var parts = content.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                        if (parts.Length == 2)
                        {
                            // Check first part to see what type of image this is
                            string imageMimeType = "image/jpeg"; // Assume JPG by default
                            var formatParts = parts[0].Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                            if (formatParts.Length == 2)
                            {
                                imageMimeType = formatParts[1];
                            }

                            // Convert second part into byte array for storage
                            var contentParts = parts[1].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                            if (contentParts.Length == 2)
                            {
                                if (contentParts[0] == "base64")
                                {
                                    byte[] imageBytes = null;
                                    try
                                    {
                                        imageBytes = Convert.FromBase64String(contentParts[1]);
                                    }
                                    catch (Exception ex)
                                    {
                                        _logger.Exception(this.GetType().Name, "GetContentXmlFromJson", ex);
                                        _logger.Error(this.GetType().Name, "GetContentXmlFromJson", "Error converting base64 string into byte array");
                                    }

                                    if (imageBytes != null)
                                    {
                                        // Save bytes to blob and generate URL
                                        string extension = "jpeg";
                                        var mimeTypeParts = imageMimeType.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                                        if (mimeTypeParts.Length == 2)
                                        {
                                            extension = mimeTypeParts[1];
                                        }

                                        string filename = $"pdf-images/{mailshot.UserId}/{mailshot.ProofPdfOrderNumber}/{jsonElement.Name}.{extension}";
                                        var blobId = await _blobService.StoreAsync(imageBytes, filename, imageMimeType);

                                        if (_startIpAddress != null)
                                        {
                                            if (_endIpAddress != null)
                                            {
                                                imageUrl = _blobService.GetBlobUriWithSas(blobId, new TimeSpan(1, 0, 0, 0), true, false, false, "StIves", _startIpAddress.ToString(), _endIpAddress.ToString()).ToString();
                                            }
                                            else
                                            {
                                                imageUrl = _blobService.GetBlobUriWithSas(blobId, new TimeSpan(1, 0, 0, 0), true, false, false, "StIves", _startIpAddress.ToString()).ToString();
                                            }
                                        }
                                        else
                                        {
                                            imageUrl = _blobService.GetBlobUriWithSas(blobId, new TimeSpan(1, 0, 0, 0), true, false, false).ToString();
                                        }
                                    }
                                }
                            }
                        }

                        if (string.IsNullOrEmpty(imageUrl))
                        {
                            contentElement.Add(new XCData(content));
                        }
                        else
                        {
                            contentElement.Add(imageUrl);
                        }
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

        private XNode ProcessSubNode(XNode rootNode, bool inOrderedList = false, int listItemNumber = 0)
        {
            bool currentElementIsOrderedList = false;
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
                else if (rootElement.Name == "ul")
                {
                    returnElement = new XElement(_foNamespace + "list-block");
                }
                else if (rootElement.Name == "ol")
                {
                    currentElementIsOrderedList = true;
                    returnElement = new XElement(_foNamespace + "list-block");
                }
                else if (rootElement.Name == "li")
                {
                    inOrderedList = false;
                    returnElement = new XElement(_foNamespace + "list-item");
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

                if (rootElement.Name == "li")
                {
                    var itemLabel = new XElement(_foNamespace + "list-item-label");
                    itemLabel.SetAttributeValue("end-indent", "label-end()");
                    if (inOrderedList)
                    {
                        listItemNumber++;
                        itemLabel.Add(new XText(listItemNumber.ToString()));
                    }
                    else
                    {
                        itemLabel.Add(new XText("&#183;"));
                    }

                    var itemBody = new XElement(_foNamespace + "list-item-body");
                    foreach (var subNode in rootElement.Nodes())
                    {
                        itemBody.Add(ProcessSubNode(subNode, false, 0));
                    }

                    returnElement.Add(itemLabel, itemBody);
                }
                else
                {
                    foreach (var subNode in rootElement.Nodes())
                    {
                        returnElement.Add(ProcessSubNode(subNode, (currentElementIsOrderedList || inOrderedList), listItemNumber));
                    }
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
