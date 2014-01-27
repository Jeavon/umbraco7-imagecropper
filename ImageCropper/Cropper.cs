namespace ImageCropper
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using global::Umbraco.Core.Logging;
    using global::Umbraco.Core.Models;
    using global::Umbraco.Web;

    using Newtonsoft.Json;

    using ImageCropper.Models;

    public static class Cropper
    {
        public static bool IsJson(this string input)
        {
            input = input.Trim();
            return input.StartsWith("{") && input.EndsWith("}") || input.StartsWith("[") && input.EndsWith("]");
        }

        public static ImageCrop GetImageCrop(this string json, string id)
        {
            var ic = new ImageCrop();
            if (IsJson(json))
            {
                try
                {
                    var imageCropperSettings = JsonConvert.DeserializeObject<List<ImageCrop>>(json);
                    ic = imageCropperSettings.First(p => p.Id == id);
                }
                catch {  }
            }
            return ic;
        }

        public static List<ImageCrop> GetImageCrops(this string json)
        {
            var imageCrops = new List<ImageCrop>();

            if (IsJson(json))
            {
                try
                {
                    imageCrops = JsonConvert.DeserializeObject<List<ImageCrop>>(json);
                }
                catch
                {
                   
                }
            }
            return imageCrops;
        }

        internal static bool HasPropertyValueDoubleCheck(IPublishedContent publishedContent, string propertyAlias)
        {            
            try
            {
                if (propertyAlias != null && publishedContent.HasProperty(propertyAlias)
                    && publishedContent.HasValue(propertyAlias))
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Warn<IPublishedContent>("Media cache is not happy - http://issues.umbraco.org/issue/U4-4146");
                var cropsProperty = publishedContent.Properties.FirstOrDefault(x => x.PropertyTypeAlias == propertyAlias);
                if (cropsProperty != null && !string.IsNullOrEmpty(cropsProperty.Value.ToString()))
                {
                    return true;
                }
            }
            return false;
        }

        internal static string GetPropertyValueHack(IPublishedContent publishedContent, string propertyAlias)
        {
            string propertyValue = null;
            try
            {
                if (propertyAlias != null && publishedContent.HasProperty(propertyAlias)
                    && publishedContent.HasValue(propertyAlias))
                {
                    propertyValue = publishedContent.GetPropertyValue<string>(propertyAlias);
                }
            }
            catch (Exception ex)
            {
                var cropsProperty = publishedContent.Properties.FirstOrDefault(x => x.PropertyTypeAlias == propertyAlias);
                if (cropsProperty != null)
                {
                    propertyValue = cropsProperty.Value.ToString();
                }
            }
            return propertyValue;
        }

    }
}
