using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using PhotoBlogApp.Core;
using PhotoBlogApp.Models;

namespace PhotoBlogApp.Controllers
{
    [Authorize(Users = "admin")]
    public class PhotoProjectsController : Controller
    {
        private readonly IDataCache dataCache;
        private DbModelContainer db = new DbModelContainer();

        public PhotoProjectsController(IDataCache dataCache)
        {
            this.dataCache = dataCache;
        }

        // GET: PhotoProjects
        public ActionResult Index()
        {
            var photoProjects = db.PhotoProjects.Include(p => p.Request);
            return View(photoProjects.ToList());
        }

        // GET: PhotoProjects/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhotoProject photoProject = db.PhotoProjects.Find(id);
            if (photoProject == null)
            {
                return HttpNotFound();
            }
            return View(photoProject);
        }

        // GET: PhotoProjects/Create
        public ActionResult Create()
        {
            ViewBag.RequestId = new SelectList(db.Requests, "RequestId", "LastName");
            return View();
        }

        // POST: PhotoProjects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(PhotoProject photoProject)
        {
            if (ModelState.IsValid)
            {
                db.PhotoProjects.Add(photoProject);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            dataCache.ReloadData();
            ViewBag.RequestId = new SelectList(db.Requests, "RequestId", "LastName", photoProject.RequestId);
            return View(photoProject);
        }

        // GET: PhotoProjects/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhotoProject photoProject = db.PhotoProjects.Find(id);
            if (photoProject == null)
            {
                return HttpNotFound();
            }
            ViewBag.RequestId = new SelectList(db.Requests, "RequestId", "LastName", photoProject.RequestId);
            return View(photoProject);
        }

        // POST: PhotoProjects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(PhotoProject photoProject)
        {
            if (ModelState.IsValid)
            {
                db.Entry(photoProject).State = EntityState.Modified;
                db.SaveChanges();
                dataCache.ReloadData();
                return RedirectToAction("Index");
            }
            ViewBag.RequestId = new SelectList(db.Requests, "RequestId", "LastName", photoProject.RequestId);
            return View(photoProject);
        }

        // GET: PhotoProjects/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhotoProject photoProject = db.PhotoProjects.Find(id);
            if (photoProject == null)
            {
                return HttpNotFound();
            }
            return View(photoProject);
        }

        // POST: PhotoProjects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult DeleteConfirmed(int id)
        {
            PhotoProject photoProject = db.PhotoProjects.Find(id);
            db.PhotoProjects.Remove(photoProject);
            db.SaveChanges();
            dataCache.ReloadData();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
