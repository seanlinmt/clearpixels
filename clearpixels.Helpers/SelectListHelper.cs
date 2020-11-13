
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
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace clearpixels.Helpers
{
    public static class SelectListHelper
    {
        public static IEnumerable<SelectListItem> ToSelectList(this IEnumerable<SelectListItem> values)
        {
            return values.ToSelectList(null);
        }

        public static IEnumerable<SelectListItem> ToSelectList(this IEnumerable<SelectListItem> values, object selectedValue)
        {
            return values.ToSelectList(selectedValue, "None", "");
        }

        public static IEnumerable<SelectListItem> ToSelectList(this IEnumerable<SelectListItem> values, 
            object selectedValue, string emptyText, string emptyValue)
        {
            var result = values.ToList();
            result.Insert(0, new SelectListItem { Text = emptyText, Value = emptyValue });
            if (selectedValue == null)
            {
                return new SelectList(result, "Value", "Text");
            }
            return new SelectList(result, "Value", "Text", selectedValue);
        }

        public static IEnumerable<SelectListItem> ToSelectList(this Type type, bool useDescrAsVal, string emptyText, string emptyValue, string selectedValue = "")
        {
            return type.ToSelectList(useDescrAsVal, emptyText, emptyValue, true, selectedValue);
        }

        public static IEnumerable<SelectListItem> ToSelectList(this Type type, bool useDescrAsVal, string emptyText, string emptyValue, bool order, string selectedValue)
        {
            var enumvalues = Enum.GetValues(type);
            var values = new List<SelectListItem>();
            foreach (Enum value in enumvalues)
            {
                var v = useDescrAsVal ? value.ToDescriptionString() : value.ToString();
                values.Add(new SelectListItem()
                               {
                                   Text = value.ToDescriptionString(),
                                   Value = v,
                                   Selected = v == selectedValue
                               });
            }

            if (order)
            {
                values = values.OrderBy(x => x.Text).ToList();
            }
            else
            {
                values = values.ToList();
            }

            if (emptyText != null)
            {
                values.Insert(0, new SelectListItem { Text = emptyText, Value = emptyValue });    
            }

            return values;
        }
    }
}