using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PhotoBlogApp.Core.Models.Albums;
using PhotoBlogApp.Models.Service;
using PhotoBlogApp.Models.Users;

namespace PhotoBlogApp.Controllers
{
    public class FileController : Controller
    {
        private static readonly string SystemAlbumName = ConfigurationManager.AppSettings["SystemAlbumName"];

        private readonly IAlbumRepository albumRepository;
        private readonly IUserRepository userRepository;
        private readonly IAlbumService albumService;
        
        public FileController(IAlbumRepository albumRepository, IAlbumService albumService, IUserRepository userRepository)
        {
            this.albumRepository = albumRepository;
            this.albumService = albumService;
            this.userRepository = userRepository;
        }

        public ActionResult Browse()
        {
            var user = userRepository.GetUserByLogin(User.Identity.Name);
            if (!user.IsAdmin && !user.IsModerator)
            {
                return Content("error");
            }

            var album = albumRepository
                .GetAlbums(withHidden: true)
                .FirstOrDefault(a => a.AlbumName == SystemAlbumName);
            if (album != null)
            {
                var model = albumService.BuildAlbumM(album);
                return View(model);
            }
            return View();
        }
    }
}