Image Cropper Property Editor
============================================

The Image Cropper property editor allows backoffice users to position pre-defined image crops on media items.
Using the ImageProcessor the images are manipulated on-the-fly.

Requirements
============
1. Umbraco 7.0.1
2. ImageProcessor.Web or Imageresizer by Imageresizing.net

Test Sites
============
username: admim, password: password

**Running in WebMatrix** you need to copy the **App_Plugins** folder from the **ImageCropper** folder into the **TestSite** folder before running.

Installation
============

1. Install ImageProcessor and ImageProcessor.Web using Nuget (http://jimbobsquarepants.github.io/ImageProcessor/).
2. Drop the folders from this repository into the corresponding folders of an Umbraco V7.0.1 instance.
3. Create a new data type of type "Image Cropper" in the "Developer" section in the Umbraco backoffice.
4. Add the predefined crops settings.
5. Assign the data type to an image mediatype in the "Settings" section of the Umbraco backoffice.
6. Add some crops to a media item and save.
7. Use the extension class to get the crops you have made in the property editor.

Disclaimers
===========

Initial release, there will probably be errors.


Contact me
==========

If you have tips or questions, give me a shout on twitter [@azertie](http://www.twitter.com/azertie).
