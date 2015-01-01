using System;
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

        public ActionResult SelectProfile()
        {
            if (!Request.IsAuthenticated)
            {
                throw new ApplicationException("You must be logged in to edit an agency profile");
            }

            var agencies = _agencies.FindByUser(User.Identity.Name);
            if (agencies.Count == 1)
            {
                var agency = agencies.First();
                return RedirectToAction("Register", "Home", new {agency.Id});
            }

            return View(agencies);
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult Register(int? id)
        {
            var agency = _agencies.Find(id) ?? _agencies.Create();
            ViewBag.Codes = _codes.Query().ToList();

            if (id > 0 && !Request.IsAuthenticated)
            {
                throw new ApplicationException("User cannot access agency");
            }

            if (Request.IsPost())
            {
                try
                {
                    TryUpdateModel(agency);
                    _agencies.Save(agency, Request.Form["Username"], Request.Form["Password"]);
                    return Content("Registration successful " + agency.Id);
                }
                catch (Exception ex)
                {
                    ViewBag.Error = ex;
                    return View(agency);
                }
            }
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

        public ActionResult Error()
        {
            return View("Error");
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
