using System.Collections.Generic;
using System.Linq;

namespace Mentor
{
    public class CodeService
    {
        private readonly MentorDb _db;

        public CodeService(MentorDb db)
        {
            _db = db;
        }

        public IQueryable<Code> Query(string type)
        {
            if (string.IsNullOrWhiteSpace(type))
                return new List<Code>().AsQueryable();

            return _db.Codes
                      .Where(x => x.Type == type)
                      .OrderBy(x => x.Seq)
                      .ThenBy(x => x.Value);
        }

        public List<string> GetTypes()
        {
            return _db.Codes
                      .OrderBy(x => x.Type)
                      .Select(x => x.Type)
                      .Distinct()
                      .ToList();
        }

        public void Save(Code code)
        {
            if (code == null || string.IsNullOrWhiteSpace(code.Value))
                return;

            _db.Save(code, code.IsNew);
            _db.SaveChanges();
        }
    };
}
