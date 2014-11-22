using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Common;

namespace Mentor
{
    [Table("Agency")]
    public class Agency
    {
        public Agency()
        {
            Codes = new HashSet<AgencyCode>();
            Users = new HashSet<User>();
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [DisplayName("Organization Name")]
        public string Name { get; set; }

        [StringLength(100)]
        [DisplayName("Program Name")]
        public string ProgramName { get; set; }

        [StringLength(50)]
        public string Address { get; set; }

        [StringLength(25)]
        public string City { get; set; }

        [StringLength(2)]
        [UIHint("_States")]
        public string State { get; set; }

        [StringLength(10)]
        [DisplayName("Zip Code")]
        public string ZipCode { get; set; }

        [StringLength(25)]
        [DisplayName("Main Phone Number")]
        public string Phone { get; set; }

        [StringLength(100)]
        [DisplayName("Website URL")]
        public string Website { get; set; }

        [MaxLength]
        [DisplayName("Mission Statement")]
        public string MissionStatement { get; set; }

        [StringLength(25)]
        [DisplayName("First Name")]
        public string ContactFirstName { get; set; }

        [StringLength(25)]
        [DisplayName("Last Name")]
        public string ContactLastName { get; set; }

        [StringLength(50)]
        [DisplayName("Title")]
        public string ContactTitle { get; set; }

        [StringLength(25)]
        [DisplayName("Phone")]
        public string ContactPhone { get; set; }

        [StringLength(100)]
        [DisplayName("E-mail")]
        public string ContactEmail { get; set; }

        [StringLength(25)]
        [DisplayName("First Name")]
        public string ReferralFirstName { get; set; }

        [StringLength(25)]
        [DisplayName("Last Name")]
        public string ReferralLastName { get; set; }

        [StringLength(50)]
        [DisplayName("Title")]
        public string ReferralTitle { get; set; }

        [StringLength(25)]
        [DisplayName("Phone")]
        public string ReferralPhone { get; set; }

        [StringLength(100)]
        [DisplayName("E-mail")]
        public string ReferralEmail { get; set; }

        [DisplayName("501c3 organization")]
        [UIHint("_YesNo")]
        public bool? IsNonProfit { get; set; }

        [DecimalPrecision(9, 6)]
        public decimal? Latitude { get; set; }

        [DecimalPrecision(9, 6)]
        public decimal? Longitude { get; set; }

        [NotMapped]
        public string[] ProgramType
        {
            get { return null; }
            set { SetCodes("ProgramType", value.Select(int.Parse).ToList()); }
        }

        [NotMapped]
        public string[] MeetingLocation
        {
            get { return null; }
            set { SetCodes("MeetingLocation", value.Select(int.Parse).ToList()); }
        }

        [StringLength(100)]
        public string MeetingLocationOther { get; set; }

        [NotMapped]
        public string[] MetroSection
        {
            get { return null; }
            set { SetCodes("MetroSection", value.Select(int.Parse).ToList()); }
        }

        [NotMapped]
        public string[] MentorAge
        {
            get { return null; }
            set { SetCodes("MentorAge", value.Select(int.Parse).ToList()); }
        }

        [NotMapped]
        public string[] MenteeAge
        {
            get { return null; }
            set { SetCodes("MenteeAge", value.Select(int.Parse).ToList()); }
        }

        [NotMapped]
        public string[] MenteeGender
        {
            get { return null; }
            set { SetCodes("MenteeGender", value.Select(int.Parse).ToList()); }
        }

        [NotMapped]
        public string[] ReferralMethod
        {
            get { return null; }
            set { SetCodes("ReferralMethod", value.Select(int.Parse).ToList()); }
        }

        [StringLength(100)]
        public string ReferralMethodOther { get; set; }

        [NotMapped]
        public string[] ExpectedCommitment
        {
            get { return null; }
            set { SetCodes("ExpectedCommitment", value.Select(int.Parse).ToList()); }
        }

        [NotMapped]
        public string[] MentoringFrequency
        {
            get { return null; }
            set { SetCodes("MentoringFrequency", value.Select(int.Parse).ToList()); }
        }

        [NotMapped]
        public string[] SessionLength
        {
            get { return null; }
            set { SetCodes("SessionLength", value.Select(int.Parse).ToList()); }
        }

        [NotMapped]
        public string[] ProgramExistence
        {
            get { return null; }
            set { SetCodes("ProgramExistence", value.Select(int.Parse).ToList()); }
        }

        public int? FullTimeStaffCount { get; set; }

        public int? PartTimeStaffCount { get; set; }

        public int? UnpaidStaffCount { get; set; }

        public int? MentorCount { get; set; }

        public int? MenteeCount { get; set; }

        [NotMapped]
        public string[] MenteeIntake
        {
            get { return null; }
            set { SetCodes("MenteeIntake", value.Select(int.Parse).ToList()); }
        }

        [StringLength(100)]
        public string MenteeIntakeOther { get; set; }

        [NotMapped]
        public string[] MentorScreening
        {
            get { return null; }
            set { SetCodes("MentorScreening", value.Select(int.Parse).ToList()); }
        }

        [StringLength(100)]
        public string MentorScreeningOther { get; set; }

        [NotMapped]
        public string[] MentorTraining
        {
            get { return null; }
            set { SetCodes("MentorTraining", value.Select(int.Parse).ToList()); }
        }

        [StringLength(100)]
        public string MentorTrainingOther { get; set; }

        public int? MenteeWaitingListCount { get; set; }

        public decimal? MentorScreeningFee { get; set; }

        [MaxLength]
        public string MentorCompensation { get; set; }

        [UIHint("_YesNo")]
        public bool? HasTimelyResponse { get; set; }

        [UIHint("_YesNo")]
        public bool? HasInterestInMma { get; set; }

        [NotMapped]
        public bool IsNew { get { return Id == 0; } }

        public virtual ICollection<AgencyCode> Codes { get; set; }

        public virtual ICollection<User> Users { get; set; }

        public string[] GetCodes(string type)
        {
            return Codes.Where(x => x.CodeType == type)
                        .Select(x => x.CodeId.ToString())
                        .ToArray();
        }

        public void SetCodes(string type, ICollection<int> ids)
        {
            var existing = Codes.Where(x => x.CodeType == type).ToList();

            //add new codes
            foreach (var id in ids)
            {
                if (existing.Any(x => x.CodeId == id))
                    continue;

                Codes.Add(new AgencyCode { CodeType = type, CodeId = id });
            }

            //remove old codes
            foreach (var code in new List<AgencyCode>(existing))
            {
                if (ids.Any(x => x == code.CodeId))
                    continue;

                Codes.Remove(code);
            }
        }
    };
}
