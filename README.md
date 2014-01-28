Image Cropper Property Editor
============================================

The Image Cropper property editor allows backoffice users to position pre-defined image crops on media items.
Using ImageProcessor, ImageResizer or ImageGen the images are manipulated on-the-fly.

Requirements
============
1. Umbraco 7.0.2
2. **[ImageProcessor.Web](http://our.umbraco.org/projects/collaboration/imageprocessor)** or **[ImageResizer](http://our.umbraco.org//projects/developer-tools/imageresizer)** by Imageresizing.net or **[ImageGen](http://our.umbraco.org/projects/website-utilities/imagegen)**

Test Sites
============
username: admim, password: password

**Running in WebMatrix** you need to copy the **App_Plugins** folder from the **ImageCropper** folder into the **TestSite** folder before running.

Package Installation
============

1. Install a server side image processor ([ImageProcessor](http://our.umbraco.org/projects/collaboration/imageprocessor), [ImageResizer](http://our.umbraco.org/projects/developer-tools/imageresizer) or [ImageGen](http://our.umbraco.org/projects/website-utilities/imagegen))
2. Install the v7 Image Cropper package
3. Clear your browser cache (otherwise you will get JS errors)
3. Create a new data type of type "Image Cropper" in the "Developer" section in the Umbraco backoffice.
4. Add the predefined crops settings.
5. Configure the crop coordinates and preview urls (see below)
5. Assign the data type to an image mediatype in the "Settings" section of the Umbraco backoffice.
6. Add some media items and set the crops
7. Use the extension class to display the crops you have made in the property editor.

Installation video (shows how to resolve below issue also!)
 
[
http://www.screenr.com/NV8N](http://www.screenr.com/NV8N)

**Known issue:**

If you add a cropper property (or any additional property) to an existing media type and try to use it you may get a YSOD.

We have added a extension method `.HasPropertyAndValue("propertyAlias")` which will attempt to workaround the issue automatically.  It is also useful as it will check for empty Json values where the crops have been removed and return false where a normal HasValue would have returned true.

To fully resolve the issue stop your application and delete all Examine Indexes (all sub folders within \App_Data\TEMP\ExamineIndexes), start up again and it should now be ok, this will fully recreate the media cache.

There is an Umbraco issue logged for this [http://issues.umbraco.org/issue/U4-4129 ](http://issues.umbraco.org/issue/U4-4129)


Processor Urls
===========
**ImageProcessor**

Crop coordinates format: `{x1},{y1},{width},{height}`

Preview url format: `{mainimageurl}?crop={x1},{y1},{width},{height}&width={cropwidth}&quality={compression}`

**ImageResizer**

Crop coordinates format: `{x1},{y1},{x2},{y2}`

Preview url format: `{mainimageurl}?crop=({x1},{y1},{x2},{y2})&width={cropwidth}&quality={compression}&cache=no`

**ImageGen**

Crop coordinates format: `{x1},{y1},{width},{height}`

Preview url format: `/imagegen.ashx?image={mainimageurl}&crop={x1},{y1},{width},{height}&width={cropwidth}&quality={compression}&nocache=true`

Razor Samples
===========

There are extension methods for each processor included in the package for use with the cropper or standalone.

Video showing how to enable Slimmage [http://www.screenr.com/2V8N](http://www.screenr.com/2V8N)

**ImageProcessor**

Add `@using ImageCropper.ImageProcessor` at the top of your view

For a manual crop, with a propertyAlias of "imageCrop" and a cropId of "mainCrop"

	@{
	    var featureImage = Umbraco.TypedMedia(1082);
	    if (featureImage.HasPropertyAndValue("imageCrop"))
	    {
	        <img src="@featureImage.GetImageProcessorUrl(width: 300, imageCropperAlias: "imageCrop", imageCropperCropId: "MainCrop")" />
	    }
	}   

To enable Slimmage for adaptive width (ensure slimmage.min.js is included in your View)

    <img src="@featureImage.GetImageProcessorUrl(width: 300, imageCropperAlias: "imageCrop", imageCropperCropId: "MainCrop", slimmage:true)" />  

**ImageResizer** - further documentation [here](docs/ImageResizer.md)

Add `@using ImageCropper.ImageResizer` at the top of your view

For a manual crop, with a propertyAlias of "imageCrop" and a cropId of "mainCrop"
	
	@{
	    var featureImage = Umbraco.TypedMedia(1082);
	    if (featureImage.HasPropertyAndValue("imageCrop"))
	    {
	        <img src="@featureImage.GetImageResizerUrl(width: 300, imageCropperAlias: "imageCrop", imageCropperCropId: "MainCrop")" />
	    }
	}     

To enable Slimmage for adaptive width (ensure slimmage.min.js is included in your View)

    <img src="@featureImage.GetImageResizerUrl(width: 300, imageCropperAlias: "imageCrop", imageCropperCropId: "MainCrop", slimmage:true)" />

**ImageGen**

Add `@using ImageCropper.ImageGen` at the top of your view

For a manual crop, with a propertyAlias of "imageCrop" and a cropId of "mainCrop"

	@{
	    var featureImage = Umbraco.TypedMedia(1082);
	    if (featureImage.HasPropertyAndValue("imageCrop"))
	    {
	        <img src="@featureImage.GetImageGenUrl(width: 300, imageCropperAlias: "imageCrop", imageCropperCropId: "MainCrop")" />
	    }
	}     

To enable Slimmage for adaptive width (ensure slimmage.min.js is included in your View)

    <img src="@featureImage.GetImageGenUrl(width: 300, imageCropperAlias: "imageCrop", imageCropperCropId: "MainCrop", slimmage:true)" />

Disclaimers
===========

Alpha release, there will probably be errors.
