using System.Linq;
using System.Web.Mvc;
using Common;

namespace Mentor
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly AgencyService _agencies;
        private readonly CodeService _codes;
        private readonly UserService _users;

        public HomeController(AgencyService agencyService, CodeService codeService, UserService userService)
        {
            _agencies = agencyService;
            _codes = codeService;
            _users = userService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Agency(int id)
        {
            var agency = _agencies.Find(id);
            return View(agency);
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
            ViewBag.Codes = _codes.Query().ToList();
            return View(agency);
        }

        public ActionResult Search(AgencySearch agencySearch)
        {
            if (agencySearch == null)
                agencySearch = new AgencySearch();

            agencySearch.Filter(_agencies.Query());

            ViewBag.Codes = _codes.Query().ToList();
            return View(agencySearch);
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
