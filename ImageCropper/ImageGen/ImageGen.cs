using System.Linq;
using System.Text;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace ImageCropper.ImageGen
{
    public static class ImageGen
    {
        public static string GetImageGenUrl(
            this IPublishedContent mediaItem,
            int? width = null,
            int? height = null,
            int? quality = null,
            Align? align = null,
            bool? allowUpsizing = null,
            bool? antiAlias = null,
            ColorMode? colorMode = null,
            string imageCropperAlias = null,
            string imageCropperCropId = null,
            string furtherOptions = null,
            bool slimmage = false)
        {
            string imageCropperValue = null;
            if (imageCropperAlias != null && mediaItem.HasProperty(imageCropperAlias) && mediaItem.HasValue(imageCropperAlias))
            {
                imageCropperValue = mediaItem.GetPropertyValue<string>(imageCropperAlias);
            }
            return mediaItem != null ? GetImageResizerUrl(mediaItem.Url, width, height, quality, align, allowUpsizing, antiAlias, colorMode, imageCropperValue, imageCropperCropId, furtherOptions, slimmage) : string.Empty;
        }

        public static string GetImageResizerUrl(
            this string imageUrl,
            int? width = null,
            int? height = null,
            int? quality = null,
            Align? align = null,
            bool? allowUpsizing = null,
            bool? antiAlias = null,
            ColorMode? colorMode = null,
            string imageCropperValue = null,
            string imageCropperCropId = null,
            string furtherOptions = null,
            bool slimmage = false)
        {
            if (!string.IsNullOrEmpty(imageUrl))
            {
                var imageGenUrl = new StringBuilder();
                imageGenUrl.Append("/ImageGen.ashx?Image=" + imageUrl);

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
                            imageGenUrl.Append("&crop=" + crop.CropCoOrds);
                        }
                    }
                }
                if (quality != null)
                {
                    imageGenUrl.Append("&quality=" + quality);
                }
                if (width != null)
                {
                    imageGenUrl.Append("&width=" + width);
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

                //AntiAlias (True of False)
                if (antiAlias != null)
                {
                    imageGenUrl.Append(string.Format("&AntiAlias={0}", antiAlias));
                }

                //Color Mode (enum)
                if (colorMode != null)
                {
                    imageGenUrl.Append(string.Format("&ColorMode={0}", colorMode));
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
            XY
        }

        public enum Format
        {
            /// <summary>
            /// Ouputs image as JPEG
            /// </summary>
            JPEG,

            /// <summary>
            /// Ouputs image as JPG
            /// </summary>
            JPG,

            /// <summary>
            /// Ouputs image as GIF
            /// </summary>
            GIF,

            /// <summary>
            /// Ouputs image as PNG
            /// </summary>
            PNG,

            /// <summary>
            /// Ouputs image as BMP
            /// </summary>
            BMP,

            /// <summary>
            /// Ouputs image as TIFF
            /// </summary>
            TIFF,

            /// <summary>
            /// Ouputs image as TIF 
            /// </summary>
            TIF
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
