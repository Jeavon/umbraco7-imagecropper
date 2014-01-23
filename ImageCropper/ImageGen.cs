using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageCropper
{
    using Umbraco.Core.Models;
    using Umbraco.Web;

    public static class ImageGen
    {

        public enum Align
        {
            Center,
            Right,
            Near,
            Far
        }

        public enum ColorMode
        {
            Color,
            GrayScale,
            Sepia
        }

        public static string GetImageGenUrl(
            this IPublishedContent mediaItem,
            int? width = null,
            int? height = null,
            Align? align = null,
            ColorMode? colorMode = null,
            string imageCropperAlias = null,
            string imageCropperCropId = null,
            bool slimmage = false)
        {
            string imageCropperValue = null;
            if (imageCropperAlias != null && mediaItem.HasProperty(imageCropperAlias) && mediaItem.HasValue(imageCropperAlias))
            {
                imageCropperValue = mediaItem.GetPropertyValue<string>(imageCropperAlias);
            }
            return mediaItem != null ? GetImageResizerUrl(mediaItem.Url, width, height, align, colorMode, imageCropperValue, imageCropperCropId, slimmage) : string.Empty;
        }

        public static string GetImageResizerUrl(
            this string imageUrl,
            int? width = null,
            int? height = null,
            Align? align = null,
            ColorMode? colorMode = null,
            string imageCropperValue = null,
            string imageCropperCropId = null,
            bool slimmage = false)
        {
            var imageGenUrl = new StringBuilder();

            imageGenUrl.Append("/ImageGen.ashx?Image=" + imageUrl);

            bool widthSet = false, heightSet = false;

            if (!string.IsNullOrEmpty(imageCropperValue) && imageCropperValue.Length > 2)
            {
                var allTheCrops = imageCropperValue.GetImageCrops();
                if (allTheCrops != null && allTheCrops.Any())
                {
                    var crop = imageCropperCropId != null
                                   ? allTheCrops.Find(x => x.Id == imageCropperCropId)
                                   : allTheCrops.First();

                    imageGenUrl.Append("&crop=" + crop.CropCoOrds);
                }
            }
            if (width != null)
            {
                imageGenUrl.Append("&width=" + width);
            }
            if (align != null)
            {
                imageGenUrl.Append("&align=" + align.ToString().ToLower());
            }
            if (colorMode != null)
            {
                imageGenUrl.Append("&colorMode=" + colorMode.ToString().ToLower());
            }
            if (slimmage)
            {
                imageGenUrl.Append("&slimmage=true");
            }

            return imageGenUrl.ToString();
        }
         
 
    }
}
