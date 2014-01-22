namespace ImageCropper
{
    using System.Collections.Generic;
    using System.Linq;

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
    }
}
