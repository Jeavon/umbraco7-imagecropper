Using the v7 Image Cropper with ImageResizer
============================================

Processor Url
===========

Crop coordinates format: `{x1},{y1},{x2},{y2}`

Preview url format: `{mainimageurl}?crop=({x1},{y1},{x2},{y2})&width={cropwidth}&quality={compression}&cache=no`


Parameters for `GetImageResizerUrl` method
===========

- width 
- height 
- quality
- mode,
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
        if (featureImage.HasPropertyAndValue("imageCrop"))
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
        if (ImageResizer.HasPropertyAndValue(featureImage, "imageCrop"))
        {
            <img src="@ImageResizer.GetImageResizerUrl(featureImage, width: 300, imageCropperAlias: "imageCrop", imageCropperCropId: "MainCrop")" />
        }
        else
        {
            <img src="@ImageResizer.GetImageResizerUrl(featureImage, width: 300, mode:ImageResizer.Mode.Crop, anchor:ImageResizer.Anchor.TopLeft)" />
        }
    }   