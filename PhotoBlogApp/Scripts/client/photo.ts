module PhotoBlogApp
{
    export class PhotoVm
    {
        fullscreen: () => void;
        photoId = "id_1";

        constructor(public photo: Api.PhotoM)
        {
            var guid = (() =>
            {
                function s4()
                {
                    return Math.floor((1 + Math.random()) * 0x10000)
                        .toString(16)
                        .substring(1);
                }
                return () => s4() + s4() + '-' + s4() + '-' + s4() + '-' +
                    s4() + '-' + s4() + s4() + s4();
            })();

            this.photoId = "photo_" + guid();

            this.fullscreen = () =>
            {
                var $img = $("#" + this.photoId);
                if ($img.length > 0) {
                    var imageWidth = $img.width(), //need the raw width due to a jquery bug that affects chrome
                        imageHeight = $img.height(), //need the raw height due to a jquery bug that affects chrome
                        maxWidth = $(window).width(),
                        maxHeight = $(window).height(),
                        widthRatio = maxWidth / Number(imageWidth),
                        heightRatio = maxHeight / Number(imageHeight);

                    var ratio = widthRatio; //default to the width ratio until proven wrong

                    if (widthRatio * Number(imageHeight) > Number(maxHeight)) {
                        ratio = heightRatio;
                    }

                    //now resize the image relative to the ratio
                    $img.attr('width', Number(imageWidth) * ratio)
                        .attr('height', Number(imageHeight) * ratio);

                    //and center the image vertically and horizontally
                    $img.css({
                        margin: 'auto',
                        position: 'absolute',
                        height: 'auto',
                        top: 0,
                        bottom: 0,
                        left: 0,
                        right: 0
                    });
                }
            };
        }
    }
} 