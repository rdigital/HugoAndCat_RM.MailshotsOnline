using RM.MailshotsOnline.Data.DAL;
using RM.MailshotsOnline.PCL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RM.MailshotsOnline.PCL.Models.MailshotSettings;
using RM.MailshotsOnline.Entities.DataModels.MailshotSettings;

namespace RM.MailshotsOnline.Data.Services
{
    public class MailshotSettingsService : IMailshotSettingsService
    {
        private StorageContext _context;

        public MailshotSettingsService()
        {
            _context = new StorageContext();
        }

        /// <summary>
        /// Add or update a Format
        /// </summary>
        /// <param name="format">The Format to be saved</param>
        public void AddOrUpdateFormat(IFormat format)
        {
            var existingFormat = _context.Formats.FirstOrDefault(f => f.UmbracoPageId == format.UmbracoPageId);
            if (existingFormat != null)
            {
                // Update values and save
                existingFormat.Name = format.Name;
                existingFormat.JsonIndex = format.JsonIndex;
                existingFormat.XslData = format.XslData;
                existingFormat.UpdatedDate = DateTime.UtcNow;
                existingFormat.PricePerPrint = format.PricePerPrint;
                existingFormat.JsonData = format.JsonData;
            }
            else
            {
                // Add format
                _context.Formats.Add((Format)format);
            }

            _context.SaveChanges();
        }

        /// <summary>
        /// Add or update a Template
        /// </summary>
        /// <param name="template">The Template to be saved</param>
        public void AddOrUpdateTemplate(ITemplate template)
        {
            var existingTemplate = _context.Templates.FirstOrDefault(f => f.UmbracoPageId == template.UmbracoPageId);
            if (existingTemplate != null)
            {
                // Update values and save
                existingTemplate.Name = template.Name;
                existingTemplate.JsonIndex = template.JsonIndex;
                existingTemplate.XslData = template.XslData;
                existingTemplate.FormatUmbracoPageId = template.FormatUmbracoPageId;
                existingTemplate.UpdatedDate = DateTime.UtcNow;
                existingTemplate.JsonData = template.JsonData;
            }
            else
            {
                // Add format
                _context.Templates.Add((Template)template);
            }

            _context.SaveChanges();
        }

        /// <summary>
        /// Add or update a Theme
        /// </summary>
        /// <param name="theme">The Theme to be saved</param>
        public void AddOrUpdateTheme(ITheme theme)
        {
            var existingTheme = _context.Themes.FirstOrDefault(f => f.UmbracoPageId == theme.UmbracoPageId);
            if (existingTheme != null)
            {
                // Update values and save
                existingTheme.Name = theme.Name;
                existingTheme.JsonIndex = theme.JsonIndex;
                existingTheme.XslData = theme.XslData;
                existingTheme.UpdatedDate = DateTime.UtcNow;
                existingTheme.JsonData = theme.JsonData;
            }
            else
            {
                // Add format
                _context.Themes.Add((Theme)theme);
            }

            _context.SaveChanges();
        }

        /// <summary>
        /// Adds or updates default content for a Mailshot
        /// </summary>
        /// <param name="defaultContent">The Mailshot Default Content to save</param>
        public void AddOrUpdateMailshotDefaultContent(IMailshotDefaultContent defaultContent)
        {
            var existingContent = _context.MailshotDefaultContent.FirstOrDefault(c => c.UmbracoPageId == defaultContent.UmbracoPageId);
            if (existingContent != null)
            {
                // Update values and save
                existingContent.JsonData = defaultContent.JsonData;
                existingContent.JsonIndex = defaultContent.JsonIndex;
                existingContent.Name = defaultContent.Name;
                existingContent.UpdatedDate = DateTime.UtcNow;
            }
            else
            {
                // Add default content
                _context.MailshotDefaultContent.Add((MailshotDefaultContent)defaultContent);
            }

            _context.SaveChanges();
        }

        /// <summary>
        /// Fetches the default mailshot content that matches the given index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public IMailshotDefaultContent GetMailshotDefaultContent(int index)
        {
            return _context.MailshotDefaultContent.FirstOrDefault(c => c.JsonIndex == index);
        }


        /// <summary>
        /// Gets all formats
        /// </summary>
        /// <returns>Collection of IFormat objects</returns>
        public IEnumerable<IFormat> GetFormats()
        {
            return _context.Formats.OrderBy(f => f.Name);
        }

        /// <summary>
        /// Fetches a Format object based on it's JSON index
        /// </summary>
        /// <param name="index">JSON index</param>
        /// <returns>A Format object</returns>
        public IFormat GetFormatByJsonIndex(int index)
        {
            return _context.Formats.FirstOrDefault(f => f.JsonIndex == index);
        }

        /// <summary>
        /// Fetches a Template based on its JSON index
        /// </summary>
        /// <param name="index">JSON index of the template</param>
        /// <param name="formatIndex">JSON index of the parent format</param>
        /// <returns>A Template object</returns>
        public ITemplate GetTemplateByJsonIndex(int index, int formatIndex)
        {
            // Odd query becuase we need to confirm that the format index matches the format assigned in Umbraco
            var templates = from t in _context.Templates
                            join f in _context.Formats on t.FormatUmbracoPageId equals f.UmbracoPageId
                            where t.JsonIndex == index && f.JsonIndex == formatIndex
                            select t;

            return templates.FirstOrDefault();
        }

        /// <summary>
        /// Gets the templates that are applicable to a format
        /// </summary>
        /// <param name="formatIndex">JSON index of the Format</param>
        /// <returns>Collection of Template objects</returns>
        public IEnumerable<ITemplate> GetTemplatesForFormat(int formatIndex)
        {
            // Odd query becuase we need to confirm that the format index matches the format assigned in Umbraco
            var templates = from t in _context.Templates
                            join f in _context.Formats on t.FormatUmbracoPageId equals f.UmbracoPageId
                            where f.JsonIndex == formatIndex
                            orderby t.Name
                            select t;

            return templates;
        }

        /// <summary>
        /// Fetches a Theme based on its JSON index
        /// </summary>
        /// <param name="index">JSON index</param>
        /// <returns>Theme Object</returns>
        public ITheme GetThemeByJsonIndex(int index)
        {
            return _context.Themes.FirstOrDefault(f => f.JsonIndex == index);
        }

        /// <summary>
        /// Fetches all themes
        /// </summary>
        /// <returns>Collection of Theme objects</returns>
        public IEnumerable<ITheme> GetThemes()
        {
            return _context.Themes.OrderBy(t => t.Name);
        }
    }
}
