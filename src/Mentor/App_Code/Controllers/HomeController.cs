using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace Mentor
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Faq()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult Login(string email, string password)
        {
            if (Request.HttpMethod == "POST")
            {
                using (var db = new MentorDb())
                {
                    var user = db.Users.Single(x => x.Email == email);
                    if (!user.Active || user.Password != password)
                        throw new ApplicationException("Invalid login");

                    FormsAuthentication.SetAuthCookie(user.Email, true);
                    return RedirectToAction("Index");
                }
            }
            return View();
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult Register(int? id)
        {
            using (var db = new MentorDb())
            {
                var agency = id.HasValue ? db.Agencies.Find(id.Value) : new Agency();

                if (Request.HttpMethod == "POST")
                {
                    TryUpdateModel(agency);
                    if (agency.Id == 0) db.Add(agency);
                    db.SaveChanges();
                    return Content("Registration successful " + agency.Id);

                }
                return View(agency);
            }
        }

        public ActionResult Search()
        {
            return View();
        }
    };
}
