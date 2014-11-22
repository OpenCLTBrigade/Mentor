using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Common;

namespace Mentor
{
    public class CodeController : Controller
    {
        private readonly CodeService _codes;

        public CodeController(CodeService codeService)
        {
            _codes = codeService;
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult ManageCodes(string type, List<Code> codes)
        {
            if (string.IsNullOrWhiteSpace(type))
            {
                var types = _codes.GetTypes();
                ViewBag.Types = new SelectList(types, type);
                return View("ManageCodes");
            }
            else if (Request.IsPost())
            {
                codes.ForEach(_codes.Save);
                return RedirectToAction("ManageCodes", new { type });
            }
            else
            {
                var model = _codes.Query(type).ToList();
                return View("ManageCodes", model);
            }
        }
    };
}
