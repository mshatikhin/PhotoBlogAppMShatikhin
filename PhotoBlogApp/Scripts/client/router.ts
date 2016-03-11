/// <reference path="../typings/sammyjs/sammyjs.d.ts" />

module App
{
    export class RouterVm
    {
        start: () => void;
        app = Sammy();
        scrollTo: (id: string) => void;
        links = ["promo", "about", "blog", "photos"];
        registerRoute: (route: string, callback: (params) => void) => void;

        constructor()
        {
            this.scrollTo = (id) =>
            {
                $("html, body").stop().animate({ scrollTop: $("#" + id).offset().top }, 400);
                $(".menu-nav__link-active").removeClass("menu-nav__link-active");
                $(".menu-nav").find("a[href='#/" + id + "']").addClass("menu-nav__link-active");

                return false;
            }

            this.links.forEach((link) =>
            {
                this.app.get("#/" + link,() =>
                {
                    this.scrollTo(link);
                    $(".menu-nav").find("a[href='#/" + link + "']").click(() =>
                    {
                        this.scrollTo(link);
                    });
                });
            });


            this.registerRoute = (route, callback) =>
            {
                this.app.get(route, context =>
                {
                    callback(context);
                });
            };

            this.start = () =>
            {
                this.app.run();
            }
        }
    }

} 