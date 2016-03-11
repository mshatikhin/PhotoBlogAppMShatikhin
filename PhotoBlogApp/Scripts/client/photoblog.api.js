var PhotoBlogApp;
(function (PhotoBlogApp) {
    var Api;
    (function (Api) {
        var DataLoader = (function () {
            function DataLoader(url, httpMethod) {
                this.load = function (data, success) {
                    var jsonData = data == null ? null : ko.mapping.toJSON(data);
                    var settings = {
                        url: url,
                        dataType: "json",
                        type: httpMethod,
                        data: jsonData,
                        contentType: "application/json; charset=utf-8",
                        error: function () { alert('При запросе данных произошла ошибка'); },
                        beforeSend: function () {
                            jQuery("#photos-loader").show();
                            jQuery("#loader-image").show();
                        },
                        success: function (data) {
                            jQuery("#loader-image").hide();
                            success(data);
                        },
                        complete: function () {
                            jQuery("#photos-loader").hide();
                        }
                    };
                    jQuery.ajax(settings);
                };
            }
            return DataLoader;
        })();
        Api.DataLoader = DataLoader;
        var AsyncClient = (function () {
            function AsyncClient() {
            }
            AsyncClient.prototype.GetPhotos = function (getPhotosParams, success) {
                var dataLoader = new DataLoader("/getPhotos", "Post");
                dataLoader.load(getPhotosParams, function (result) {
                    success(result);
                });
            };
            AsyncClient.prototype.GetPosts = function (success) {
                var dataLoader = new DataLoader("/Home/getPosts", "Post");
                dataLoader.load(null, function (result) {
                    success(result);
                });
            };
            AsyncClient.prototype.GetAlbums = function (success) {
                var dataLoader = new DataLoader("/Home/getAlbums", "Post");
                dataLoader.load(null, function (result) {
                    success(result);
                });
            };
            AsyncClient.prototype.GetPhotoProjects = function (success) {
                var dataLoader = new DataLoader("/Home/getPhotoProjects", "Post");
                dataLoader.load(null, function (result) {
                    success(result);
                });
            };
            AsyncClient.Current = new AsyncClient();
            return AsyncClient;
        })();
        Api.AsyncClient = AsyncClient;
    })(Api = PhotoBlogApp.Api || (PhotoBlogApp.Api = {}));
})(PhotoBlogApp || (PhotoBlogApp = {}));
//# sourceMappingURL=photoblog.api.js.map