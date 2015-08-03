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
        /// <summary>
        /// Gets the XML and XSLT output for a mailshot to send to the SparQ service
        /// </summary>
        /// <param name="mailshot">Mailshot to be generated</param>
        /// <returns>Item 1: XML content; Item 2: XSLT transform file</returns>
        public Tuple<string, string> GetXmlAndXslForMailshot(IMailshot mailshot)
        {
            if (mailshot == null)
            {
                throw new ArgumentNullException("mailshot");
            }

            string formatXsl = null;

            switch (mailshot.FormatId)
            {
                case 1:
                    formatXsl = "XSL for Format 1 goes here";
                    break;
                default:
                    throw new ArgumentOutOfRangeException("mailshot", "Currently only supporting Format ID of 1.");
                    break;
            }

            string TemplateXsl = null;

            switch (mailshot.TemplateId)
            {
                case 1:
                    formatXsl = "XSL for Template 1 goes here";
                    break;
                default:
                    throw new ArgumentOutOfRangeException("mailshot", "Currently only supporting Template ID of 1.");
                    break;
            }

            string themeXsl = null;

            switch (mailshot.ThemeId)
            {
                case 1:
                    themeXsl = "XSL for Theme 1 goes here";
                    break;
                default:
                    throw new ArgumentOutOfRangeException("mailshot", "Currently only supporting Theme ID of 1.");
                    break;
            }

            string contentXml = null;

            if (mailshot.Content == null || string.IsNullOrEmpty(mailshot.Content.Content))
            {
                throw new ArgumentException("Mailshot content cannot be null", "mailshot");
            }

            MailshotEditorContent content = JsonConvert.DeserializeObject<MailshotEditorContent>(mailshot.Content.Content);

            //TODO: Compile the XSL properly
            var finalXsl = File.ReadAllText("C:\\Projects\\RoyalMail\\MSOL\\RM.MailshotsOnline\\RM.MailshotsOnline\\XML\\Formats\\A4PageComplete.xsl");

            //TODO: Convert the XML from the content
            var finalXml = GetContentXmlFromJson(content);

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
            var ticket = new XElement("page");
            foreach (var jsonElement in jsonContent.Elements)
            {
                var contentElement = new XElement(jsonElement.Name);
                if (!jsonElement.Content.StartsWith("data:image"))
                {
                    contentElement.Value = ProcessHtmlContent(jsonElement.Content);
                }
                else
                {
                    contentElement.Value = jsonElement.Content;
                }

                ticket.Add(contentElement);
            }

            contentXml.Add(ticket);
            return contentXml.ToString(SaveOptions.None);
        }

        /// <summary>
        /// Processes the HTML from the editable divs of the editor and converts it into XSL:FO XML snippets
        /// </summary>
        /// <param name="htmlContent">HTML content from the editor</param>
        /// <returns>XSL:FO XML snippets</returns>
        private string ProcessHtmlContent(string htmlContent)
        {
            // TODO: Strip all the HTML
            var output = htmlContent.Replace("<div><br></div>", Environment.NewLine);
            output = output.Replace("<div>", string.Empty).Replace("</div>", string.Empty);

            // TODO: Convert PX into PT
            var fontsizeRegex = new Regex(@"<span style=\""(font\-size): ([\d] +)px;\"">([\w\W]*)<\/span>");
            output = fontsizeRegex.Replace(output, @"<fo:inline $1=""$2pt"">$3</fo:inline>");

            // TODO: All of the other required conversions!

            return output;
        }
    }
}
