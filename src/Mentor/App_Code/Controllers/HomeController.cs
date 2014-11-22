using System.Web.Mvc;
using Common;

namespace Mentor
{
    public class HomeController : Controller
    {
        private readonly AgencyService _agencies;
        private readonly UserService _users;

        public HomeController(AgencyService agencyService, UserService userService)
        {
            _agencies = agencyService;
            _users = userService;
        }

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
            if (Request.IsPost())
            {
                _users.Login(email, password);
                return RedirectToAction("Index");
            }
            return View();
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult Logout()
        {
            _users.Logout();
            return RedirectToAction("Index");
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult Register(int? id)
        {
            var agency = _agencies.Find(id) ?? _agencies.Create();
            if (Request.IsPost())
            {
                TryUpdateModel(agency);
                _agencies.Save(agency);
                return Content("Registration successful " + agency.Id);

            }
            return View(agency);
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
