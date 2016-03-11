using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;
using PhotoBlogApp.Core;
using PhotoBlogApp.Models.ClientModels;
using PhotoBlogApp.Models.Service;

namespace PhotoBlogApp.Controllers
{

    public class RequestController : Controller
    {
        private readonly IRequestService requestService;
        private readonly IPhotoProjectService photoProjectService;
        private readonly string SiteName = ConfigurationManager.AppSettings["SiteName"];

        [AllowAnonymous]
        [HttpPost]
        public ActionResult SendRequest(RequestM requestM)
        {
            var comments = requestM.Tfp ? "TFP (бесплатно) -" + requestM.Comments : requestM.Comments;
            if (requestM.PhotoProjects != null)
            {
                var projects = photoProjectService.GetPhotoProjects().Where(p => requestM.PhotoProjects.Contains(p.PhotoProjectId)).Select((v) => v.Name).ToArray();
                comments += "<br/> Фотопроекты: " + string.Join(",", projects);
            }

            var request = new RequestM
            {
                Name = requestM.Name,
                Comments = comments,
                Coordinates = requestM.Coordinates,
                PhotoProjects = requestM.PhotoProjects ?? new[] { 0 },
                Tfp = requestM.Tfp
            };
            var result = requestService.Validate(request);
            string m;
            if (result.IsSuccess)
            {
                requestService.Save(request);
                SendMail(request);
                m = "Заявка успешно отправлена";
            }
            else
            {
                m = "Заявка не была отправлена";
            }
            return RedirectToAction("Index", "Home", new { message = m });
        }

        private void SendMail(RequestM request)
        {
            using (var smtpClient = new SmtpClient())
            {
                var address = "mshatikhin@gmail.com";
                var password = "@PkPv29masoc";

                smtpClient.Host = "smtp.gmail.com";
                smtpClient.Port = 587;
                smtpClient.Credentials = new NetworkCredential(address, password);
                smtpClient.EnableSsl = true;
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.Timeout = 20000;

                var body = string.Format("Заявка от: {0}. <br/> Координаты: {1}. <br/> Комментарии: {2}.", request.Name, request.Coordinates, request.Comments);

                var mail = new MailMessage
                {
                    From = new MailAddress(address, SiteName),
                    Body = body,
                    IsBodyHtml = true,
                    Subject = "Заявка с сайта " + SiteName,
                    Priority = MailPriority.High
                };

                mail.To.Add(new MailAddress(address));

                smtpClient.Send(mail);
            }
        }
        
        private DbModelContainer db = new DbModelContainer();

        public RequestController(IRequestService requestService, IPhotoProjectService photoProjectService)
        {
            this.requestService = requestService;
            this.photoProjectService = photoProjectService;
        }

        [Authorize(Users = "admin, smithtanya")]
        public ActionResult Index()
        {
            return View(db.Requests.ToList());
        }

        [Authorize(Users = "admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Request request = db.Requests.Find(id);
            if (request == null)
            {
                return HttpNotFound();
            }
            return View(request);
        }

        [Authorize(Users = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RequestId,Date,LastName,FirstName,Comment,ContactInfo")] Request request)
        {
            if (ModelState.IsValid)
            {
                db.Entry(request).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(request);
        }

        [Authorize(Users = "admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Request request = db.Requests.Find(id);
            if (request == null)
            {
                return HttpNotFound();
            }
            return View(request);
        }

        [Authorize(Users = "admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Request request = db.Requests.Find(id);
            db.Requests.Remove(request);
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
