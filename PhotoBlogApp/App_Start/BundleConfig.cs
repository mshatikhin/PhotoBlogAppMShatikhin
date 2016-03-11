using System.Web.Optimization;

namespace PhotoBlogApp
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            //BundleTable.EnableOptimizations = true;

            bundles.Add(new ScriptBundle("~/bundles/filupload")
                .Include("~/Scripts/FileUpload/jquery.ui.widget.js")
                .Include("~/Scripts/FileUpload/jquery.iframe-transport.js")
                .Include("~/Scripts/FileUpload/jquery.fileupload.js")
                );

            bundles.Add(new ScriptBundle("~/bundles/main")
                .Include("~/Scripts/jquery-{version}.js")
                .Include("~/Scripts/bootstrap.js")
                .Include("~/Scripts/share.js")
                .Include("~/Scripts/fotorama.js")
                .Include("~/Scripts/sammy.js")
                .Include("~/Scripts/client/router.js")
                .Include("~/Scripts/client/main.js")

                //.Include("~/Scripts/knockout-{version}.js")
                //.Include("~/Scripts/knockout.custom.js")
                //.Include("~/Scripts/client/photoblog.api.js")
                //.Include("~/Scripts/client/models.js")
                //.Include("~/Scripts/client/photoProjectsVm.js")
                //.Include("~/Scripts/client/landing.js")


                //.Include("~/Scripts/client/custom.js")
                //.Include("~/Scripts/timeline-min.js")
                //.Include("~/Scripts/timeline-ru.js")
                //.Include("~/Scripts/storyjs-embed.js")

                );

            bundles.Add(new ScriptBundle("~/bundles/portfolio")
                .Include("~/Scripts/jquery-{version}.js")
                .Include("~/Scripts/bootstrap.js")
                .Include("~/Scripts/share.js")
                .Include("~/Scripts/fotorama.js")
                .Include("~/Scripts/sammy.js")
                .Include("~/Scripts/client/router.js")
                .Include("~/Scripts/client/main.js")
                .Include("~/Scripts/imagesloaded.pkgd.min.js")
                .Include("~/Scripts/packery.pkgd.min.js")
                );

            //////////////////////////////////////////////////

            bundles.Add(new StyleBundle("~/bundles/Content/css")
                .Include("~/Content/css/normalize.css")
                .Include("~/Content/css/jquery.fileupload-ui.css")
                .Include("~/Content/css/bootstrap.css")
                .Include("~/Content/css/common.min.css")
                .Include("~/Content/css/social.css")
                .Include("~/Content/css/fotorama.css")
                .Include("~/Content/css/app.min.css")

                //.Include("~/Content/css/timeline.css")
                );

            bundles.Add(new StyleBundle("~/bundles/Content/reactcss")
                .Include("~/Content/css/normalize.css")
                .Include("~/Content/css/common.min.css")
                .Include("~/Content/css/msced.min.css")
                );
        }
    }
}