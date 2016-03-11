
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PhotoBlogApp.Core;
using PhotoBlogApp.Core.Models.Albums;
using PhotoBlogApp.Core.Models.Photos;
using PhotoBlogApp.Helpers;
using PhotoBlogApp.Models;
using PhotoBlogApp.Models.ClientModels;
using PhotoBlogApp.Models.Photos;
using PhotoBlogApp.Models.Service;
using PhotoBlogApp.Models.Users;

namespace PhotoBlogApp.Controllers
{
    [System.Web.Mvc.Authorize(Users = "admin")]
    public class AlbumController : Controller
    {
        private static readonly string VirtualPathRootName = ConfigurationManager.AppSettings["VirtualPathRootName"];
        private static readonly string SystemAlbumName = ConfigurationManager.AppSettings["SystemAlbumName"];
        private readonly IAlbumRepository albumRepository;
        private readonly IUserRepository userRepository;
        private readonly IPhotoService photoService;
        private readonly IPhotoRepository photoRepository;
        private readonly IDataCache dataCache;

        public AlbumController(
            IAlbumRepository albumRepository,
            IUserRepository userRepository,
            IPhotoService photoService,
            IPhotoRepository photoRepository,
            IDataCache dataCache)
        {
            this.albumRepository = albumRepository;
            this.userRepository = userRepository;
            this.photoService = photoService;
            this.photoRepository = photoRepository;
            this.dataCache = dataCache;
        }

        [HttpPost]
        public ActionResult AddPhotoToSystemAlbum()
        {
            var user = userRepository.GetUserByLogin(User.Identity.Name);
            var result = new Result<ViewDataUploadFileResult>();
            if (!user.IsAdmin && !user.IsModerator)
            {
                return Content("Нет доступа");
            }
            var album = albumRepository
                .GetAlbums(true)
                .FirstOrDefault(a => a.AlbumName == SystemAlbumName);
            if (album != null)
            {
                var fileUploadM = new FileUploadM
                {
                    Login = User.Identity.Name,
                    AlbumId = album.AlbumId,
                    UploadedFile = Request.Files[0]
                };
                result.Data = photoService.SaveImage(fileUploadM, Server.MapPath("~/" + VirtualPathRootName));
                dataCache.ReloadData();
                return Content("ok");
            }
            return Content("error");
        }


        [HttpPost]
        public JsonResult AddPhotos(int albumid)
        {
            var user = userRepository.GetUserByLogin(User.Identity.Name);
            var result = new Result<ViewDataUploadFileResult>();
            if (!user.IsAdmin && !user.IsModerator)
            {
                result.Errors.Add("Нет доступа");
                return Json(result);
            }
            var fileUploadM = new FileUploadM
            {
                Login = User.Identity.Name,
                AlbumId = albumid,
                UploadedFile = Request.Files[0]
            };
            result.Data = photoService.SaveImage(fileUploadM, Server.MapPath("~/" + VirtualPathRootName));
            dataCache.ReloadData();
            return Json(result);
        }

        public ActionResult Index()
        {
            var albums = albumRepository.GetAlbums(withHidden: true).Include(x => x.User);
            return View(albums.ToList());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Album album)
        {
            var user = userRepository.GetUserByLogin(User.Identity.Name);
            album.UserId = user.UserId;
            if (ModelState.IsValid)
            {
                album.TranslitName = Transliteration.ResolveTextForUrl(album.AlbumName.ToLower());
                albumRepository.Add(album);
                dataCache.ReloadData();
                return RedirectToAction("Index");
            }
            var users = userRepository.GetUsers();
            ViewBag.UserId = new SelectList(users, "UserId", "Login", album.UserId);
            return View(album);
        }


        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var album = albumRepository.Find(id.Value);
            if (album == null)
            {
                return HttpNotFound();
            }
            return View(album);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Album album)
        {
            if (ModelState.IsValid)
            {
                album.TranslitName = Transliteration.ResolveTextForUrl(album.AlbumName.ToLower());
                albumRepository.Save(album);
                dataCache.ReloadData();
                return RedirectToAction("Index");
            }
            var users = userRepository.GetUsers();
            ViewBag.UserId = new SelectList(users, "UserId", "Login", album.UserId);
            return View(album);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var album = albumRepository.Find(id.Value);
            if (album == null)
            {
                return HttpNotFound();
            }
            return View(album);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            albumRepository.Delete(id);
            dataCache.ReloadData();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult DeletePhoto(int id, int albumId)
        {
            photoService.DeleteImage(User.Identity.Name, id, Server.MapPath("~/" + VirtualPathRootName));
            photoRepository.Delete(id);
            dataCache.ReloadData();
            return RedirectToAction("Edit", "Album", new { id = albumId });
        }
    }
}
