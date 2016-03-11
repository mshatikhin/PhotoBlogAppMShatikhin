var PhotoBlogApp;
(function (PhotoBlogApp) {
    var PhotoProjectVm = (function () {
        function PhotoProjectVm(photoProject, toogleDescription) {
            this.photoProject = photoProject;
            this.toogleDescription = toogleDescription;
            this.showDescription = ko.observable(false);
        }
        return PhotoProjectVm;
    }());
    PhotoBlogApp.PhotoProjectVm = PhotoProjectVm;
    var PhotoProjectsVm = (function () {
        function PhotoProjectsVm(target) {
            var _this = this;
            this.photoProjects = ko.observableArray([]);
            this.toogleDescription = function (projectVm) {
                var show = !projectVm.showDescription();
                _this.photoProjects().forEach(function (p) {
                    p.showDescription(false);
                });
                projectVm.showDescription(show);
            };
            this.load = function () {
                PhotoBlogApp.Api.AsyncClient.Current.GetPhotoProjects(function (photoProjects) {
                    _this.photoProjects(photoProjects.map(function (p) { return new PhotoProjectVm(p, _this.toogleDescription); }));
                });
            };
            ko.applyBindings(this, target);
        }
        return PhotoProjectsVm;
    }());
    PhotoBlogApp.PhotoProjectsVm = PhotoProjectsVm;
})(PhotoBlogApp || (PhotoBlogApp = {}));
//# sourceMappingURL=photoProjectsVm.js.map