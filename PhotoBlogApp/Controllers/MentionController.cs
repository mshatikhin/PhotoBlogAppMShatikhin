using System.Linq;
using System.Net;
using System.Web.Mvc;
using PhotoBlogApp.Core;
using PhotoBlogApp.Models.Service;

namespace PhotoBlogApp.Controllers
{
    public class MentionController : Controller
    {
        private readonly IMentionService mentionService;
        private DbModelContainer db = new DbModelContainer();

        public MentionController(IMentionService mentionService)
        {
            this.mentionService = mentionService;
        }

        public ActionResult SendMention(string name, string mention)
        {
            mentionService.Add(name, mention);
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Index()
        {
            return View(db.Mentions.ToList());
        }

        [Authorize(Users = "admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mention mention = db.Mentions.Find(id);
            if (mention == null)
            {
                return HttpNotFound();
            }
            return View(mention);
        }

        [Authorize(Users = "admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var mention = db.Mentions.Find(id);
            db.Mentions.Remove(mention);
            db.SaveChanges();
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
