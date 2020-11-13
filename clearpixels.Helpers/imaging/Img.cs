
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
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace clearpixels.Helpers.imaging
{
    public static class Img
    {
        public const string PHOTO_NO_THUMBNAIL = "/Content/img/users/profile_nophoto.png";

        /// <summary>
        /// returns path to thumbnail, if thumbnail does not exist it creates it then returns the new path
        /// </summary>
        /// <param name="filePath">the path to the thumbnail, usually stored in db</param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string by_size(string filePath, Imgsize type)
        {
            var dim = getImageDimensionsFromSize(type);
            int width = dim.Width;
            int height = dim.Height;
            // appends dimension to path
            string suffix = width + "x" + height;
            string ofile;
            string thumb = ofile = filePath;
            string ext = thumb.Substring(thumb.LastIndexOf(".") + 1);
            string part1 = thumb.Substring(0, thumb.LastIndexOf("."));
            if (part1.IndexOf(".") == -1)
            {
                thumb = part1 + "." + suffix + "." + ext;
            }
            else
            {
                string part2 = part1.Substring(0, part1.LastIndexOf("."));
                thumb = part2 + "." + suffix + "." + ext;
            }
            bool fileExist = false;

            if (!fileExist)
            {
                fileExist = File.Exists(AppDomain.CurrentDomain.BaseDirectory + thumb);
            }
            if (!fileExist)
            {
                thumb = thumbnail(ofile, suffix, width, height, type);
            }

            return thumb;
        }

        private static Image createResizedImage(Image originalImage, Size poSize, Imgsize type)
        {
            //Detach image from its source
            Image oImageOriginal = (Image) originalImage.Clone();

            //Resize new image
            var oResizedImage = new Bitmap(poSize.Width, poSize.Height);
            Graphics oGraphic = Graphics.FromImage(oResizedImage);

            oGraphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
            oGraphic.CompositingQuality = CompositingQuality.HighSpeed;
            oGraphic.SmoothingMode = SmoothingMode.HighSpeed;
            oGraphic.InterpolationMode = InterpolationMode.Low;
            Rectangle oRectangle = new Rectangle(0, 0, poSize.Width, poSize.Height);

            oGraphic.DrawImage(oImageOriginal, oRectangle);

            // cleanup
            oGraphic.Dispose();

            oImageOriginal.Dispose();
            return oResizedImage;
        }

        public static void Delete(string path)
        {
            // go through each image size
            var sizes = Enum.GetValues(typeof (Imgsize));
            foreach (Imgsize size in sizes)
            {
                var loc = AppDomain.CurrentDomain.BaseDirectory + by_size(path, size);
                bool exists = File.Exists(loc);
                if (exists)
                {
                    File.Delete(loc);
                }
            }

            // finally delete the main image
            var mainloc = AppDomain.CurrentDomain.BaseDirectory + path;
            if (File.Exists(mainloc))
            {
                File.Delete(mainloc);
            }
        }

        private static Size getImageDimensionsFromSize(Imgsize size)
        {
            switch (size)
            {
                case Imgsize.THUMB:
                    return new Size(200, 300);
                case Imgsize.BLOG:
                    return new Size(800, 500);
                case Imgsize.GALLERY:
                    return new Size(75, 75);
                case Imgsize.MAX:
                    return new Size(900, 675);
                case Imgsize.USER_THUMB:
                    return new Size(50, 50);
                case Imgsize.USER_PROFILE:
                    return new Size(270, 270);
                default:
                    throw new ArgumentException();
            }
        }

        /// <summary>
        /// generates a thumbnail given the path of the original images
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="suffix"></param>
        /// <param name="desiredWidth"></param>
        /// <param name="desiredHeight"></param>
        /// <returns></returns>
        private static string thumbnail(string filePath, string suffix, float desiredWidth, float desiredHeight,
                                        Imgsize type)
        {
            string thumb = filePath;
            string file = filePath;
            string ext = thumb.Substring(thumb.LastIndexOf(".") + 1);
            thumb = thumb.Substring(0, thumb.IndexOf(".")) + "." + suffix + "." + ext;
            bool exists = File.Exists(AppDomain.CurrentDomain.BaseDirectory + file);
            if (!exists)
            {
                //Syslog.Write(ErrorLevel.ERROR, string.Concat("Cannot find file: ", AppDomain.CurrentDomain.BaseDirectory + file));
                return "";
            }
            // These are the ratio calculations
            int width;
            int height;
            Image img = null;
            try
            {
                img = Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + file);
                width = img.Width;
                height = img.Height;
            }
            catch (OutOfMemoryException)
            {
                return "";
            }
            finally
            {
                if (img != null)
                {
                    img.Dispose();
                }
            }

            float factor = 0;
            if (width > 0 && height > 0)
            {
                float wfactor = desiredWidth/width;
                float hfactor = desiredHeight/height;
                factor = wfactor < hfactor ? wfactor : hfactor;
            }
            if (factor > 0 || factor < 0)
            {
                int twidth = Convert.ToInt32(Math.Floor(factor*width));
                int theight = Convert.ToInt32(Math.Floor(factor*height));
                convert(file, thumb, twidth, theight, type);
            }
            else
            {
                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + thumb))
                {
                    File.Delete(AppDomain.CurrentDomain.BaseDirectory + thumb);
                }
                File.Copy(AppDomain.CurrentDomain.BaseDirectory + file,
                                    AppDomain.CurrentDomain.BaseDirectory + thumb);
            }
            return thumb;
        }

        private static void convert(string source, string destination, int desiredWidth, int desiredHeight, Imgsize type)
        {
            createImage(source, destination, desiredWidth, desiredHeight, type);
        }

        private static bool createImage(string srcName, string destName, int desiredWidth, int desiredHeight,
                                        Imgsize type)
        {
            var source = AppDomain.CurrentDomain.BaseDirectory + srcName;
            var destination = AppDomain.CurrentDomain.BaseDirectory + destName;
            // Capture the original size of the uploaded image
            Image src = null;
            try
            {
                src = Image.FromFile(source);
            }
            catch (Exception ex)
            {
                if (src != null)
                {
                    src.Dispose();
                }
                throw;
            }

            //Resize new image
            //Image tmp = src.GetThumbnailImage(desiredWidth, desiredHeight, null, IntPtr.Zero);
            Size imgSize = new Size(desiredWidth, desiredHeight);
            Image tmp = createResizedImage(src, imgSize, type);

            try
            {
                File.Delete(destination);
            }
            catch (Exception)
            {
            }

            try
            {
                tmp.Save(destination, destination.ToImageFormat());
            }
            catch
            {
                return false;
            }
            finally
            {
                src.Dispose();
                tmp.Dispose();
            }

            return true;
        }

        public static bool SaveImage(Stream imageStream, string destName)
        {
            try
            {
                var destination = AppDomain.CurrentDomain.BaseDirectory + destName;
                // delete if exists
                if (File.Exists(destination))
                {
                    File.Delete(destination);
                }
                var image = Bitmap.FromStream(imageStream);
                image.Save(destination, destName.ToImageFormat());
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }
}
