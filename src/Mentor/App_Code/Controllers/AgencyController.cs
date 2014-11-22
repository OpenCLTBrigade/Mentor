﻿using System.Linq;
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

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Delete(int? id)
        {
            _agencies.Delete(id);
            return RedirectToAction("ListAgencies");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ImportAgencies()
        {
            var file = Request.Files["InputFile"];
            if (file != null && file.InputStream != null && file.ContentLength > 0)
            {
                _agencies.Import(file.InputStream);
            }
            return RedirectToAction("ListAgencies");
        }
    };
}
