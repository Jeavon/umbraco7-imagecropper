angular.module("umbraco").controller("ImageCropper",
    function ($scope, $routeParams, assetsService, mediaResource, imageHelper, angularHelper, dialogService) {
        var jcrop_api;

        var config = {
            items: [],
            multiple: true
        };

        $scope.scaleStep = 30;

        String.prototype.format = function (args) {
            var newStr = this;
            for (var key in args) {
                newStr = newStr.replace('{' + key + '}', args[key]);
            }
            return newStr;
        }

        $scope.openPreview = function () {
            dialogService.open({
                template: "/App_Plugins/ImageCropper/imagecropperpreview.html", scope: $scope
            });
        }

        $scope.scaleImage = function (type) {
            scaleImg(type);
        }

        function scaleImg(type) {

            var currentimagewidth = $scope.resizeimagewidth != 0 ? $scope.resizeimagewidth : $scope.mainimagewidth;
            var currentimageheight = $scope.resizeimageheight != 0 ? $scope.resizeimageheight : $scope.mainimageheight;
            var newwidth = currentimagewidth;

            if (type == "+") {
                newwidth = newwidth + $scope.scaleStep;
            }
            else {
                newwidth = newwidth - $scope.scaleStep;
            }

            var newheight = Math.round(newwidth / $scope.mainimageratio);

            removeCurrent();
            destroyJcrop();

            $scope.resizeimagewidth = newwidth;
            $scope.resizeimageheight = newheight;

            $('#mainimage').attr("style", "width:" + newwidth + "px; height:" + newheight + "px");
            $('#mainimage').attr("title", "width:" + newwidth + "px - height:" + newheight + "px");

            if ($scope.cropsetting != undefined || $scope.cropsetting != null) {
                selectorclick();
            }

        }

        function updPreview() {
            $scope.showPreview = $scope.model.value.length > 0;
            $scope.showReset = $scope.model.value.length > 0;
            $scope.info = " (" + $scope.model.value.length + "/" + $scope.model.config.items.length + ")";
        }

        updPreview();

        $scope.reset = function () {
            $scope.model.value = [];
            $scope.cropsetting = "";
            $('#mainimage').removeAttr("width");
            $('#mainimage').removeAttr("height");
            $('#mainimage').removeAttr("style");
            updPreview();
            destroyJcrop();
            $scope.resizeimageheight = 0;
            $scope.resizeimagewidth = 0;
        }

        $scope.selectAction = function () {
            selectorclick();
        };

        function selectorclick() {
            if ($scope.cropsetting != null) {
                destroyJcrop();
                var w = $scope.cropsetting.width;
                var h = $scope.cropsetting.height;

                var srcw = $scope.resizeimagewidth == 0 ? $scope.mainimagewidth : $scope.resizeimagewidth;
                var srch = $scope.resizeimageheight == 0 ? $scope.mainimageheight : $scope.resizeimageheight;

                if ((w < srcw) && (h < srch)) {
                    var x1 = 0, y1 = 0, x2 = 0, y2 = 0, croppedWidth = 0;
                    for (i = 0; i < $scope.model.value.length; i++) {
                        if ($scope.model.value[i].id === $scope.cropsetting.id) {
                            x1 = $scope.model.value[i].x1;
                            x2 = $scope.model.value[i].x2;
                            y1 = $scope.model.value[i].y1;
                            y2 = $scope.model.value[i].y2;
                            croppedWidth = $scope.model.value[i].resizewidth;
                        }
                    }

                    if (croppedWidth > 0) {
                        var newheight = Math.round(croppedWidth / $scope.mainimageratio);

                        removeCurrent();

                        $scope.resizeimagewidth = croppedWidth;
                        $scope.resizeimageheight = newheight;

                        $('#mainimage').attr("style", "width:" + croppedWidth + "px; height:" + newheight + "px");
                        $('#mainimage').attr("title", "width:" + croppedWidth + "px - height:" + newheight + "px");
                    }

                    createJcrop(w, h);

                    if (x1 != 0) {
                        jcrop_api.animateTo([x1, y1, x2, y2]);
                    }

                    updPreview();
                }
                else {
                    $scope.info = "Crop width or height does not fit..";
                }

            }
            else {
                destroyJcrop();
            }
        }

        function destroyJcrop() {
            try {
                jcrop_api.destroy();
            }
            catch (err) {
            }
        }

        function createJcrop(w, h) {
            $('#mainimage').Jcrop({
                minSize: [w, h],
                maxSize: [w, h],
                onChange: updCoords,
                bgOpacity: 0.3
            }, function () {
                jcrop_api = this;
            });

        }

        function removeCurrent() {
            if ($scope.cropsetting != null) {
                for (i = 0; i < $scope.model.value.length; i++) {
                    if ($scope.model.value[i].id == $scope.cropsetting.id) {
                        $scope.model.value.splice(i, 1);
                    }
                }
            }
        }

        function updCoords(c) {
            var currentOptionComp = $scope.cropsetting.compression;
            for (i = 0; i < $scope.model.value.length; i++) {
                if ($scope.model.value[i].id == $scope.cropsetting.id) {
                    $scope.model.value.splice(i, 1);
                }
            }
            var w = $scope.cropsetting.width;
            var h = $scope.cropsetting.height;
            var url = "";
            if (parseInt($scope.resizeimagewidth) > 0) {
                var scaleRatio = $scope.mainimagewidth / $scope.resizeimagewidth;
                url = $scope.model.config.urlformatresize.format({ resizewidth: $scope.resizeimagewidth, x1: c.x, y1: c.y, width: w, height: h, compression: currentOptionComp, orginalx1: Math.round(c.x * scaleRatio), orginaly1: Math.round(c.y * scaleRatio), orginalx2: Math.round(c.x2 * scaleRatio), orginaly2: Math.round(c.y2 * scaleRatio) });
            }
            else {
                url = $scope.model.config.urlformat.format({ x1: c.x, y1: c.y, x2: c.x2, y2:c.y2, width: w, height: h, compression: currentOptionComp });
            }
            $scope.model.value.push({ id: $scope.cropsetting.id, x1: c.x, y1: c.y, x2: c.x2, y2: c.y2, widthoriginal: $scope.mainimagewidth, heightoriginal: $scope.mainimageheight, widthdisplay: $scope.cropsetting.width, heightdisplay: $scope.cropsetting.height, compression: currentOptionComp, processorurl: url, resizewidth: $scope.resizeimagewidth });
            updPreview();
            //console.log(JSON.stringify($scope.model.value));
        }

        if (!angular.isArray($scope.model.value)) {
            $scope.model.value = [];
        }


        angular.extend(config, $scope.model.config);
        $scope.model.config = config;
        if (angular.isArray($scope.model.config.items)) {
            $scope.cropsettings = [];
            for (var i = 0; i < $scope.model.config.items.length; i++) {
                var c = $scope.model.config.items[i].value;
                var aCrop = c.split(";");
                $scope.cropsettings.push({ id: aCrop[0], name: aCrop[0] + " (" + aCrop[1] + "x" + aCrop[2] + ")", width: aCrop[1], height: aCrop[2], compression: aCrop[3] });
            }
        }
        else if (!angular.isObject($scope.model.config.items)) {
            throw "The items property an array";
        }

        assetsService.load(["/App_Plugins/ImageCropper/jquery.jcrop.min.js"]).then(function () {
            var cId = $routeParams.id;

            mediaResource.getById(cId).then(function (media) {
                var myMedia = media;
                var props = [];
                if ({ imageModel: myMedia }.imageModel.properties) {
                    props = { imageModel: myMedia }.imageModel.properties;
                } else {
                    $({ imageModel: myMedia }.imageModel.tabs).each(function (i, tab) {
                        props = props.concat(tab.properties);
                    });
                }
                var imageProp = _.find(props, function (item) {
                    if (item.alias === "umbracoWidth") {

                        $scope.showCropper = true;
                        $scope.mainimagewidth = parseInt(item.value)
                        $scope.resizeimagewidth = 0;
                    }
                    if (item.alias === "umbracoHeight") {
                        $scope.mainimageheight = parseInt(item.value);
                        $scope.resizeimageheight = 0;
                    }
                });

                var myMediaUrl = imageHelper.getImagePropertyValue({ imageModel: myMedia });
                $('#mainimage').attr("src", myMediaUrl);
                $scope.mainimageratio = $scope.mainimagewidth / $scope.mainimageheight;
                $scope.mainimageurl = myMediaUrl;

                var ratio = $scope.mainimageheight / $scope.mainimagewidth,
                    cWidth = $(".umb-panel-body").width() - 300,
                    cHeight = $(window).height() - 300;

                if ($scope.mainimagewidth >= $scope.mainimageheight) {

                    if ($scope.mainimagewidth > cWidth) {
                        setWidth();
                    }

                }
                else if ($scope.mainimagewidth < $scope.mainimageheight) {

                    if ($scope.mainimageheight > cHeight) {
                        setHeight();
                    }

                }

                function setWidth() {
                    var x = cWidth,
                        y = $scope.mainimagewidth,
                        z = x / y * 100,
                        p = z / 100 * y,
                        h = Math.floor(p * ratio),
                        w = 0;

                    if (h > cHeight) {
                        var perc = Math.abs((cHeight / h * 100));
                        h = perc / 100 * h;
                        w = Math.floor(h / ratio);
                        $('#mainimage').css("height", h); // Needed to overrite css height rule
                    } else {
                        w = z / 100 * y;
                    }

                    $scope.scaledWidth = w;
                    $scope.scaledHeight = h;
                    $scope.resizeimagewidth = w;
                };

                function setHeight() {
                    var x = cHeight,
                        y = $scope.mainimageheight,
                        z = x / y * 100,
                        p = (z / 100 * y) - 100,
                        w = Math.floor(p / ratio);

                    $scope.scaledHeight = p;
                    $('#mainimage').css("height", p); // Needed to overrite css height rule

                    $scope.resizeimagewidth = w;
                };

            });
        });

    });
