using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using PhotoBlogApp.Core;
using PhotoBlogApp.Core.Models.Albums;
using PhotoBlogApp.Core.Models.Photos;
using PhotoBlogApp.Models.Service;

namespace PhotoBlogApp.Controllers
{
    [Authorize(Users = "admin")]
    public class PhotoController : Controller
    {

        private readonly IPhotoService photoService;
        private readonly IPhotoRepository photoRepository;
        private readonly IAlbumRepository albumRepository;
        private static readonly string VirtualPathRootName = ConfigurationManager.AppSettings["VirtualPathRootName"];

        public PhotoController(IPhotoRepository photoRepository, IPhotoService photoService, IAlbumRepository albumRepository)
        {
            this.photoRepository = photoRepository;
            this.photoService = photoService;
            this.albumRepository = albumRepository;
        }

        public ActionResult Index()
        {
            var photos = photoRepository.GetPhotos().Include(p => p.Album);
            return View(photos.ToList());
        }

        public ActionResult Create()
        {
            ViewBag.AlbumId = new SelectList(albumRepository.GetAlbums(true), "AlbumId", "AlbumName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PhotoId,PhotoName,Path,Extention,AlbumId,Description,Date")] Photo photo)
        {
            if (ModelState.IsValid)
            {
                photoRepository.Add(photo);
                return RedirectToAction("Index");
            }

            ViewBag.AlbumId = new SelectList(albumRepository.GetAlbums(true), "AlbumId", "AlbumName", photo.AlbumId);
            return View(photo);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var photo = photoRepository.Find(id.Value);
            if (photo == null)
            {
                return HttpNotFound();
            }
            ViewBag.AlbumId = new SelectList(albumRepository.GetAlbums(true), "AlbumId", "AlbumName", photo.AlbumId);
            return View(photo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PhotoId,PhotoName,Path,Extention,AlbumId,Description,Date")] Photo photo)
        {
            if (ModelState.IsValid)
            {
                photoRepository.Save(photo);
                return RedirectToAction("Index");
            }
            ViewBag.AlbumId = new SelectList(albumRepository.GetAlbums(true), "AlbumId", "AlbumName", photo.AlbumId);
            return View(photo);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            photoService.DeleteImage(User.Identity.Name, id, Server.MapPath("~/" + VirtualPathRootName));
            photoRepository.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
