/// <reference path="../typings/bootstrap/bootstrap.d.ts" />
/// <reference path="../typings/timelinejs/timelinejs.d.ts" />
var getParameterByName = function (name) {
    name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
    var regexS = "[\\?&]" + name + "=([^&#]*)";
    var regex = new RegExp(regexS);
    var results = regex.exec(window.location.href);
    if (results == null)
        return "";
    else
        return decodeURIComponent(results[1].replace(/\+/g, " "));
};
var setGetParameter = function (paramName, paramValue) {
    var url = window.location.href;
    if (url.indexOf(paramName + "=") >= 0) {
        var prefix = url.substring(0, url.indexOf(paramName));
        var suffix = url.substring(url.indexOf(paramName));
        suffix = suffix.substring(suffix.indexOf("=") + 1);
        suffix = (suffix.indexOf("&") >= 0) ? suffix.substring(suffix.indexOf("&")) : "";
        url = prefix + paramName + "=" + paramValue + suffix;
    }
    else {
        if (url.indexOf("?") < 0)
            url += "?" + paramName + "=" + paramValue;
        else
            url += "&" + paramName + "=" + paramValue;
    }
    window.location.href = url;
};
var PhotoBlogApp;
(function (PhotoBlogApp) {
    var AboutVm = (function () {
        function AboutVm(target) {
            ko.applyBindings(this, target);
        }
        return AboutVm;
    }());
    PhotoBlogApp.AboutVm = AboutVm;
    var PostModalVm = (function () {
        function PostModalVm(modalId) {
            var _this = this;
            this.modalId = modalId;
            this.author = ko.observable("");
            this.title = ko.observable("");
            this.html = ko.observable("");
            this.urlReferer = "";
            this.postId = 0;
            this.inited = false;
            this.init = function () {
                var postModal = $("#" + _this.modalId);
                postModal.on("show.bs.modal", function (e) {
                    location.hash = _this.urlReferer + "/post/" + _this.postId;
                });
                postModal.on("hide.bs.modal", function (e) {
                    location.hash = "/";
                });
                _this.inited = true;
                postModal.modal("show");
            };
            this.open = function () {
                if (!_this.inited) {
                    _this.init();
                }
                else {
                    $("#" + _this.modalId).modal("show");
                }
            };
            this.close = function () {
                $("#" + _this.modalId).modal("hide");
            };
        }
        return PostModalVm;
    }());
    PhotoBlogApp.PostModalVm = PostModalVm;
    var BlogVm = (function () {
        function BlogVm(target, router) {
            var _this = this;
            this.router = router;
            this.postModal = new PostModalVm("postModal");
            this.posts = ko.observableArray([]);
            this.router.registerRoute("#/blog/post/:postId", function (context) {
                var post = _this.posts().filter(function (p) { return p.PostId === parseInt(context.params.postId); })[0];
                _this.openPostModal(post);
            });
            this.openPostModal = function (post) {
                _this.postModal.author(post.Author || "");
                _this.postModal.title(post.Title || "");
                _this.postModal.html(post.HTML || "");
                _this.postModal.postId = post.PostId;
                _this.postModal.urlReferer = "/blog";
                _this.postModal.open();
            };
            this.load = function () {
                PhotoBlogApp.Api.AsyncClient.Current.GetPosts(function (results) {
                    if (results != null) {
                        _this.posts(results.Posts);
                        createStoryJS({
                            type: "timeline",
                            width: "100%",
                            height: "880",
                            source: "/home/GetTimeline",
                            embed_id: "timeline-adventure",
                            start_at_end: true,
                            start_zoom_adjust: "3",
                            hash_bookmark: false,
                            debug: false,
                            maptype: "watercolor",
                            css: "/Content/css/timeline.css",
                            js: "/Scripts/timeline.js"
                        });
                        setTimeout(function () {
                            _this.router.start();
                        }, 100);
                    }
                });
            };
            ko.applyBindings(this, target);
        }
        return BlogVm;
    }());
    PhotoBlogApp.BlogVm = BlogVm;
    var PhotoModalVm = (function () {
        function PhotoModalVm(modalId, getPhotos) {
            var _this = this;
            this.modalId = modalId;
            this.getPhotos = getPhotos;
            this.title = ko.observable("");
            this.description = ko.observable("");
            this.photoUrl = ko.observable("");
            this.hasNext = ko.observable(true);
            this.hasPrev = ko.observable(true);
            this.urlReferer = "";
            this.inited = false;
            this.photoId = 0;
            this.openedPhoto = null;
            this.hasNextOrPrev = function () {
                var photos = _this.getPhotos();
                var indexPhoto = photos.indexOf(_this.openedPhoto);
                _this.hasPrev(photos[indexPhoto - 1] != null);
                _this.hasNext(photos[indexPhoto + 1] != null);
            };
            this.setPhoto = function (photo) {
                if (photo != null) {
                    _this.openedPhoto = photo;
                    _this.title(photo.AlbumName || "");
                    _this.description(photo.AlbumDescription || "");
                    _this.photoUrl(photo.FullUrl || "");
                    _this.photoId = photo.PhotoId;
                }
            };
            this.init = function () {
                var postModal = $("#" + _this.modalId);
                postModal.on("show.bs.modal", function (e) {
                    location.hash = _this.urlReferer + "/photo/" + _this.photoId;
                });
                postModal.on("hide.bs.modal", function (e) {
                    _this.photoUrl("/Content/css/loading.gif");
                    location.hash = "/";
                });
                _this.inited = true;
                postModal.modal("show");
            };
            this.open = function (photo) {
                _this.setPhoto(photo);
                _this.hasNextOrPrev();
                _this.urlReferer = "/photos";
                if (!_this.inited) {
                    _this.init();
                }
                else {
                    $("#" + _this.modalId).modal("show");
                }
            };
            this.close = function () {
                $("#" + _this.modalId).modal("hide");
            };
            this.prev = function () {
                var photos = _this.getPhotos();
                var indexPhoto = photos.indexOf(_this.openedPhoto);
                var prevPhoto = photos[indexPhoto - 1];
                _this.setPhoto(prevPhoto);
                _this.hasPrev(photos[indexPhoto - 2] != null);
                _this.hasNext(photos[indexPhoto] != null);
            };
            this.next = function () {
                var photos = _this.getPhotos();
                var indexPhoto = photos.indexOf(_this.openedPhoto);
                var nextPhoto = photos[indexPhoto + 1];
                _this.setPhoto(nextPhoto);
                _this.hasPrev(photos[indexPhoto] != null);
                _this.hasNext(photos[indexPhoto + 2] != null);
            };
        }
        return PhotoModalVm;
    }());
    PhotoBlogApp.PhotoModalVm = PhotoModalVm;
    var PortfolioVm = (function () {
        function PortfolioVm(target, router) {
            var _this = this;
            this.router = router;
            this.photoModal = ko.observable(null);
            this.albums = ko.observableArray([]);
            this.photos = [];
            this.countAlbumsLoaded = 0;
            this.getPhotos = function () {
                if (_this.photos.length === 0) {
                    _this.albums().forEach(function (a) {
                        _this.photos = _this.photos.concat(a.Photos);
                    });
                }
                return _this.photos;
            };
            this.photoModal(new PhotoModalVm("photoModal", this.getPhotos));
            this.router.registerRoute("#/photos/photo/:photoId", function (context) {
                var photo = null;
                _this.albums().forEach(function (a) {
                    if (photo != null) {
                        return;
                    }
                    else {
                        photo = a.Photos.filter(function (p) { return p.PhotoId === parseInt(context.params.photoId); })[0];
                    }
                });
                _this.openPhoto(photo);
            });
            this.afterRender = function () {
            };
            this.afterRenderAlbums = function () {
            };
            this.afterRenderPhotos = function () {
            };
            this.openPhoto = function (photo) {
                if (photo != null) {
                    _this.photoModal().open(photo);
                }
            };
            this.load = function () {
                PhotoBlogApp.Api.AsyncClient.Current.GetAlbums(function (results) {
                    _this.albums(results.AlbumMs);
                    setTimeout(function () { _this.router.start(); }, 100);
                });
            };
            ko.applyBindings(this, target);
        }
        return PortfolioVm;
    }());
    PhotoBlogApp.PortfolioVm = PortfolioVm;
})(PhotoBlogApp || (PhotoBlogApp = {}));
//# sourceMappingURL=landing.js.map