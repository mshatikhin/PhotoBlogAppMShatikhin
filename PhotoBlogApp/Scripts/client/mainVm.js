var PhotoBlogApp;
(function (PhotoBlogApp) {
    var ValueVm = (function () {
        function ValueVm(v, onSetValidForm) {
            var _this = this;
            this.v = v;
            this.onSetValidForm = onSetValidForm;
            this.isValid = ko.observable(false);
            this.value = ko.observable("");
            this.value(v);
            this.value.subscribe(function (newValue) {
                if (newValue.length > 0) {
                    _this.isValid(true);
                }
                else {
                    _this.isValid(false);
                }
                _this.onSetValidForm();
            });
        }
        return ValueVm;
    })();
    PhotoBlogApp.ValueVm = ValueVm;
    var AlbumVm = (function () {
        function AlbumVm(album) {
            this.album = album;
        }
        return AlbumVm;
    })();
    PhotoBlogApp.AlbumVm = AlbumVm;
    var PostVm = (function () {
        function PostVm(post) {
            this.post = post;
            this.isCollapsed = ko.observable(false);
            this.scroll = ko.observable(false);
            this.url = "";
            this.shareUrl = "";
        }
        return PostVm;
    })();
    PhotoBlogApp.PostVm = PostVm;
    var MainPagePhotoVm = (function () {
        function MainPagePhotoVm(url, fullUrl) {
            this.url = url;
            this.fullUrl = fullUrl;
        }
        return MainPagePhotoVm;
    })();
    PhotoBlogApp.MainPagePhotoVm = MainPagePhotoVm;
    var MainPhotosVm = (function () {
        function MainPhotosVm() {
            var _this = this;
            this.photoLeft = ko.observable(null);
            this.photoRight = ko.observable(null);
            this.images = [];
            this.shuffled = true;
            var random = function (min, max) {
                var num = Math.random() * (max - min) + min;
                return num.toFixed();
            };
            this.images = [
                new MainPagePhotoVm("//mshatikhin.ru/Content/img/main/img_2247_.jpg", "//mshatikhin.ru/Content/img/main/img_2247.jpg"),
                new MainPagePhotoVm("//mshatikhin.ru/Content/img/main/img_8367_.jpg", "//mshatikhin.ru/Content/img/main/img_8367.jpg"),
                new MainPagePhotoVm("//mshatikhin.ru/Content/img/main/img_3544_.jpg", "//mshatikhin.ru/Content/img/main/img_3544.jpg"),
                new MainPagePhotoVm("//mshatikhin.ru/Content/img/main/img_9431_.jpg", "//mshatikhin.ru/Content/img/main/img_9431.jpg")
            ];
            this.shuffle = function () {
                _this.photoLeft(_this.images[random(0, 1)]);
                _this.photoRight(_this.images[random(2, 3)]);
            };
            this.shuffle();
        }
        return MainPhotosVm;
    })();
    PhotoBlogApp.MainPhotosVm = MainPhotosVm;
    var MainVm = (function () {
        function MainVm(targetElem, hash) {
            var _this = this;
            this.hash = hash;
            this.imageUrl = ko.observable("");
            this.imageHeight = ko.observable(250);
            this.isValidForm = ko.observable(false);
            this.mainPhotosVm = new MainPhotosVm();
            this.setValidForm = function () {
                _this.isValidForm(_this.valueName().isValid() && _this.valueCoordinates().isValid());
            };
            this.valueName = ko.observable(new ValueVm("", this.setValidForm));
            this.valueCoordinates = ko.observable(new ValueVm("", this.setValidForm));
            this.init = function () {
                PhotoBlogApp.Util.introAnimate();
            };
            this.afterRender = function () {
                setTimeout(_this.init(), 1);
            };
            ko.applyBindings(this, targetElem);
            $(document).on("click", ".img_js", function (event) {
                _this.imageUrl("");
                var url = $(event.target).attr("data-fullimage");
                if (url === null || url === undefined || url === "") {
                    url = $(event.target).attr("src");
                }
                var height = $(window).height();
                var imageHeight = height - (height * 0.1);
                _this.imageUrl(url);
                _this.imageHeight(imageHeight);
            });
        }
        return MainVm;
    })();
    PhotoBlogApp.MainVm = MainVm;
})(PhotoBlogApp || (PhotoBlogApp = {}));
//# sourceMappingURL=mainVm.js.map