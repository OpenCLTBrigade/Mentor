using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mentor
{
    [Table("Agency")]
    public class Agency
    {
        public Agency()
        {
            Codes = new HashSet<AgencyCode>();
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

        public int? FullTimeStaffCount { get; set; }

        public int? PartTimeStaffCount { get; set; }

        public int? UnpaidStaffCount { get; set; }

        public int? MentorCount { get; set; }

        public int? MenteeCount { get; set; }

        public int? MenteeWaitingListCount { get; set; }

        public decimal? MentorScreeningFee { get; set; }

        [MaxLength]
        public string MentorCompensation { get; set; }

        public bool? HasTimelyResponse { get; set; }

        public bool? HasInterestInMma { get; set; }

        public virtual ICollection<AgencyCode> Codes { get; set; }
    };
}
