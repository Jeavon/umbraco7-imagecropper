Using the v7 Image Cropper with ImageProcessor.Web
============================================

ImageProcessor for Umbraco can be installed by downloading this [package](http://our.umbraco.org/projects/collaboration/imageprocessor) or using NuGet `PM> Install-Package ImageProcessor.Web`

Processor Url
===========

Crop coordinates format: `{x1},{y1},{width},{height}`

Preview url format: `{mainimageurl}?crop={x1},{y1},{width},{height}&width={cropwidth}&quality={compression}`

`GetImageProcessorUrl` extension method
===========

To use the extension method add `@using ImageCropper.ImageProcessor` at the top of your view

Parameters:

- width
- height
- quality
- mode
- anchor
- imageCropperAlias
- imageCropperCropId
- furtherOptions
- slimmage

Razor Samples
===========

Example using strongly typed IPublishedContent

	@using ImageCropper.ImageProcessor
    @{
        var featureImage = Umbraco.TypedMedia(1082);
	    if (featureImage.HasCrop("imageCrop", "MainCrop"))
        {
	        <img src="@featureImage.GetImageProcessorUrl(width: 300, imageCropperAlias: "imageCrop", imageCropperCropId: "MainCrop")" />
        }
        else
        {
            <img src="@featureImage.GetImageProcessorUrl(width: 300, mode: ImageProcessor.Mode.Crop, anchor: ImageProcessor.Anchor.Top)" />
        }
    }      


Example using dynamic IPublishedContent

	@using ImageCropper.ImageProcessor
    @{
        var featureImage = Umbraco.Media(1082);
	    if (ImageProcessor.HasCrop(featureImage, "imageCrop", "MainCrop"))
        {
            <img src="@ImageProcessor.GetImageProcessorUrl(featureImage, width: 300, imageCropperAlias: "imageCrop", imageCropperCropId: "MainCrop")" />
        }
        else
        {
            <img src="@ImageProcessor.GetImageProcessorUrl(width: 300, mode: ImageProcessor.Mode.Crop, anchor: ImageProcessor.Anchor.Top)" />
        }
    }   

To enable Slimmage for adaptive width (ensure slimmage.min.js is included in your View)

    <img src="@featureImage.GetImageProcessorUrl(width: 300, imageCropperAlias: "imageCrop", imageCropperCropId: "MainCrop", slimmage:true)" />  