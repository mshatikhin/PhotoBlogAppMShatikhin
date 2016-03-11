using System.Web.Mvc;
using System.Web.Routing;

namespace PhotoBlogApp
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");


            routes.MapRoute(
               name: "Main",
               url: "main",
               defaults: new { controller = "Home", action = "Index" }
               );

            routes.MapRoute(
              name: "Image",
              url: "portfolio/album/{title}/photo/{photoid}",
              defaults: new { controller = "Home", action = "Image" }
              );

            routes.MapRoute(
              name: "Albump",
              url: "portfolio/album/{title}",
              defaults: new { controller = "Home", action = "Images" }
              );

            routes.MapRoute(
             name: "Portfolio",
             url: "portfolio",
             defaults: new { controller = "Home", action = "Portfolio" }
             );

            routes.MapRoute(
              name: "Post",
              url: "blog/post/{title}",
              defaults: new { controller = "Home", action = "Post" }
              );


            routes.MapRoute(
              name: "Blog",
              url: "blog",
              defaults: new { controller = "Home", action = "Blog" }
              );

            routes.MapRoute(
             name: "About",
             url: "about",
             defaults: new { controller = "Home", action = "About" }
             );

            routes.MapRoute(
             name: "Contact",
             url: "contact",
             defaults: new { controller = "Home", action = "Contact" }
             );

            routes.MapRoute(
                name: "adminBlog",
                url: "admin/Blog",
                defaults: new { controller = "Blog", action = "Index", id = UrlParameter.Optional }
                );

            routes.MapRoute(
                name: "Photo",
                url: "Photographs",
                defaults: new { controller = "Photo", action = "Index", id = UrlParameter.Optional }
                );

            routes.MapRoute(
                name: "Albums",
                url: "photoalbums",
                defaults: new { controller = "Album", action = "Index", id = UrlParameter.Optional }
                );

            routes.MapRoute(
                name: "Requests",
                url: "Requests",
                defaults: new { controller = "Request", action = "Index", id = UrlParameter.Optional }
                );

            routes.MapRoute(
                name: "Users",
                url: "Users",
                defaults: new { controller = "User", action = "Index", id = UrlParameter.Optional }
                );

            routes.MapRoute(
                name: "login",
                url: "login",
                defaults: new { controller = "Account", action = "Login", id = UrlParameter.Optional }
                );

            routes.MapRoute(
                name: "logOff",
                url: "logOff",
                defaults: new { controller = "Account", action = "LogOff", id = UrlParameter.Optional }
                );

            routes.MapRoute(
                name: "register",
                url: "register",
                defaults: new { controller = "Account", action = "Register", id = UrlParameter.Optional }
                );

            routes.MapRoute(
                name: "getAlbums",
                url: "getAlbums",
                defaults: new { controller = "Home", action = "GetAlbums", id = UrlParameter.Optional }
                );
            
            routes.MapRoute(
                name: "getPosts",
                url: "getPosts",
                defaults: new { controller = "Home", action = "GetPosts", id = UrlParameter.Optional }
                );

            routes.MapRoute(
                name: "getPhotos",
                url: "getPhotos",
                defaults: new { controller = "Root", action = "GetPhotos", id = UrlParameter.Optional }
                );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
