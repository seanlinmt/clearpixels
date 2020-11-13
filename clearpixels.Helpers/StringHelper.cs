
/*
 * LICENSE NOTE:
 *
 * Copyright  2012-2013 Clear Pixels Limited, All Rights Reserved.
 *
 * Unless explicitly acquired and licensed from Licensor under another license, the
 * contents of this file are subject to the Reciprocal Public License ("RPL")
 * Version 1.5, or subsequent versions as allowed by the RPL, and You may not copy
 * or use this file in either source code or executable form, except in compliance
 * with the terms and conditions of the RPL. 
 *
 * All software distributed under the RPL is provided strictly on an "AS IS" basis,
 * WITHOUT WARRANTY OF ANY KIND, EITHER EXPRESS OR IMPLIED, AND LICENSOR HEREBY
 * DISCLAIMS ALL SUCH WARRANTIES, INCLUDING WITHOUT LIMITATION, ANY WARRANTIES OF
 * MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE, QUIET ENJOYMENT, OR
 * NON-INFRINGEMENT. See the RPL for specific language governing rights and
 * limitations under the RPL.
 *
 * @author         Sean Lin Meng Teck <seanlinmt@clearpixels.co.nz>
 * @copyright      2012-2013 Clear Pixels Limited
 */
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Microsoft.CSharp;

namespace clearpixels.Helpers
{
    public static class StringHelper
    {
        public static string ToNumbersOnly(this string str)
        {
            return Regex.Replace(str, @"[^0-9]", "");
        }

        public static string ToLiteral(this string input)
        {
            var writer = new StringWriter();
            var provider = new CSharpCodeProvider();
            provider.GenerateCodeFromExpression(new CodePrimitiveExpression(input), writer, null);
            return writer.GetStringBuilder().ToString();
        }

        public static HtmlString ToHtmlString(this string input)
        {
            if (input == null)
            {
                input = "";
            }

            return new HtmlString(input);
        }

        /// <summary>
        /// removes string starting from specified string if specified string exists
        /// </summary>
        /// <param name="str">string to parse</param>
        /// <param name="toremove">string to remove</param>
        /// <returns></returns>
        public static string Remove(this string str, string toremove)
        {
            var idx = str.IndexOf(toremove, StringComparison.Ordinal);
            if (idx != -1)
            {
                return str.Substring(0, idx);
            }
            return str;
        }

        public static string RemoveLinks(this string input)
        {
            return Regex.Replace(input, @"<[a|A][^>]*[a|A]>", String.Empty);

        }

        public static IHtmlString ToQuantityHtmlString(this int quantity, string singular, string plural = "", string counterclass = "counter")
        {
            return new HtmlString(quantity.ToQuantityString(singular, plural, counterclass));
        }

        public static string ToQuantityString(this int quantity, string singular, string plural = "", string counterclass = "counter")
        {
            if (quantity == 1)
            {
                return String.Format("<span class='{0}'>1</span> {1}", counterclass, singular);
            }

            return String.Format("<span class='{0}'>{1}</span> {2}", counterclass, quantity, plural == "" ? singular + "s" : plural);
        }

        public static string ToSafeFilename(this string str)
        {
            return Regex.Replace(str, @"[\\/:\*\?<>|]", "-");
        }


        public static string ToString(this decimal? number, string format)
        {
            if (!number.HasValue)
            {
                return "";
            }

            return number.Value.ToString(format);
        }

        /// <summary>
        /// replacement for String.Format
        /// </summary>
        public static string With(this string format, params object[] args)
        {
            return String.Format(format, args);
        }
    }
}
