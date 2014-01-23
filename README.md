Image Cropper Property Editor
============================================

The Image Cropper property editor allows backoffice users to position pre-defined image crops on media items.
Using the ImageProcessor the images are manipulated on-the-fly.

Requirements
============
1. Umbraco 7.0.2
2. **[ImageProcessor.Web](http://jimbobsquarepants.github.io/ImageProcessor/)** or **[Imageresizer](http://imageresizing.net/)** by Imageresizing.net or **[ImageGen](http://our.umbraco.org/projects/website-utilities/imagegen)**

Test Sites
============
username: admim, password: password

**Running in WebMatrix** you need to copy the **App_Plugins** folder from the **ImageCropper** folder into the **TestSite** folder before running.

Installation
============

1. Install a server side image processor (ImageProcessor, Imageresizer or ImageGen)
2. Drop the folders from this repository into the corresponding folders of an Umbraco V7.0.1 instance.
3. Create a new data type of type "Image Cropper" in the "Developer" section in the Umbraco backoffice.
4. Add the predefined crops settings.
5. Configure the crop coordinates and preview urls (see below)
5. Assign the data type to an image mediatype in the "Settings" section of the Umbraco backoffice.
6. Add some crops to a media item and save.
7. Use the extension class to get the crops you have made in the property editor.


Processor Urls
===========
**ImageProcessor**

Crop coordinates format: `{x1},{y1},{width},{height}`

Preview url format: `{mainimageurl}?crop={x1},{y1},{width},{height}&width={cropwidth}&quality={compression}`

**ImageResizer**

Crop coordinates format: `{x1},{y1},{x2},{y2}`

Preview url format: `{mainimageurl}?crop=({x1},{y1},{x2},{y2})&width={cropwidth}&quality={compression}`

**ImageGen**

Crop coordinates format: `{x1},{y1},{width},{height}`

Preview url format: `/imagegen.ashx?image={mainimageurl}&crop={x1},{y1},{width},{height}&width={cropwidth}&compression={compression}`

Disclaimers
===========

Alpha release, there will probably be errors.
