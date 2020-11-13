
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
using System.Drawing.Imaging;
using System.Web;

namespace clearpixels.Helpers.imaging
{
    public static class ImageHelper
    {
        public static string GetUniqueFilename(this string extension)
        {
            return string.Concat(DateTime.UtcNow.Ticks, extension);
        }

        public static bool IsImage(this string filename)
        {
            var extIndex = filename.LastIndexOf('.');
            var ext = filename.Substring(extIndex);
            switch (ext)
            {
                case ".jpg":
                case ".jpeg":
                case ".JPG":
                case "JPEG":
                case ".png":
                case ".PNG":
                case ".gif":
                case ".GIF":
                    return true;
                default:
                    return false;
            }
        }

        public static ImageFormat ToImageFormat(this string filename)
        {
            var extIndex = filename.LastIndexOf('.');
            var ext = filename.Substring(extIndex);
            switch (ext)
            {
                case ".jpg":
                case ".jpeg":
                case ".JPG":
                case "JPEG":
                    return ImageFormat.Jpeg;
                case ".png":
                case ".PNG":
                    return ImageFormat.Png;
                case ".gif":
                case ".GIF":
                    return ImageFormat.Gif;
                default:
                    // 14/11: commented out because gbase will flood log
                    //Syslog.Write(ErrorLevel.INFORMATION, string.Concat("Unrecognised image extension: ", filename));
                    return ImageFormat.Jpeg;
            }
        }

        private static string ToDataUriType(this string filename)
        {
            var extIndex = filename.LastIndexOf('.');
            var ext = filename.Substring(extIndex);
            switch (ext)
            {
                case ".jpg":
                case ".jpeg":
                case ".JPG":
                case "JPEG":
                    return "data:image/jpeg;base64,";
                case ".png":
                case ".PNG":
                    return "data:image/png;base64,";
                case ".gif":
                case ".GIF":
                    return "data:image/gif;base64,";
                default:
                    throw new Exception(string.Concat("Unrecognised image extension for datauri: ", filename));
            }
        }

        public static string ToImageString(this string imgpath, string imageclass = "", string altText = "")
        {
            return string.Format("<img class='{0}' src='{1}' alt='{2}' />", imageclass, imgpath, altText);
        }

        public static IHtmlString ToHtmlImage(this string imgpath, string imageclass = "", string altText = "")
        {
            return new HtmlString(imgpath.ToImageString(imageclass, altText));
        }
    }
}
