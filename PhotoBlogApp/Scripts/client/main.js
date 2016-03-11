/// <reference path="../typings/jquery/jquery.d.ts" />
/// <reference path="../typings/knockout/knockout.d.ts" />
$(function () {
    var router = new App.RouterVm();
    var requestModal = $("#request-box");
    var mentionModal = $("#mention-box");
    requestModal.on("shown.bs.modal", function (e) {
        $("#Name").focus();
    });
    requestModal.on("hide.bs.modal", function (e) {
        location.hash = "#/";
    });
    mentionModal.on("shown.bs.modal", function (e) {
        $("#Name").focus();
    });
    mentionModal.on("hide.bs.modal", function (e) {
        location.hash = "#/";
    });
    router.registerRoute("#/request", function (context) {
        requestModal.modal("show");
    });
    router.registerRoute("#/mention", function (context) {
        mentionModal.modal("show");
    });
    router.start();
    //$(document).on("mousedown", "img", function (e)
    //{
    //    if (e.which === 3) {
    //        $(this).on("contextmenu", ev =>
    //        {
    //            alert("Hey, Thats My Picture!");
    //            ev.preventDefault();
    //        });
    //    };
    //});
    var menunavBlock = $(".navigation");
    menunavBlock.find("a").each(function (i, el) {
        var url = $(el).attr("href");
        var pathname = window.location.pathname;
        console.log(pathname);
        var isMain = pathname === "/" && url.indexOf("main") !== -1;
        var isActive = pathname.indexOf(url) === 0;
        if (isActive || isMain) {
            $(el).addClass("navigation__link-active");
        }
    });
});
//# sourceMappingURL=main.js.map