using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.MailshotsOnline.Web.Extensions
{
    public static class StringExtensions
    {
        public static List<int> ToIntList(this string input)
        {
            var result = new List<int>();
            var stringArray = input.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var stringValue in stringArray)
            {
                int intValue;
                if (int.TryParse(stringValue, out intValue))
                {
                    result.Add(intValue);
                }
            }

            return result;
        }
    }
}
