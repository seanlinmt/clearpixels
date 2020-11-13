
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
namespace clearpixels.Helpers.JSON
{
    public class JsonData : ErrorResponse
    {
        public string id { get; set; }
        public object data { get; set; }
        public string title { get; set; }
    }

    public static class JsonDataHelper
    {
        public static JsonData ToJsonOKMessage(this string msg)
        {
            return new JsonData
                       {
                           message = msg, 
                           success = true, 
                           data = null
                       };
        }

        public static JsonData ToJsonOKData(this object data, string dialogTitle = "")
        {
            return new JsonData
                       {
                           message = "", 
                           success = true, 
                           data = data,
                           title = dialogTitle
                       };
        }

        public static JsonData ToJsonFailData(this object data)
        {
            return new JsonData
                       {
                           message = "", 
                           success = false, 
                           data = data
                       };
        }

        public static JsonData ToJsonFail(this string msg)
        {
            return new JsonData
                       {
                           message = msg, 
                           success = false
                       };
        }
    }
}
