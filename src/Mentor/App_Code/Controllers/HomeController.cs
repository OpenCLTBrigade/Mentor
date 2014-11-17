using System;
using System.Data.Entity;
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
        public ActionResult Register(int? id, FormCollection form)
        {
            using (var db = new MentorDb())
            {
                var agency = id.HasValue ? db.Agencies.Include(x => x.Codes).Single(x => x.Id == id) : new Agency();

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

        //[Authorize]
        //public ActionResult Temp()
        //{
        //    var sb = new System.Text.StringBuilder();
        //    foreach (var code in new MentorDb().Codes)
        //    {
        //        sb.AppendFormat("new Code {{ Id={0}, Type=\"{1}\", Value=\"{2}\", Label=\"{3}\", Seq={4} }},\r\n",
        //            code.Id, code.Type, code.Value, code.Label, code.Seq);
        //    }
        //    return Content(sb.ToString(), "text/plain");
        //}

        //[Authorize]
        //public ActionResult Migrate()
        //{
        //    var configuration = new MentorDbMigrationConfiguration();
        //    var migrator = new DbMigrator(configuration);
        //    migrator.Update();
        //    return Content("Migrated", "text/plain");
        //}
    };
}
