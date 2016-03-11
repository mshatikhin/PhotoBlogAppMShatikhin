var PhotoBlogApp;
(function (PhotoBlogApp) {
    var Util = (function () {
        function Util() {
        }
        Util.introAnimate = function () {
            //var titleEl = $("#intro_wrap__title");
            //if (titleEl.length > 0) {
            //    titleEl.animate({ 'marginTop': '0' }, 500);
            //}
            var links = $(".menu-links_js .link");
            var existActive = false;
            links.each(function (inxex, el) {
                var link = $(el);
                link.removeClass("active-link");
                if (link.attr("href") === location.hash) {
                    link.addClass("active-link");
                    existActive = true;
                }
            });
            if (!existActive) {
                links.first().addClass("active-link");
            }
        };
        return Util;
    }());
    PhotoBlogApp.Util = Util;
})(PhotoBlogApp || (PhotoBlogApp = {}));
//# sourceMappingURL=util.js.map