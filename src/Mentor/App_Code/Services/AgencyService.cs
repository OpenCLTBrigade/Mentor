using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

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
    };
}
