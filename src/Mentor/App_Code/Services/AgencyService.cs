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
            return _db.Agencies;
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
    };
}
