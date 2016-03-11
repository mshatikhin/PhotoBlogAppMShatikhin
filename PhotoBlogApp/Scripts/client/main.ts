/// <reference path="../typings/jquery/jquery.d.ts" />
/// <reference path="../typings/knockout/knockout.d.ts" />

$(() =>
{
    var router = new App.RouterVm();

    var requestModal = $("#request-box");
    var mentionModal = $("#mention-box");

    requestModal.on("shown.bs.modal", e => {
        $("#Name").focus();
    }); 
    requestModal.on("hide.bs.modal", e =>
    {
        location.hash = "#/";
    });

    mentionModal.on("shown.bs.modal", e =>
    {
        $("#Name").focus();
    });
    mentionModal.on("hide.bs.modal", e =>
    {
        location.hash = "#/";
    });

    router.registerRoute("#/request", (context) =>
    {
        requestModal.modal("show");
    });
    router.registerRoute("#/mention", (context) =>
    {
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
    menunavBlock.find("a").each((i, el) =>
    {
        var url = $(el).attr("href");
        let pathname = window.location.pathname;
        console.log(pathname);
        let isMain = pathname === "/" && url.indexOf("main") !== -1;
        var isActive = pathname.indexOf(url) === 0;
        if (isActive || isMain) {
            $(el).addClass("navigation__link-active");
        }
    });

});