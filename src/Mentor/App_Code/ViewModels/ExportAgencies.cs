using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Mime;
using System.Reflection;
using System.Web.Mvc;
using OfficeOpenXml;

namespace Mentor
{
    public class ExportAgencies
    {
        private readonly AgencyService _agencyService;
        private readonly CodeService _codeService;

        public ExportAgencies(AgencyService agencyService, CodeService codeService)
        {
            _agencyService = agencyService;
            _codeService = codeService;
        }

        public ActionResult Download()
        {
            var codeDict = _codeService.Query().ToDictionary(x => x.Id.ToString(), x => x.Label);

            using (var excel = new ExcelPackage())
            {
                var worksheet = excel.Workbook.Worksheets.Add("Agencies");
                worksheet.Cells[1, 1].Value = "Agencies";

                var exclude = new[] {"Codes", "UserId", "User", "IsNew"};
                var props = typeof(Agency).GetProperties()
                                          .Where(x => !exclude.Contains(x.Name))
                                          .ToList();

                //Header
                var row = worksheet.Dimension.End.Row + 1;
                for (var i = 0; i < props.Count; i++)
                {
                    worksheet.Cells[row, i + 1].Value = props[i].Name;
                }

                //Rows
                foreach (var agency in _agencyService.Query().Include(x => x.Codes).ToList())
                {
                    row = worksheet.Dimension.End.Row + 1;
                    for (var i = 0; i < props.Count; i++)
                    {
                        worksheet.Cells[row, i + 1].Value = GetValue(props[i], agency, codeDict);
                    }
                }

                return new FileContentResult(excel.GetAsByteArray(), MediaTypeNames.Application.Octet)
                {
                    FileDownloadName = "Agencies.xlsx",
                };
            }
        }

        private static object GetValue(PropertyInfo prop, Agency agency, IDictionary<string, string> codeDict)
        {
            if (prop.PropertyType == typeof(string[]))
            {
                var arr = (string[])prop.GetValue(agency);
                return string.Join("; ", arr.Select(x => codeDict[x]));
            }
            else
            {
                return prop.GetValue(agency);
            }
        }
    };
}
