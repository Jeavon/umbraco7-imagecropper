using System.Linq;
using System.Text;

namespace ImageCropper
{
    using Umbraco.Core.Models;

    public static class ImageResizer
    {
        public enum Mode
        {
            Crop,
            Max,
            Strech,
            Pad,
            Carve
        }

        public enum Anchor
        {
            TopLeft,

            TopCenter,

            TopRight,

            MiddleLeft,

            MiddleCenter,

            MiddleRight,

            BottomLeft,

            BottomCenter,

            BottomRight
        }

        public static string GetImageResizerUrl(
            this IPublishedContent mediaItem,
            int? width = null,
            int? height = null,
            Mode? mode = null,
            Anchor? anchor = null,
            string imageCropperValue = null,
            string imageCropperCropId = null,
            bool slimmage = false)
        {
            return mediaItem != null ? GetImageResizerUrl(mediaItem.Url, width, height, mode, anchor, imageCropperValue, imageCropperCropId, slimmage) : string.Empty;
        }

        public static string GetImageResizerUrl(
            this string imageUrl,
            int? width = null,
            int? height = null,
            Mode? mode = null,
            Anchor? anchor = null,
            string imageCropperValue = null,
            string imageCropperCropId = null,
            bool slimmage = false)
        {
            if (!string.IsNullOrEmpty(imageUrl))
            {

                var imageResizerUrl = new StringBuilder();
                imageResizerUrl.Append(imageUrl);

                bool widthSet = false, heightSet = false;

                if (!string.IsNullOrEmpty(imageCropperValue) && imageCropperValue.Length > 2)
                {
                    var allTheCrops = imageCropperValue.GetImageCrops();
                    if (allTheCrops != null && allTheCrops.Any())
                    {
                        var crop = imageCropperCropId != null
                                       ? allTheCrops.Find(x => x.Id == imageCropperCropId)
                                       : allTheCrops.First();
                        if (crop.ProcessorUrl.Contains("width="))
                        {
                            widthSet = true;
                        }
                        if (crop.ProcessorUrl.Contains("height="))
                        {
                            heightSet = true;
                        }
                        imageResizerUrl.Append("?mode=" + Mode.Crop.ToString().ToLower() + "&" + crop.ProcessorUrl);
                    } 
                }
                else
                {
                    if (mode == null)
                    {
                        mode = Mode.Pad;
                    }
                    imageResizerUrl.Append("?mode=" + mode.ToString().ToLower());

                    if (anchor != null)
                    {
                        imageResizerUrl.Append("&anchor=" + anchor.ToString().ToLower());
                    }
                }
                if (width != null && !widthSet)
                {
                    imageResizerUrl.Append("&width=" + width);
                }
                if (height != null && !heightSet)
                {
                    imageResizerUrl.Append("&height=" + height);
                }
                if (slimmage)
                {
                    imageResizerUrl.Append("&slimmage=true");
                }
                return imageResizerUrl.ToString();

            }
            return string.Empty;
        }
    }
}
