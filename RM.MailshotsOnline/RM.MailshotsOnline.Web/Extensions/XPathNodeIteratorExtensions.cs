using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Xml.XPath;

namespace RM.MailshotsOnline.Web.Extensions
{
    public static class XPathNodeIteratorExtensions
    {
        public static IDictionary<string, string> ToPreValueDictionary(this XPathNodeIterator iterator)
        {
            var d = new Dictionary<string, string>();

            while (iterator.MoveNext())
            {
                var nav = iterator.Current;
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
