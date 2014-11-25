using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Common;

namespace Mentor
{
    public class AgencySearch
    {
        public int? Miles { get; set; }
        public string Address { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
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

        public List<AgencyInfo> Agencies { get; set; }

        public void Filter(IQueryable<Agency> agencies)
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

            var list = query.Select(Transform)
                            .OrderBy(x => x.Name)
                            .ToList();

            if (Latitude.HasValue && Longitude.HasValue)
            {
                list.ForEach(x => x.Distance = Ext.CalcDist(Latitude, Longitude, x.Latitude, x.Longitude));
                list.RemoveAll(x => x.Distance == null || x.Distance > Miles);
            }
            Agencies = list;
        }

        public static AgencyInfo Transform(Agency agency)
        {
            return new AgencyInfo
            {
                Id = agency.Id,
                Name = agency.Name,
                Address = agency.Address,
                Latitude = (double?)agency.Latitude,
                Longitude = (double?)agency.Longitude,
                CSZ = agency.City + ", " + agency.State + " " + agency.ZipCode,
                Phone = agency.Phone,
                Website = agency.Website,
            };
        }

        private static IQueryable<Agency> FilterCode(IQueryable<Agency> agencies, string type, string[] stringIds)
        {
            if (agencies == null || stringIds == null || stringIds.Length < 1)
                return agencies;

            var ids = stringIds.Select(int.Parse);
            return agencies.Where(a => a.Codes.Any(c => c.CodeType == type && ids.Contains(c.CodeId)));
        }

        public class AgencyInfo
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Address { get; set; }
            public double? Latitude { get; set; }
            public double? Longitude { get; set; }
            public double? Distance { get; set; }
            public string CSZ { get; set; }
            public string Phone { get; set; }
            public string Website { get; set; }
        };
    };
}
