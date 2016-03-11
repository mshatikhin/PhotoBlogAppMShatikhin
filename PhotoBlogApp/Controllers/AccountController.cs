using System.Net;
using System.Web.Mvc;
using System.Web.Security;
using PhotoBlogApp.Helpers;
using PhotoBlogApp.Models.Users;

namespace PhotoBlogApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly AuthorizationService authorizationService;
        private readonly IUserRepository userRepository;

        public AccountController(AuthorizationService authorizationService, IUserRepository userRepository)
        {
            this.authorizationService = authorizationService;
            this.userRepository = userRepository;
        }

        public ActionResult Register()
        {
            return View(new NetworkCredential());
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(NetworkCredential credentials)
        {
            var valid = true;
            if (string.IsNullOrWhiteSpace(credentials.UserName))
            {
                valid = false;
                ModelState.AddModelError("UserName", "Заполните имя пользователя");
            }
            if (string.IsNullOrWhiteSpace(credentials.Password))
            {
                valid = false;
                ModelState.AddModelError("Password", "Введите пароль");
            }
            if (valid)
            {
                var isExistUser = userRepository.ExistUser(credentials.UserName);
                if (isExistUser)
                {
                    ModelState.AddModelError("UserName", "Пользователь " + credentials.UserName + " уже зарегистрирован");
                    return View("Register");
                }
                var usr = userRepository.Save(credentials);
                FormsAuthentication.SetAuthCookie(usr.Login, true);
                return Redirect("/");
            }
            return View("Register");
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View(new NetworkCredential());
        }

        [HttpPost]
        public ActionResult Login(NetworkCredential credentials)
        {
            var validUser = authorizationService.ValidateUser(credentials.UserName, credentials.Password);
            if (validUser)
            {
                FormsAuthentication.SetAuthCookie(credentials.UserName, true);
                FormsAuthentication.RedirectFromLoginPage(credentials.UserName, true);
                return Redirect("~/");
            }
            return View(credentials);
        }

        [HttpGet]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}