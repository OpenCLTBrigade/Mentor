using System.Linq;
using System.Web.Mvc;
using Common;

namespace Mentor
{
    public class AgencyController : Controller
    {
        private readonly AgencyService _agencies;

        public AgencyController(AgencyService agencyService)
        {
            _agencies = agencyService;
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult ListAgencies()
        {
            var agencies = _agencies.Query().ToList();
            return View("ListAgencies", agencies);
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult EditAgency(int? id)
        {
            var agency = _agencies.Find(id) ?? _agencies.Create();
            if (Request.IsPost())
            {
                TryUpdateModel(agency);
                _agencies.Save(agency);
                return RedirectToAction("ListAgencies");
            }
            return View(agency);
        }
    };
}
