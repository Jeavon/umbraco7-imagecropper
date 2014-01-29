Image Cropper Property Editor
============================================

As well as the Cropper iteself, this package includes easy to use extension methods for all three image processors and also Slimmage/Slimresponse for easy implementation using any of the processors.

Umbraco v7.1 is planned to include a Image Cropper as standard so this package may become obsolete :)

Alpha release, there will probably be errors.

Requirements
============
1. Umbraco 7.0.2
2. **[ImageProcessor.Web](http://our.umbraco.org/projects/collaboration/imageprocessor)** or **[ImageResizer](http://our.umbraco.org/projects/developer-tools/imageresizer)** by Imageresizing.net or **[ImageGen](http://our.umbraco.org/projects/website-utilities/imagegen)**

Source Code Test Sites
============
There is a test site for each image processor.

username: admim, password: password

**Running in WebMatrix** you need to copy the **App_Plugins** folder from the **ImageCropper** folder into the **TestSite** folder before running.

Package Installation
============

1. Install a server side image processor ([ImageProcessor](http://our.umbraco.org/projects/collaboration/imageprocessor), [ImageResizer](http://our.umbraco.org/projects/developer-tools/imageresizer) or [ImageGen](http://our.umbraco.org/projects/website-utilities/imagegen))
2. Install the v7 Image Cropper package
3. Clear your browser cache (otherwise you will get JS errors)
3. Create a new data type of type "Image Cropper" in the "Developer" section in the Umbraco backoffice.
4. Add the predefined crops settings.
5. Configure the crop coordinates and preview urls for your image processor (see processor documentation links)
5. Assign the data type to an image mediatype in the "Settings" section of the Umbraco backoffice.
6. Add some media items and set the crops
7. Use the extension class to display the crops you have made in the property editor.

Installation video (shows how to resolve below issue also!)
 
[http://www.screenr.com/NV8N](http://www.screenr.com/NV8N)

**Known issue:**

If you add a cropper property (or any additional property) to an existing media type and try to use it you may get a YSOD.

We have added a useful extension method `.HasCrop("propertyAlias","cropID")` to check if the crop has been created, this method will also attempt to workaround the issue automatically.

To fully resolve the issue stop your application and delete all Examine Indexes (all sub folders within \App_Data\TEMP\ExamineIndexes), start up again and it should now be ok, this will fully recreate the media cache.

There is an Umbraco issue logged for this [http://issues.umbraco.org/issue/U4-4129 ](http://issues.umbraco.org/issue/U4-4129)


Processor documentation
===========

There are extension methods for each processor included in the package for use with the cropper or standalone.

**ImageProcessor** - documentation [here](https://github.com/Jeavon/umbraco7-imagecropper/blob/master/Docs/ImageProcessor.md)


**ImageResizer** - documentation [here](https://github.com/Jeavon/umbraco7-imagecropper/blob/master/Docs/ImageResizer.md)


**ImageGen** - documentation [here](https://github.com/Jeavon/umbraco7-imagecropper/blob/master/Docs/ImageGen.md)

Videos
===========
Video showing how to enable Slimmage [http://www.screenr.com/2V8N](http://www.screenr.com/2V8N)


Credits and licenses
===========

Joost van den Berg who created the original v7 Angluar JS image cropper upon which this package is based.

Warren Buckley who created the UmbracoExtensionMethods from which the ImageGen extension method is derived.

Nathanael Jones who created both Slimmage and SlimResponse which are included in the package. Both project are MIT/Apache dual licensed by Imazen.