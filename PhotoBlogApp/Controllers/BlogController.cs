using System;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using PhotoBlogApp.Core;
using PhotoBlogApp.Core.Models.Blogs;
using PhotoBlogApp.Helpers;
using PhotoBlogApp.Models;
using PhotoBlogApp.Models.Service;
using PhotoBlogApp.Models.Users;

namespace PhotoBlogApp.Controllers
{
    public class BlogController : Controller
    {
        private readonly IUserRepository userRepository;
        private readonly IBlogRepository blogRepository;
        private readonly IBlogService blogService;
        private readonly IDataCache dataCache;
        private static readonly string SiteUrl = ConfigurationManager.AppSettings["SiteUrl"];

        public BlogController(
            IUserRepository userRepository,
            IBlogRepository blogRepository,
            IBlogService blogService,
            IDataCache dataCache)
        {
            this.userRepository = userRepository;
            this.blogRepository = blogRepository;
            this.blogService = blogService;
            this.dataCache = dataCache;
        }

        [System.Web.Mvc.Authorize]
        public ActionResult Index()
        {
            ViewBag.Title = "Блог фотографа \"Михаила Шатихина\"";
            ViewBag.Type = "website";
            ViewBag.Url = SiteUrl + "/blog";
            var user = userRepository.GetUserByLogin(User.Identity.Name);
            var blogs = blogRepository
                .GetAll(true)
                .Include(b => b.User)
                .Where(b => b.User.IsAdmin || b.User.UserId == user.UserId)
                .OrderByDescending(x => x.BlogId)
                .ToList();

            return View(blogs);
        }

        public ActionResult Post(int id)
        {
            var post = blogRepository.Find(id);
            if (post != null)
            {
                ViewBag.Title = post.HeaderName;
                ViewBag.Type = "article";
                ViewBag.ImageUrl = post.ImageUrl;
                ViewBag.Description = string.IsNullOrEmpty(post.Description) ? post.ContentText.Substring(0, 300) : post.Description;
                ViewBag.Url = SiteUrl + "/post/" + id;
                var postM = blogService.BuildPostM(post);
                return View(postM);
            }
            return View();
        }

        [System.Web.Mvc.Authorize(Users = "admin")]
        public ActionResult Create()
        {
            ViewBag.UserId = new SelectList(userRepository.GetUsers(), "UserId", "Login");
            return View();
        }

        [System.Web.Mvc.Authorize(Users = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(Blog blog)
        {
            var datePublish = DateTime.Now;
            blog.DatePublish = datePublish;
            blog.DateEnd = datePublish;
            blog.UserId = userRepository.GetUserByLogin(User.Identity.Name).UserId;
            if (ModelState.IsValid)
            {
                blog.TranslitName = Transliteration.ResolveTextForUrl(blog.HeaderName.ToLower());
                blogRepository.Add(blog);
                dataCache.ReloadData();
                return RedirectToAction("Index");
            }

            ViewBag.UserId = new SelectList(userRepository.GetUsers(), "UserId", "Login", blog.UserId);
            return View(blog);
        }

        [System.Web.Mvc.Authorize(Users = "admin")]
        [ValidateInput(false)]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var blog = blogRepository.Find(id.Value);
            if (blog == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(userRepository.GetUsers(), "UserId", "Login", blog.UserId);
            return View(blog);
        }

        [System.Web.Mvc.Authorize(Users = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(Blog blog)
        {
            if (ModelState.IsValid)
            {
                blog.TranslitName = Transliteration.ResolveTextForUrl(blog.HeaderName.ToLower());
                blogRepository.Save(blog);
                dataCache.ReloadData();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(userRepository.GetUsers(), "UserId", "Login", blog.UserId);
            return View(blog);
        }

        [System.Web.Mvc.Authorize(Users = "admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var blog = blogRepository.Find(id.Value);
            if (blog == null)
            {
                return HttpNotFound();
            }
            return View(blog);
        }

        [System.Web.Mvc.Authorize(Users = "admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            blogRepository.Delete(id);
            dataCache.ReloadData();
            return RedirectToAction("Index");
        }
    }
}
