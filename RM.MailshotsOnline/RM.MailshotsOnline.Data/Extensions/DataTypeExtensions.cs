using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using umbraco;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace RM.MailshotsOnline.Data.Extensions
{
    public static class DataTypeServiceExtenions
    {
        public static IDictionary<string, string> GetPreValues(this IDataTypeService dataTypeService, string preValueName)
        {
            var titleDataType = Umbraco.Core.ApplicationContext.Current.Services.DataTypeService.GetDataTypeDefinitionByName(preValueName);
            var titlePrevalues = library.GetPreValues(titleDataType.Id);

            var d = new Dictionary<string, string>();

            while (titlePrevalues.MoveNext())
            {
                var nav = titlePrevalues.Current;
                var x = nav.Select("preValue");

                while (x.MoveNext())
                {
                    d.Add(x.Current.GetAttribute("id", ""), x.Current.Value);
                }
            }

            return d;
        }
    }
}
