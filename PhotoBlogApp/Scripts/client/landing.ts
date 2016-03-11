/// <reference path="../typings/bootstrap/bootstrap.d.ts" />
/// <reference path="../typings/timelinejs/timelinejs.d.ts" />

var getParameterByName = function (name)
{
    name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
    var regexS = "[\\?&]" + name + "=([^&#]*)";
    var regex = new RegExp(regexS);
    var results = regex.exec(window.location.href);
    if (results == null) return "";
    else return decodeURIComponent(results[1].replace(/\+/g, " "));
}

var setGetParameter = function (paramName, paramValue)
{
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
}

module PhotoBlogApp
{

    export class AboutVm
    {
        constructor(target: HTMLElement)
        {
            ko.applyBindings(this, target);
        }
    }

    export class PostModalVm
    {
        author = ko.observable("");
        title = ko.observable("");
        html = ko.observable("");
        open: () => void;
        close: () => void;

        urlReferer = "";
        postId = 0;
        inited = false;
        init: () => void;
        constructor(private modalId: string)
        {
            this.init = () =>
            {
                var postModal = $("#" + this.modalId);
                postModal.on("show.bs.modal", e =>
                {
                    location.hash = this.urlReferer + "/post/" + this.postId;
                });

                postModal.on("hide.bs.modal", e =>
                {
                    location.hash = "/";
                });
                this.inited = true;
                postModal.modal("show");
            }

            this.open = () =>
            {
                if (!this.inited) {
                    this.init();
                } else {
                    $("#" + this.modalId).modal("show");
                }
            }

            this.close = () =>
            {
                $("#" + this.modalId).modal("hide");
            }
        }
    }

    export class BlogVm
    {
        postModal = new PostModalVm("postModal");
        posts = ko.observableArray<Api.PostM>([]);
        load: () => void;
        openPostModal: (post: Api.PostM) => void;

        constructor(target: HTMLElement, private router: App.RouterVm)
        {
            this.router.registerRoute("#/blog/post/:postId",(context) =>
            {
                var post = this.posts().filter((p) => { return p.PostId === parseInt(context.params.postId) })[0];
                this.openPostModal(post);
            });

            this.openPostModal = (post) =>
            {
                this.postModal.author(post.Author || "");
                this.postModal.title(post.Title || "");
                this.postModal.html(post.HTML || "");

                this.postModal.postId = post.PostId;
                this.postModal.urlReferer = "/blog";
                this.postModal.open();
            }

            this.load = () =>
            {
                Api.AsyncClient.Current.GetPosts((results) =>
                {
                    if (results != null) {
                        this.posts(results.Posts);
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
                        setTimeout(() =>
                        {
                            this.router.start();
                        }, 100);
                    }
                });
            }

            ko.applyBindings(this, target);
        }

    }

    export class PhotoModalVm
    {
        title = ko.observable("");
        description = ko.observable("");
        photoUrl = ko.observable("");
        setPhoto: (photo: Api.PhotoM) => void;
        hasNext = ko.observable(true);
        hasPrev = ko.observable(true);
        hasNextOrPrev: () => void;
        prev: () => void;
        next: () => void;
        init: () => void;
        open: (photo: Api.PhotoM) => void;
        close: () => void;

        urlReferer = "";
        inited = false;
        photoId = 0;
        openedPhoto: Api.PhotoM = null;

        constructor(private modalId: string, private getPhotos: () => Api.PhotoM[])
        {
            this.hasNextOrPrev = () =>
            {
                var photos = this.getPhotos();
                var indexPhoto = photos.indexOf(this.openedPhoto);
                this.hasPrev(photos[indexPhoto - 1] != null);
                this.hasNext(photos[indexPhoto + 1] != null);
            }

            this.setPhoto = (photo) =>
            {
                if (photo != null) {
                    this.openedPhoto = photo;
                    this.title(photo.AlbumName || "");
                    this.description(photo.AlbumDescription || "");
                    this.photoUrl(photo.FullUrl || "");
                    this.photoId = photo.PhotoId;
                }
            }

            this.init = () =>
            {
                var postModal = $("#" + this.modalId);
                postModal.on("show.bs.modal", e =>
                {
                    location.hash = this.urlReferer + "/photo/" + this.photoId;
                });

                postModal.on("hide.bs.modal", e =>
                {
                    this.photoUrl("/Content/css/loading.gif");
                    location.hash = "/";
                });
                this.inited = true;
                postModal.modal("show");
            }

            this.open = (photo) =>
            {
                this.setPhoto(photo);
                this.hasNextOrPrev();
                this.urlReferer = "/photos";

                if (!this.inited) {
                    this.init();
                } else {
                    $("#" + this.modalId).modal("show");
                }
            }

            this.close = () =>
            {
                $("#" + this.modalId).modal("hide");
            }

            this.prev = () =>
            {
                var photos = this.getPhotos();
                var indexPhoto = photos.indexOf(this.openedPhoto);
                var prevPhoto = photos[indexPhoto - 1];
                this.setPhoto(prevPhoto);
                this.hasPrev(photos[indexPhoto - 2] != null);
                this.hasNext(photos[indexPhoto] != null);
            }

            this.next = () =>
            {
                var photos = this.getPhotos();
                var indexPhoto = photos.indexOf(this.openedPhoto);
                var nextPhoto = photos[indexPhoto + 1];
                this.setPhoto(nextPhoto);
                this.hasPrev(photos[indexPhoto] != null);
                this.hasNext(photos[indexPhoto + 2] != null);
            }
        }
    }



    export class PortfolioVm
    {
        photoModal = ko.observable<PhotoModalVm>(null);
        albums = ko.observableArray<Api.AlbumM>([]);
        getPhotos: () => Api.PhotoM[];
        photos: Api.PhotoM[] = [];
        load: () => void;
        afterRender: () => void;
        openPhoto: (photo: Api.PhotoM) => void;

        afterRenderAlbums: () => void;
        afterRenderPhotos: () => void;
        private countAlbumsLoaded = 0;

        constructor(target: HTMLElement, private router: App.RouterVm)
        {
            this.getPhotos = () =>
            {
                if (this.photos.length === 0) {
                    this.albums().forEach((a) =>
                    {
                        this.photos = this.photos.concat(a.Photos);
                    });
                }
                return this.photos;
            }

            this.photoModal(new PhotoModalVm("photoModal", this.getPhotos));

            this.router.registerRoute("#/photos/photo/:photoId",(context) =>
            {
                var photo = null;
                this.albums().forEach((a) =>
                {
                    if (photo != null) {
                        return;
                    } else {
                        photo = a.Photos.filter((p) => { return p.PhotoId === parseInt(context.params.photoId) })[0];
                    }
                });
                this.openPhoto(photo);
            });


            this.afterRender = () =>
            {
            }

            this.afterRenderAlbums = () =>
            {
            }

            this.afterRenderPhotos = () =>
            {
            }

            this.openPhoto = (photo) =>
            {
                if (photo != null) {
                    this.photoModal().open(photo);
                }
            }

            this.load = () =>
            {
                Api.AsyncClient.Current.GetAlbums((results) =>
                {
                    this.albums(results.AlbumMs);
                    setTimeout(() => { this.router.start(); }, 100);
                });
            }

            ko.applyBindings(this, target);
        }
    }
} 