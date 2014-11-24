using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Mentor
{
    public class AgencySearch
    {
        public int? Miles { get; set; }
        public string Address { get; set; }
        public string[] ProgramType { get; set; }
        public string[] MenteeAge { get; set; }
        public string[] MenteeGender { get; set; }
        public string Q { get; set; }
        public string[] MeetingLocation { get; set; }
        public string[] MetroSection { get; set; }
        public string[] MentorAge { get; set; }
        public string[] ReferralMethod { get; set; }
        public string[] ExpectedCommitment { get; set; }
        public string[] MentoringFrequency { get; set; }
        public string[] SessionLength { get; set; }

        public List<Agency> Agencies { get; set; }

        public IQueryable<Agency> Filter(IQueryable<Agency> agencies)
        {
            var query = agencies.Include(x => x.Codes).AsQueryable();

            query = FilterCode(query, "ProgramType", ProgramType);
            query = FilterCode(query, "MenteeAge", MenteeAge);
            query = FilterCode(query, "MenteeGender", MenteeGender);

            if (!string.IsNullOrWhiteSpace(Q))
            {
                query = query.Where(x => x.Name.Contains(Q) || x.MissionStatement.Contains(Q));
            }

            query = FilterCode(query, "MeetingLocation", MeetingLocation);
            query = FilterCode(query, "MetroSection", MetroSection);
            query = FilterCode(query, "MentorAge", MentorAge);
            query = FilterCode(query, "ReferralMethod", ReferralMethod);
            query = FilterCode(query, "ExpectedCommitment", ExpectedCommitment);
            query = FilterCode(query, "MentoringFrequency", MentoringFrequency);
            query = FilterCode(query, "SessionLength", SessionLength);

            return query.OrderBy(x => x.Name);
        }

        private static IQueryable<Agency> FilterCode(IQueryable<Agency> agencies, string type, string[] stringIds)
        {
            if (agencies == null || stringIds == null || stringIds.Length < 1)
                return agencies;

            var ids = stringIds.Select(int.Parse);
            return agencies.Where(a => a.Codes.Any(c => c.CodeType == type && ids.Contains(c.CodeId)));
        }
    };
}
