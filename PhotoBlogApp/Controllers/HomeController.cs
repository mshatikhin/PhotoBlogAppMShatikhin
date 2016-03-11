using System;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using PhotoBlogApp.Models.ClientModels;
using PhotoBlogApp.Models.Providers;

namespace PhotoBlogApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBlogProvider blogProvider;
        private readonly IAlbumProvider albumProvider;

        private readonly string PictureOfTheDayAlbumName = ConfigurationManager.AppSettings["PictureOfTheDayAlbumName"];
        private readonly string SiteName = ConfigurationManager.AppSettings["SiteName"];

        public HomeController(
            IBlogProvider blogProvider,
            IAlbumProvider albumProvider)
        {
            this.blogProvider = blogProvider;
            this.albumProvider = albumProvider;
        }

        public ActionResult Feed()
        {
            return Content("ok");
        }

        public ActionResult Index(string message)
        {
            //var albumM = albumProvider.GetAlbum(1008);
            var mainVm = new MainViewM
            {
                PhotoM = new[]
                {
                    new PhotoM { FullUrl = "/Content/img/main/slideshow/1.jpg" },
                    new PhotoM { FullUrl = "/Content/img/main/slideshow/2.jpg" },
                    new PhotoM { FullUrl = "/Content/img/main/slideshow/3.jpg" },
                    new PhotoM { FullUrl = "/Content/img/main/slideshow/4.jpg" },
                    new PhotoM { FullUrl = "/Content/img/main/slideshow/5.jpg" },
                    new PhotoM { FullUrl = "/Content/img/main/slideshow/6.jpg" },
                    new PhotoM { FullUrl = "/Content/img/main/slideshow/7.jpg" },
                    new PhotoM { FullUrl = "/Content/img/main/slideshow/8.jpg" },
                    new PhotoM { FullUrl = "/Content/img/main/slideshow/9.jpg" },
                    new PhotoM { FullUrl = "/Content/img/main/slideshow/10.jpg" },
                    new PhotoM { FullUrl = "/Content/img/main/slideshow/11.jpg" },
                    new PhotoM { FullUrl = "/Content/img/main/slideshow/12.jpg" },
                    new PhotoM { FullUrl = "/Content/img/main/slideshow/13.jpg" },
                    new PhotoM { FullUrl = "/Content/img/main/slideshow/14.jpg" },
                    new PhotoM { FullUrl = "/Content/img/main/slideshow/15.jpg" },
                    new PhotoM { FullUrl = "/Content/img/main/slideshow/16.jpg" },
                    new PhotoM { FullUrl = "/Content/img/main/slideshow/17.jpg" },
                    new PhotoM { FullUrl = "/Content/img/main/slideshow/18.jpg" },
                    new PhotoM { FullUrl = "/Content/img/main/slideshow/19.jpg" },
                    new PhotoM { FullUrl = "/Content/img/main/slideshow/20.jpg" }
                },
               // PicturesOfDay = albumM
            };

            const string seotitle = "Главная - Михаил Шатихин | Фотограф Екатеринбург";
            const string description = "Добро пожаловать в блог Михаила Шатихина. Здесь можно узнать многое о моих увлечениях, посмотреть фотографии, записаться ко мне на фотосессию, почитать различные статьи о моих путешествиях по Уралу и не только.";
            const string type = "article";
            const string url = "url";
            const string imageUrl = "";
            SetViewBagValues(seotitle, description, url, imageUrl, type);
            ViewBag.Message = message;
            return View(mainVm);
        }

        public ActionResult Portfolio()
        {
            var albums = albumProvider.GetAlbums();
            var model = new AlbumsM
            {
                AlbumMs = albums
            };

            const string seotitle = "Портфолио - Михаил Шатихин";
            const string description = "Моё скромное портфолио. Если вам нравятся мои работы, приглашаю вас к сотрудничеству со мной. Если же вам хочется купить работу пишите мне на почту mshatikhin@gmail.com";
            const string type = "article";
            const string url = "url";
            const string imageUrl = "";
            SetViewBagValues(seotitle, description, url, imageUrl, type);

            return View(model);
        }

        public ActionResult Images(string title)
        {
            var album = albumProvider.GetAlbums().SingleOrDefault(a => a.TitleUrl == title);
            if (album != null)
            {
                var seotitle = string.Format("Альбом {0} | Фотограф Екатеринбург", album.Name);
                var description = string.Format("Работы из портфолио Михаила Шатихина. Альбом {0}", album.Name);
                const string type = "article";
                const string url = "url";
                const string imageUrl = "";
                SetViewBagValues(seotitle, description, url, imageUrl, type);

                return View(album);
            }
            return View();
        }

        [HttpGet]
        public ActionResult Image(string title, int photoid)
        {
            var album = albumProvider
                .GetAlbums()
                .SingleOrDefault(a => a.TitleUrl == title);

            if (album == null)
                return View();

            var photo = album.Photos.SingleOrDefault(p => p.PhotoId == photoid);

            if (photo != null)
            {
                var seotitle = string.Format("{0} | Портфолио", photo.Title);
                const string description = "";
                const string type = "article";
                const string url = "url";
                const string imageUrl = "";
                SetViewBagValues(seotitle, description, url, imageUrl, type);
                ViewBag.ActiveIndex = Array.IndexOf(album.Photos, photo);
            }
            return View(album);
        }

        [HttpGet]
        public ActionResult About()
        {
            const string seotitle = "Обо мне - Михаил Шатихин";
            const string description = "Чуточку информации обо мне.";
            const string type = "article";
            const string url = "url";
            const string imageUrl = "";
            SetViewBagValues(seotitle, description, url, imageUrl, type);

            return View();
        }

        [HttpGet]
        public ActionResult Contact()
        {
            const string seotitle = "Контакты - Михаил Шатихин";
            const string description = "Готов к общению с вами. Связаться со мной очень просто.";
            const string type = "article";
            const string url = "url";
            const string imageUrl = "";
            SetViewBagValues(seotitle, description, url, imageUrl, type);

            return View();
        }

        public ActionResult Blog()
        {
            var blogs = blogProvider.GetPosts();

            const string seotitle = "Блог Михаила Шатихина";
            const string description = "Добро пожаловать в мой бложек. Иногда я пишу интересные на мой вгляд мысли, и небольшие рассказы о моих путешествиях.";
            const string type = "article";
            const string url = "url";
            const string imageUrl = "";
            SetViewBagValues(seotitle, description, url, imageUrl, type);

            return View(blogs);
        }

        public ActionResult Post(string title)
        {
            var post = blogProvider.GetPost(title);
            post.PagingM = GetPaging(blogProvider.GetPosts(), post);

            var seotitle = string.Format("{0} | Михаил Шатихин", post.Title);
            const string description = "";
            const string type = "article";
            const string url = "url";
            const string imageUrl = "";
            SetViewBagValues(seotitle, description, url, imageUrl, type);

            return View(post);
        }

        private PagingM GetPaging(PostM[] posts, PostM currentPost)
        {
            var paging = new PagingM();
            var indexOfCurrentPost = Array.IndexOf(posts, currentPost);
            var previdx = indexOfCurrentPost - 1;
            var nextidx = indexOfCurrentPost + 1;

            if (previdx < 0) { paging.HasPrevPage = false; } else { paging.PrevPage = posts[previdx].TitleUrl; }
            if (nextidx >= posts.Length) { paging.HasNextPage = false; } else { paging.NextPage = posts[nextidx].TitleUrl; }

            return paging;
        }

        private void SetViewBagValues(string title, string description, string url, string imageUrl, string type)
        {
            ViewBag.SiteName = SiteName;
            ViewBag.Title = title;
            ViewBag.Description = description;
            ViewBag.Type = type;
            ViewBag.ShareUrl = url;
            ViewBag.ImageUrl = imageUrl;

            //ViewBag.Url = url;
        }
    }
}