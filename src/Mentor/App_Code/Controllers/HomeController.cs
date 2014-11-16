using System.Web.Mvc;

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

        public ActionResult Login()
        {
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
