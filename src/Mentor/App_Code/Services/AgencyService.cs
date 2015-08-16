using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using Common;

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
            return _db.Agencies.OrderBy(x => x.Name);
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

        public void Save(Agency agency, string username = null, string password = null)
        {
            if (string.IsNullOrWhiteSpace(agency.Name))
            {
                throw new ApplicationException("Please enter an agency name.");
            }
            else
            {
                agency.Name = agency.Name.Trim();
            }

            if (agency.IsNew && _db.Agencies.Any(x => x.Name == agency.Name))
            {
                throw new ApplicationException("The agency name '" + agency.Name + "' already exists. Please enter a unqiue name for the new agency.");
            }

            if (!string.IsNullOrEmpty(username) && !string.IsNullOrWhiteSpace(password))
            {
                var user = _db.Users.SingleOrDefault(x => x.Email == username);
                if (user == null)
                {
                    user = new User {Email = username, Password = password, Active = true};
                }
                else if (user.Password != password)
                {
                    throw new ApplicationException("Invalid password");
                }
                agency.User = user;
            }

            if (!string.IsNullOrWhiteSpace(agency.Website) && !agency.Website.ToLower().StartsWith("http"))
            {
                agency.Website = "http://" + agency.Website;
            }

            _db.Save(agency, agency.IsNew);
            _db.SaveChanges();
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

        public List<Agency> FindByUser(string username)
        {
            return _db.Agencies
                      .Where(x => x.User.Email == username)
                      .ToList();
        }

        private readonly static string[] IMAGE_EXTENSIONS = { "gif", "jpeg", "jpg", "png"};
        public void SaveLogo(HttpPostedFileBase file, Agency agency)
        {
            if (file == null || file.ContentLength == 0 || file.InputStream == null || string.IsNullOrWhiteSpace(file.FileName))
                return;

            var ext = Path.GetExtension(file.FileName).ToLower().Trim('.');
            if (!IMAGE_EXTENSIONS.Contains(ext))
                return;

            if (agency.Id == 0)
                return;

            //Delete existing logo
            if (agency.Logo != null)
            {
                try
                {
                    var existingLogoPath = HostingEnvironment.MapPath(agency.Logo);
                    if (existingLogoPath != null && File.Exists(existingLogoPath))
                    {
                        File.Delete(existingLogoPath);
                    }
                }
                catch
                {
                    //ignore errors
                }
            }

            agency.Logo = VirtualPathUtility.ToAbsolute("~/Content/logos/" + agency.Id + "-" + agency.Name.Slug() + "." + ext);

            //Save Logo
            var logoPath = HostingEnvironment.MapPath(agency.Logo);
            Directory.CreateDirectory(Path.GetDirectoryName(logoPath));
            file.SaveAs(logoPath);

            _db.SaveChanges();
        }
    };
}
