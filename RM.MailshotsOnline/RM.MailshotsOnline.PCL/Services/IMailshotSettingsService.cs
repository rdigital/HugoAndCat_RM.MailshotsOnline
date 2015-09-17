using RM.MailshotsOnline.PCL.Models.MailshotSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RM.MailshotsOnline.PCL.Services
{
    public interface IMailshotSettingsService
    {
        /// <summary>
        /// Add or update a Format
        /// </summary>
        /// <param name="format">The Format to be saved</param>
        void AddOrUpdateFormat(IFormat format);

        /// <summary>
        /// Add or update a Template
        /// </summary>
        /// <param name="template">The Template to be saved</param>
        void AddOrUpdateTemplate(ITemplate template);

        /// <summary>
        /// Add or update a Theme
        /// </summary>
        /// <param name="theme">The Theme to be saved</param>
        void AddOrUpdateTheme(ITheme theme);

        /// <summary>
        /// Gets all formats
        /// </summary>
        /// <returns>Collection of IFormat objects</returns>
        IEnumerable<IFormat> GetFormats();

        /// <summary>
        /// Fetches a Format object based on it's JSON index
        /// </summary>
        /// <param name="index">JSON index</param>
        /// <returns>A Format object</returns>
        IFormat GetFormatByJsonIndex(int index);

        /// <summary>
        /// Fetches a Template based on its JSON index
        /// </summary>
        /// <param name="index">JSON index of the template</param>
        /// <param name="formatIndex">JSON index of the parent format</param>
        /// <returns>A Template object</returns>
        ITemplate GetTemplateByJsonIndex(int index, int formatIndex);

        /// <summary>
        /// Gets the templates that are applicable to a format
        /// </summary>
        /// <param name="formatIndex">JSON index of the Format</param>
        /// <returns>Collection of Template objects</returns>
        IEnumerable<ITemplate> GetTemplatesForFormat(int formatIndex);

        /// <summary>
        /// Fetches a Theme based on its JSON index
        /// </summary>
        /// <param name="index">JSON index</param>
        /// <returns>Theme Object</returns>
        ITheme GetThemeByJsonIndex(int index);

        /// <summary>
        /// Fetches all themes
        /// </summary>
        /// <returns>Collection of Theme objects</returns>
        IEnumerable<ITheme> GetThemes();
    }
}
