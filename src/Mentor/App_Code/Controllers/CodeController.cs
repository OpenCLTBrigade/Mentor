using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Mentor
{
    public class CodeController : Controller
    {
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult Index(string type, List<Code> codes)
        {
            using (var db = new MentorDb())
            {
                if (Request.HttpMethod == "POST")
                {
                    foreach (var code in codes)
                    {
                        if (string.IsNullOrWhiteSpace(code.Value))
                            continue;

                        db.Save(code, code.Id == 0);
                    }
                    db.SaveChanges();
                    return RedirectToAction("Index", "Code", new {type});
                }

                var types = db.Codes
                              .OrderBy(x => x.Type)
                              .Select(x => x.Type)
                              .Distinct()
                              .ToList();

                var model = db.Codes
                              .Where(x => x.Type == type)
                              .OrderBy(x => x.Seq)
                              .ThenBy(x => x.Value)
                              .ToList();

                ViewBag.Types = new SelectList(types, type);

                return View("ManageCodes", model);
            }
        }
    };
}
