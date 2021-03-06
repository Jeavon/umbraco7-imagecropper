﻿using System.Linq;
using System.Text;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace ImageCropper.ImageResizer
{
    public static class ImageResizer
    {
        public static bool HasCrop(this IPublishedContent publishedContent, string propertyAlias, string cropId)
        {
            return publishedContent.HasPropertyAndValueAndCrop(propertyAlias, cropId);
        }

        public static string GetImageResizerUrl(
            this IPublishedContent mediaItem,
            int? width = null,
            int? height = null,
            int? quality = null,
            Mode? mode = null,
            Anchor? anchor = null,
            string imageCropperAlias = null,
            string imageCropperCropId = null,
            string furtherOptions = null,
            bool slimmage = false)
        {
            string imageCropperValue = null;

            if (mediaItem.HasPropertyAndValueAndCrop(imageCropperAlias,imageCropperCropId))
            {
                imageCropperValue = mediaItem.GetPropertyValueHack(imageCropperAlias);
            }

            return mediaItem != null ? GetImageResizerUrl(mediaItem.Url, width, height, quality, mode, anchor, imageCropperValue, imageCropperCropId, furtherOptions, slimmage) : string.Empty;
        }

        public static string GetImageResizerUrl(
            this string imageUrl,
            int? width = null,
            int? height = null,
            int? quality = null,
            Mode? mode = null,
            Anchor? anchor = null,
            string imageCropperValue = null,
            string imageCropperCropId = null,
            string furtherOptions = null,
            bool slimmage = false)
        {
            if (!string.IsNullOrEmpty(imageUrl))
            {
                var imageResizerUrl = new StringBuilder();
                imageResizerUrl.Append(imageUrl);

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
                            imageResizerUrl.Append("?mode=" + Mode.Crop.ToString().ToLower() + "&crop=(" + crop.CropCoOrds + ")");
                        }
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

                if (quality != null)
                {
                    imageResizerUrl.Append("&quality=" + quality);
                }
                if (width != null)
                {
                    imageResizerUrl.Append("&width=" + width);
                }
                if (height != null)
                {
                    imageResizerUrl.Append("&height=" + height);
                }
                if (slimmage)
                {
                    if (width == null)
                    {
                        imageResizerUrl.Append("&width=300");
                    }
                    if (quality == null)
                    {
                        imageResizerUrl.Append("&quality=90");
                    }
                    imageResizerUrl.Append("&slimmage=true");
                }
                if (furtherOptions != null)
                {
                    imageResizerUrl.Append(furtherOptions);
                }

                return imageResizerUrl.ToString();

            }
            return string.Empty;
        }

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
    }
}
