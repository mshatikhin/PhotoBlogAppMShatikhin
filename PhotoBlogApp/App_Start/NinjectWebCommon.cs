using System;
using System.Web;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using Ninject;
using Ninject.Web.Common;
using PhotoBlogApp;
using PhotoBlogApp.Core.Models.Albums;
using PhotoBlogApp.Core.Models.Blogs;
using PhotoBlogApp.Core.Models.Mentions;
using PhotoBlogApp.Core.Models.Photos;
using PhotoBlogApp.Core.Models.Requests;
using PhotoBlogApp.Models;
using PhotoBlogApp.Models.Builders;
using PhotoBlogApp.Models.Providers;
using PhotoBlogApp.Models.Service;
using PhotoBlogApp.Models.Users;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(NinjectWebCommon), "Stop")]

namespace PhotoBlogApp
{
    public static class NinjectWebCommon
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }

        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }

        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<IMainVmBuilder>().To<MainVmBuilder>();
            kernel.Bind<ITimeLineBuilder>().To<TimeLineBuilder>();

            kernel.Bind<IRequestRepository>().To<RequestRepository>();
            kernel.Bind<IUserRepository>().To<UserRepository>();
            kernel.Bind<IAlbumRepository>().To<AlbumRepository>();
            kernel.Bind<IBlogRepository>().To<BlogRepository>();
            kernel.Bind<IPhotoRepository>().To<PhotoRepository>();
            kernel.Bind<IMentionRepository>().To<MentionRepository>();
            kernel.Bind<IPhotoProjectRepository>().To<PhotoProjectRepository>();

            kernel.Bind<IAlbumService>().To<AlbumService>();
            kernel.Bind<IBlogService>().To<BlogService>();
            kernel.Bind<IPhotoService>().To<PhotoService>().InSingletonScope();
            kernel.Bind<IRequestService>().To<RequestService>();
            kernel.Bind<IMentionService>().To<MentionService>();
            kernel.Bind<IPhotoProjectService>().To<PhotoProjectService>();

            kernel.Bind<IAlbumProvider>().To<AlbumProvider>().InSingletonScope();
            kernel.Bind<IBlogProvider>().To<BlogProvider>().InSingletonScope();
            kernel.Bind<IPhotoProjectProvider>().To<PhotoProjectProvider>().InSingletonScope();

            kernel.Bind<IDataCache>().To<DataCache>().InSingletonScope();
        }
    }
}
