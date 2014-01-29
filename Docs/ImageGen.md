Using the v7 Image Cropper with ImageGen
============================================

ImageGen can be installed by downloading this [package](http://our.umbraco.org/projects/website-utilities/imagegen) 

Processor Url
===========

Crop coordinates format: `{x1},{y1},{width},{height}`

Preview url format: `/imagegen.ashx?image={mainimageurl}&crop={x1},{y1},{width},{height}&width={cropwidth}&quality={compression}&nocache=true`

`GetImageGenUrl` extension method
===========

To use the extension method add `@using ImageCropper.ImageGen` at the top of your view

Parameters:

- width
- height
- quality
- align
- allowUpsizing
- antiAlias
- border
- borderColor
- colorMode
- constrain
- flip
- fontSize
- fontStyle
- font
- fontColor
- format
- lineHeight
- maxHeight
- maxWidth
- noCache
- overlayMargin
- pad
- rotate
- transparent
- vAlign
- altImage
- bgColor         
- overlayImage
- text
- imageCropperAlias
- imageCropperCropId
- furtherOptions
- slimmage

Razor Samples
===========

Example using strongly typed IPublishedContent

	@using ImageCropper.ImageGen
    @{
        var featureImage = Umbraco.TypedMedia(1082);
	    if (featureImage.HasCrop("imageCrop", "MainCrop"))
        {
            <img src="@featureImage.GetImageGenUrl(width: 300, imageCropperAlias: "imageCrop", imageCropperCropId: "MainCrop")" />
        }
        else
        {
            <img src="@featureImage.GetImageGenUrl(300, align:ImageGen.Align.Near, vAlign:ImageGen.VAlign.Top)" />
        }
    }      


Example using dynamic IPublishedContent

	@using ImageCropper.ImageGen
    @{
        var featureImage = Umbraco.Media(1082);
	    if (ImageGen.HasCrop(featureImage, "imageCrop", "MainCrop"))
        {
            <img src="@ImageGen.GetImageGenUrl(featureImage, width: 300, imageCropperAlias: "imageCrop", imageCropperCropId: "MainCrop")" />
        }
        else
        {
            <img src="@ImageGen.GetImageGenUrl(300, align:ImageGen.Align.Near, vAlign:ImageGen.VAlign.Top)" />
        }
    }   

To enable Slimmage for adaptive width (ensure slimmage.min.js is included in your View)

    <img src="@featureImage.GetImageGenUrl(width: 300, imageCropperAlias: "imageCrop", imageCropperCropId: "MainCrop", slimmage:true)" />