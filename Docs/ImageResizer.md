Using the v7 Image Cropper with ImageResizer
============================================

ImageResizer for Umbraco can be installed by downloading this [package](http://our.umbraco.org//projects/developer-tools/imageresizer) or using NuGet `PM> Install-Package ImageResizer.MvcWebConfig`

Processor Url
===========

Crop coordinates format: `{x1},{y1},{x2},{y2}`

Preview url format: `{mainimageurl}?crop=({x1},{y1},{x2},{y2})&width={cropwidth}&quality={compression}&cache=no`


`GetImageResizerUrl` extension method
===========

To use the extension method add `@using ImageCropper.ImageResizer` at the top of your view

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

	@using ImageCropper.ImageResizer
    @{
	    var featureImage = Umbraco.TypedMedia(1082);
	    if (featureImage.HasCrop("imageCrop", "MainCrop"))
	    {
	        <img src="@featureImage.GetImageResizerUrl(width: 300, imageCropperAlias: "imageCrop", imageCropperCropId: "MainCrop")" />
	    }
	    else
	    {
	        <img src="@featureImage.GetImageResizerUrl(width: 300, mode:ImageResizer.Mode.Crop, anchor:ImageResizer.Anchor.TopLeft)" />
	    }
    }      


Example using dynamic IPublishedContent

	@using ImageCropper.ImageResizer
    @{
	    var featureImage = Umbraco.Media(1082);
	    if (ImageResizer.HasCrop(featureImage, "imageCrop", "MainCrop"))
	    {
	        <img src="@ImageResizer.GetImageResizerUrl(featureImage, width: 300, imageCropperAlias: "imageCrop", imageCropperCropId: "MainCrop")" />
	    }
	    else
	    {
	        <img src="@ImageResizer.GetImageResizerUrl(featureImage, width: 300, mode:ImageResizer.Mode.Crop, anchor:ImageResizer.Anchor.TopLeft)" />
	    }
    }   

To enable Slimmage for adaptive width (ensure slimmage.min.js is included in your View)

    <img src="@featureImage.GetImageResizerUrl(width: 300, imageCropperAlias: "imageCrop", imageCropperCropId: "MainCrop", slimmage:true)" />