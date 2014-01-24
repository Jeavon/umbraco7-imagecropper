Image Cropper Property Editor
============================================

The Image Cropper property editor allows backoffice users to position pre-defined image crops on media items.
Using the ImageProcessor the images are manipulated on-the-fly.

Requirements
============
1. Umbraco 7.0.2
2. **[ImageProcessor.Web](http://our.umbraco.org/projects/collaboration/imageprocessor)** or **[Imageresizer](http://imageresizing.net/)** by Imageresizing.net or **[ImageGen](http://our.umbraco.org/projects/website-utilities/imagegen)**

Test Sites
============
username: admim, password: password

**Running in WebMatrix** you need to copy the **App_Plugins** folder from the **ImageCropper** folder into the **TestSite** folder before running.

Package Installation
============

1. Install a server side image processor ([ImageProcessor](http://our.umbraco.org/projects/collaboration/imageprocessor), Imageresizer or [ImageGen](http://our.umbraco.org/projects/website-utilities/imagegen))
2. Install the ImageCropper package
3. Clear your browser cache (otherwise you will get JS errors)
3. Create a new data type of type "Image Cropper" in the "Developer" section in the Umbraco backoffice.
4. Add the predefined crops settings.
5. Configure the crop coordinates and preview urls (see below)
5. Assign the data type to an image mediatype in the "Settings" section of the Umbraco backoffice.
6. Add some media items and set the crops
7. Use the extension class to display the crops you have made in the property editor.

**Known issue:**

If you add a cropper property (or any additional property) to an existing media type and try to use it you may get a YSOD. To resolve the issue stop your application and delete all Examine Indexes (all sub folders within \App_Data\TEMP\ExamineIndexes), start up again and it should now be ok.

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


**ImageProcessor**

Add `@using ImageCropper.ImageProcessor` at the top of your view

For a manual crop, with a propertyAlias of "imageCrop" and a cropId of "mainCrop"

	@if (caseStudyImage.HasProperty("imageCrop") && caseStudyImage.HasValue("imageCrop"))
	{
	<img src="@caseStudyImage.GetImageProcessorUrl(width: 300, imageCropperAlias: "imageCrop", imageCropperCropId: "MainCrop")" />
	} 

To enable Slimmage for adaptive width (ensure slimmage.min.js is included in your View  and SlimResponse is installed)

    <img src="@caseStudyImage.GetImageProcessorUrl(width: 300, imageCropperAlias: "imageCrop", imageCropperCropId: "MainCrop", slimmage:true)" />  

**ImageResizer**

Add `@using ImageCropper.ImageResizer` at the top of your view

For a manual crop, with a propertyAlias of "imageCrop" and a cropId of "mainCrop"

	@if (caseStudyImage.HasProperty("imageCrop") && caseStudyImage.HasValue("imageCrop"))
	{
	<img src="@caseStudyImage.GetImageResizerUrl(width: 300, imageCropperAlias: "imageCrop", imageCropperCropId: "MainCrop")" />
	}

To enable Slimmage for adaptive width (ensure slimmage.min.js is included in your View  and SlimResponse is installed)

    <img src="@caseStudyImage.GetImageResizerUrl(width: 300, imageCropperAlias: "imageCrop", imageCropperCropId: "MainCrop", slimmage:true)" />

**ImageGen**

Add `@using ImageCropper.ImageGen` at the top of your view

For a manual crop, with a propertyAlias of "imageCrop" and a cropId of "mainCrop"

	@if (caseStudyImage.HasProperty("imageCrop") && caseStudyImage.HasValue("imageCrop"))
	{
	<img src="@caseStudyImage.GetImageGenUrl(width: 300, imageCropperAlias: "imageCrop", imageCropperCropId: "MainCrop")" />
	} 

To enable Slimmage for adaptive width (ensure slimmage.min.js is included in your View  and SlimResponse is installed)

    <img src="@caseStudyImage.GetImageGenUrl(width: 300, imageCropperAlias: "imageCrop", imageCropperCropId: "MainCrop", slimmage:true)" />

Disclaimers
===========

Alpha release, there will probably be errors.
