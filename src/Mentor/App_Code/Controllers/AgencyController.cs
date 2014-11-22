using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace Mentor
{
    public class AgencyController : Controller
    {
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult ListAgencies()
        {
            using (var db = new MentorDb())
            {
                var agencies = db.Agencies.ToList();
                return View("ListAgencies", agencies);
            }
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult EditAgency(int? id)
        {
            using (var db = new MentorDb())
            {
                var agency = id.HasValue ? db.Agencies.Include(x => x.Codes).Single(x => x.Id == id) : new Agency();

                if (Request.HttpMethod == "POST")
                {
                    TryUpdateModel(agency);
                    if (agency.IsNew) db.Add(agency);
                    db.SaveChanges();
                    return RedirectToAction("ListAgencies");

                }

                return View(agency);
            }
        }
    };
}
