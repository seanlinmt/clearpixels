
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
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace clearpixels.Helpers
{
    public static class SerializerHelper
    {
        public static XElement ToXML<T>(this T obj)
        {
            var x = new XmlSerializer(typeof(T));
            var doc = new XDocument();

            using (XmlWriter xw = doc.CreateWriter())
            {
                x.Serialize(xw, obj);
                xw.Close();
            }

            return doc.Root;

        }

        public static T FromXML<T>(this XElement el) where T : class 
        {
            var x = new XmlSerializer(typeof(T));
            using (XmlReader xr = el.CreateReader())
            {
                return x.Deserialize(xr) as T;
            }

        }
    }
}