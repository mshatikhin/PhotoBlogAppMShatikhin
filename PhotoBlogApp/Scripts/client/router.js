/// <reference path="../typings/sammyjs/sammyjs.d.ts" />
var App;
(function (App) {
    var RouterVm = (function () {
        function RouterVm() {
            var _this = this;
            this.app = Sammy();
            this.links = ["promo", "about", "blog", "photos"];
            this.scrollTo = function (id) {
                $("html, body").stop().animate({ scrollTop: $("#" + id).offset().top }, 400);
                $(".menu-nav__link-active").removeClass("menu-nav__link-active");
                $(".menu-nav").find("a[href='#/" + id + "']").addClass("menu-nav__link-active");
                return false;
            };
            this.links.forEach(function (link) {
                _this.app.get("#/" + link, function () {
                    _this.scrollTo(link);
                    $(".menu-nav").find("a[href='#/" + link + "']").click(function () {
                        _this.scrollTo(link);
                    });
                });
            });
            this.registerRoute = function (route, callback) {
                _this.app.get(route, function (context) {
                    callback(context);
                });
            };
            this.start = function () {
                _this.app.run();
            };
        }
        return RouterVm;
    }());
    App.RouterVm = RouterVm;
})(App || (App = {}));
//# sourceMappingURL=router.js.map