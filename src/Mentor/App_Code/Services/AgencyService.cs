using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using Common;
using OfficeOpenXml;

namespace Mentor
{
    public class AgencyService
    {
        private readonly MentorDb _db;

        public AgencyService(MentorDb db)
        {
            _db = db;
        }

        public IQueryable<Agency> Query()
        {
            return _db.Agencies
                      .OrderBy(x => x.Name);
        }

        public Agency Find(int? id)
        {
            if (!id.HasValue)
                return null;

            return _db.Agencies
                      .Include(x => x.Codes)
                      .SingleOrDefault(x => x.Id == id);
        }

        public Agency Create()
        {
            return new Agency();
        }

        public void Save(Agency agency)
        {
            _db.Save(agency, agency.IsNew);
            _db.SaveChanges();
        }

        public void Import(Stream inputStream)
        {
            using (var excel = new ExcelPackage(inputStream))
            {
                var worksheet = excel.Workbook.Worksheets[1];
                for (var r = 2; r <= worksheet.Dimension.End.Row; r++)
                {
                    var name = worksheet.Cells[r, 1].GetValue<string>().Trim();
                    var agency = Query().Include(x => x.Codes).SingleOrDefault(x => x.Name == name) ?? Create();
                    agency.Name = name;
                    agency.MissionStatement = worksheet.Cells[r, 2].GetValue<string>() + Environment.NewLine
                                              + worksheet.Cells[r, 3].GetValue<string>();
                    agency.ProgramType = GetProgramTypes(worksheet.Cells[r, 4].GetValue<string>());
                    agency.Website = CleanUrl(worksheet.Cells[r, 5].GetValue<string>());
                    agency.Phone = CleanPhone(worksheet.Cells[r, 6].GetValue<string>());
                    Save(agency);
                }
            }
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

        public void Delete(int? id)
        {
            var agency = Find(id);
            if (agency != null)
            {
                _db.Agencies.Remove(agency);
            }
            _db.SaveChanges();
        }
    };
}
