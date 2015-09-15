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
            }
            else
            {
                // Add format
                _context.Themes.Add((Theme)theme);
            }

            _context.SaveChanges();
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
        /// Fetches a Theme based on its JSON index
        /// </summary>
        /// <param name="index">JSON index</param>
        /// <returns>Theme Object</returns>
        public ITheme GetThemeByJsonIndex(int index)
        {
            return _context.Themes.FirstOrDefault(f => f.JsonIndex == index);
        }
    }
}
