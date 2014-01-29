using System.Linq;
using System.Text;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace ImageCropper.ImageGen
{
    using System.Web;

    public static class ImageGen
    {
        public static bool HasCrop(this IPublishedContent publishedContent, string propertyAlias, string cropId)
        {
            return publishedContent.HasPropertyAndValueAndCrop(propertyAlias, cropId);
        }

        /// <summary>
        /// Generate an ImageGen URL from a Media Item
        /// </summary>
        /// <param name="mediaItem">The media item (iPublishedContent)</param>
        /// <param name="align">The horizontal alignment of the overlay image or text</param>
        /// <param name="allowUpsizing">Allow the image to be upsized</param>
        /// <param name="antiAlias">Boolean to allow the image to be Anti Aliased</param>
        /// <param name="border">Pass in a int to specifiy the border width</param>
        /// <param name="borderColor">The string of the Hexadecimal color value for the border color</param>
        /// <param name="colorMode">The colour mode of the image (colour, greyscale, sepia)</param>
        /// <param name="quality">A percentage scale of the amount of compression to apply to the image</param>
        /// <param name="constrain">A boolean if to constrain the width and height of the image</param>
        /// <param name="flip">Apply x, y or x & y flipping of the image</param>
        /// <param name="fontSize">An int of the size of the font to be applied to the image</param>
        /// <param name="fontStyle">The font style such as bold, italic to be applied to the image</param>
        /// <param name="font">The path or the name of the font installed on the server</param>
        /// <param name="fontColor">The string of the Hexadecimal color value for the font</param>/// 
        /// <param name="format">The output format of the image to be served, such as gif, jpb, png or tiff</param>
        /// <param name="height">The height of the image you want it to be resized to</param>
        /// <param name="lineHeight">An int of the lineheight used in conjuction with text & font params</param>
        /// <param name="maxHeight">An int of the max height of the image</param>
        /// <param name="maxWidth">An int of the max width of the image</param>
        /// <param name="noCache">A boolean to ensure the image is not cached</param>
        /// <param name="overlayMargin">An int to apply a margin to the overlay item, such as text or image</param>
        /// <param name="pad">An int to apply padding to the image</param>
        /// <param name="rotate">The number of degrees to rotate the image by</param>
        /// <param name="transparent">A bool to ensure the image is returned as transparent or not</param>
        /// <param name="vAlign">The vertical alignment of the overlay image or text</param>
        /// <param name="width">The width of the image you want it to be resized to</param>
        /// <param name="altImage">A path to an alternative image if the original image is not found</param>
        /// <param name="bgColor">The string of the Hexadecimal color value for the background of the image</param>
        /// <param name="overlayImage">A path to the image you wish to display over the top of the image, such as PNG watermark</param>
        /// <param name="text">The text you wish to display over the top of the image</param>
        /// <param name="imageCropperAlias">If using a custom crop, the alias of the cropper property</param>
        /// <param name="imageCropperCropId">If using a custom crop, the id of the corp to use</param>
        /// <param name="furtherOptions">If you need some further querystring option pass them here, e.g. &something=true</param>
        /// <param name="slimmage">Output the image for use with Slimmage</param>
        /// <returns>A string to the ImageGen Url with the parameters passed in</returns>
        public static string GetImageGenUrl(
            this IPublishedContent mediaItem,
            int? width = null,
            int? height = null,
            int? quality = null,
            Align? align = null,
            bool? allowUpsizing = null,
            bool? antiAlias = null,
            int? border = null,
            string borderColor = "",
            ColorMode? colorMode = null,
            bool? constrain = null,
            Flip? flip = null,
            int? fontSize = null,
            FontStyle? fontStyle = null,
            string font = null,
            string fontColor = null,
            Format? format = null,
            int? lineHeight = null,
            int? maxHeight = null,
            int? maxWidth = null,
            bool? noCache = null,
            int? overlayMargin = null,
            bool? pad = null,
            int? rotate = null,
            bool? transparent = null,
            VAlign? vAlign = null,
            string altImage = null,
            string bgColor = null,            
            string overlayImage = null,
            string text = null,
            string imageCropperAlias = null,
            string imageCropperCropId = null,
            string furtherOptions = null,
            bool slimmage = false)
        {
            string imageCropperValue = null;
            if (mediaItem.HasPropertyAndValueAndCrop(imageCropperAlias, imageCropperCropId))
            {
                imageCropperValue = mediaItem.GetPropertyValueHack(imageCropperAlias);
            }
            return mediaItem != null ? GetImageGenUrl(mediaItem.Url, width, height, quality, align, allowUpsizing, antiAlias, border, borderColor, colorMode, constrain, flip, fontSize, fontStyle, font, fontColor, format, lineHeight, maxHeight, maxWidth, noCache, overlayMargin, pad, rotate, transparent, vAlign, altImage, bgColor, overlayImage, text, imageCropperValue, imageCropperCropId, furtherOptions, slimmage) : string.Empty;
        }

        public static string GetImageGenUrl(
            this string imageUrl,
            int? width = null,
            int? height = null,
            int? quality = null,
            Align? align = null,
            bool? allowUpsizing = null,
            bool? antiAlias = null,
            int? border = null,
            string borderColor = null,
            ColorMode? colorMode = null,
            bool? constrain = null,
            Flip? flip = null,
            int? fontSize = null,
            FontStyle? fontStyle = null,
            string font = "",
            string fontColor = "",
            Format? format = null,
            int? lineHeight = null,
            int? maxHeight = null,
            int? maxWidth = null,
            bool? noCache = null,
            int? overlayMargin = null,
            bool? pad = null,
            int? rotate = null,
            bool? transparent = null,
            VAlign? vAlign = null,
            string altImage = null,
            string bgColor = null,
            string overlayImage = null,
            string text = null,
            string imageCropperValue = null,
            string imageCropperCropId = null,
            string furtherOptions = null,
            bool slimmage = false)
        {
            if (!string.IsNullOrEmpty(imageUrl))
            {
                var imageGenUrl = new StringBuilder();
                imageGenUrl.Append(string.Format("/ImageGen.ashx?Image={0}",imageUrl));

                if (!string.IsNullOrEmpty(imageCropperValue) && imageCropperValue.IsJson())
                {
                    var allTheCrops = imageCropperValue.GetImageCrops();
                    if (allTheCrops != null && allTheCrops.Any())
                    {
                        var crop = imageCropperCropId != null
                            ? allTheCrops.Find(x => x.Id == imageCropperCropId)
                            : allTheCrops.First();
                        if (crop != null)
                        {
                            imageGenUrl.Append(string.Format("&crop={0}",crop.CropCoOrds));
                        }
                    }
                }
                if (width != null)
                {                   
                    imageGenUrl.Append(string.Format("&width={0}",width));
                }
                if (height != null)
                {
                    imageGenUrl.Append(string.Format("&height={0}", width));
                }
                if (quality != null)
                {
                    imageGenUrl.Append(string.Format("&quality={0}", quality));
                }

                //Align (Left, Center, Right, Near, Far)
                if (align != null)
                {
                    imageGenUrl.Append(string.Format("&Align={0}", align));
                }
                //Allow Upsizing (True False)
                if (allowUpsizing != null)
                {
                    imageGenUrl.Append(string.Format("&AllowUpsizing={0}", allowUpsizing));
                }

                //Alt Image (/photos/waterfall.png)
                if (!string.IsNullOrEmpty(altImage))
                {
                    imageGenUrl.Append(string.Format("&AltImage={0}", altImage));
                }

                //AntiAlias (True of False)
                if (antiAlias != null)
                {
                    imageGenUrl.Append(string.Format("&AntiAlias={0}", antiAlias));
                }

                //BgColor (FFFFFF)
                if (!string.IsNullOrEmpty(bgColor))
                {
                    imageGenUrl.Append(string.Format("&BgColor={0}", bgColor));
                }

                //Color Mode (enum)
                if (colorMode != null)
                {
                    imageGenUrl.Append(string.Format("&ColorMode={0}", colorMode));
                }

                //Border (15)
                if (border != null)
                {
                    imageGenUrl.Append(string.Format("&Border={0}", border));
                }

                //BorderColor (000000)
                if (!string.IsNullOrEmpty(borderColor))
                {
                    imageGenUrl.Append(string.Format("&BorderColor={0}", borderColor));
                }

                //Color Mode (enum)
                if (colorMode != null)
                {
                    imageGenUrl.Append(string.Format("&ColorMode={0}", colorMode));
                }

                //Constrain (true/false)
                if (constrain != null)
                {
                    imageGenUrl.Append(string.Format("&Constrain={0}", constrain));
                }

                //Flip (x,y,xy)
                if (flip != null)
                {
                    imageGenUrl.Append(string.Format("&Flip={0}", flip));
                }

                //fontSize (000033)
                if (fontSize != null)
                {
                    imageGenUrl.Append(string.Format("&FontSize={0}", fontSize));
                }

                //FontStyle (Bold%2BItalic)
                if (fontStyle != null)
                {
                    imageGenUrl.Append(string.Format("&FontStyle={0}", fontStyle));
                }

                //Font (Verdana)
                if (font != null)
                {
                    imageGenUrl.Append(string.Format("&Font={0}", font));
                }

                //FontColor (000033)
                if (fontColor != null)
                {
                    imageGenUrl.Append(string.Format("&FontColor={0}", fontColor));
                }

                //Format (JPEG, JPG, GIF, PNG, BMP, TIFF, TIF)
                if (format != null)
                {
                    imageGenUrl.Append(string.Format("&Format={0}", format));
                }

                //LineHeight (40)
                if (lineHeight != null)
                {
                    imageGenUrl.Append(string.Format("&LineHeight={0}", lineHeight));
                }

                //MaxHeight (40)
                if (maxHeight != null)
                {
                    imageGenUrl.Append(string.Format("&MaxHeight={0}", maxHeight));
                }

                //MaxWidth (40)
                if (maxWidth != null)
                {
                    imageGenUrl.Append(string.Format("&MaxWidth={0}", maxWidth));
                }

                //NoCache (true/false)
                if (noCache != null)
                {
                    imageGenUrl.Append(string.Format("&NoCache={0}", noCache));
                }

                //OverlayMargin (8)
                if (overlayMargin != null)
                {
                    imageGenUrl.Append(string.Format("&OverlayMargin={0}", overlayMargin));
                }

                //Pad (true/false)
                if (pad != null)
                {
                    imageGenUrl.Append(string.Format("&Pad={0}", pad));
                }

                //Rotate (0 - 360) int with validation -360 to 360
                if (rotate != null)
                {
                    //Check values
                    //If larger than 360 set it to 360
                    if (rotate > 360)
                    {
                        rotate = 360;
                    }

                    //If larger than minus 360 set it to minus 360
                    if (rotate < -360)
                    {
                        rotate = -360;
                    }

                    imageGenUrl.Append(string.Format("&Rotate={0}", rotate));
                }

                //Transparent (Bool True of False)
                if (transparent != null)
                {
                    imageGenUrl.Append(string.Format("&Transparent={0}", transparent));
                }


                //VAlign (Enum Top, Middle, Bottom, Near, Far)
                if (vAlign != null)
                {
                    imageGenUrl.Append(string.Format("&VAlign={0}", vAlign));
                }

                //OverlayImage (/images/watermark.png)
                if (overlayImage != null)
                {
                    imageGenUrl.Append(string.Format("&OverlayImage={0}", overlayImage));
                }

                if (text != null)
                {
                    imageGenUrl.Append(string.Format("&Text={0}", HttpUtility.UrlEncode(HttpUtility.HtmlEncode(text))));
                }

                if (slimmage)
                {
                    if (width == null)
                    {
                        imageGenUrl.Append("&width=300");
                    }
                    if (quality == null)
                    {
                        imageGenUrl.Append("&quality=90");
                    }
                    imageGenUrl.Append("&slimmage=true");
                }
                if (furtherOptions != null)
                {
                    imageGenUrl.Append(furtherOptions);
                }

                return imageGenUrl.ToString();
            }
            return string.Empty;
        }

        public enum Align
        {
            /// <summary>
            /// Aligns image left
            /// </summary>
            Left,

            /// <summary>
            /// Aligns image center
            /// </summary>
            Center,

            /// <summary>
            /// Aligns image right
            /// </summary>
            Right,

            /// <summary>
            /// Aligns image near
            /// </summary>
            Near,

            /// <summary>
            /// Aligns image far
            /// </summary>
            Far
        }

        public enum ColorMode
        {
            /// <summary>
            /// Makes image in color
            /// </summary>
            Color,

            /// <summary>
            /// Makes image in Grayscale
            /// </summary>
            Grayscale,

            /// <summary>
            /// Makes image in Sepia
            /// </summary>
            Sepia
        }

        public enum VAlign
        {
            /// <summary>
            /// Aligns image top
            /// </summary>
            Top,

            /// <summary>
            /// Aligns image middle
            /// </summary>
            Middle,

            /// <summary>
            /// Aligns image bottom
            /// </summary>
            Bottom,

            /// <summary>
            /// Aligns image near
            /// </summary>
            Near,

            /// <summary>
            /// Aligns image far
            /// </summary>
            Far
        }

        public enum Flip
        {
            /// <summary>
            /// Flip image on X axis
            /// </summary>
            X,

            /// <summary>
            /// Flip image on Y axis
            /// </summary>
            Y,

            /// <summary>
            /// Flip image on X and Y axis
            /// </summary>
            // ReSharper disable once InconsistentNaming
            XY
        }

        public enum Format
        {
            /// <summary>
            /// Ouputs image as JPEG
            /// </summary>
            Jpeg,

            /// <summary>
            /// Ouputs image as JPG
            /// </summary>
            Jpg,

            /// <summary>
            /// Ouputs image as GIF
            /// </summary>
            Gif,

            /// <summary>
            /// Ouputs image as PNG
            /// </summary>
            Png,

            /// <summary>
            /// Ouputs image as BMP
            /// </summary>
            Bmp,

            /// <summary>
            /// Ouputs image as TIFF
            /// </summary>
            Tiff,

            /// <summary>
            /// Ouputs image as TIF 
            /// </summary>
            Tif
        }

        public enum FontStyle
        {
            /// <summary>
            /// Font weight - Regular
            /// </summary>
            Regular,

            /// <summary>
            /// Font weight - Bold
            /// </summary>
            Bold,

            /// <summary>
            /// Font weight - Italic
            /// </summary>
            Italic,

            /// /// <summary>
            /// Font weight - Underline
            /// </summary>
            Underline,

            /// <summary>
            /// Font weight - Strikeout 
            /// </summary>
            Strikeout
        }
    }
}
