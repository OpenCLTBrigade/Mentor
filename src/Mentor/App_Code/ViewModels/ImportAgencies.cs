using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using Common;
using OfficeOpenXml;

namespace Mentor
{
    public class ImportAgencies
    {
        private readonly AgencyService _agencyService;

        public ImportAgencies(AgencyService agencyService)
        {
            _agencyService = agencyService;
        }

        public int Import(Stream inputStream)
        {
            var count = 0;
            using (var excel = new ExcelPackage(inputStream))
            {
                var worksheet = excel.Workbook.Worksheets[1];
                for (var r = 2; r <= worksheet.Dimension.End.Row; r++)
                {
                    var name = worksheet.Cells[r, 1].GetValue<string>().Trim();

                    var agency = _agencyService.Query()
                                               .Include(x => x.Codes)
                                               .SingleOrDefault(x => x.Name == name)
                                               ?? _agencyService.Create();

                    agency.Name = name;
                    agency.MissionStatement = worksheet.Cells[r, 2].GetValue<string>() + Environment.NewLine
                                              + worksheet.Cells[r, 3].GetValue<string>();
                    agency.ProgramType = GetProgramTypes(worksheet.Cells[r, 4].GetValue<string>());
                    agency.Website = CleanUrl(worksheet.Cells[r, 5].GetValue<string>());
                    agency.Phone = CleanPhone(worksheet.Cells[r, 6].GetValue<string>());

                    _agencyService.Save(agency);
                    count++;
                }
            }
            return count;
        }

        private static string[] GetProgramTypes(string types)
        {
            var list = new List<string>();
            if (types.Contains("one-to-one", true)) list.Add("1");
            if (types.Contains("group", true)) list.Add("2");
            //if (types.Contains("peer to peer")) list.Add("");
            if (types.Contains("e-mentoring", true)) list.Add("5");
            return list.ToArray();
        }

        private static string CleanUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url) || url.Length < 5)
                return null;

            return url.StartsWith("http") ? url : "http://" + url;
        }

        private static string CleanPhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return null;

            return phone.Split(',').First().Trim();
        }
    };
}
