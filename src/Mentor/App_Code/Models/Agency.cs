using System.Collections.Generic;
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
        public string Name { get; set; }

        [StringLength(100)]
        public string ProgramName { get; set; }

        [StringLength(50)]
        public string Address { get; set; }

        [StringLength(25)]
        public string City { get; set; }

        [StringLength(2)]
        public string State { get; set; }

        [StringLength(10)]
        public string ZipCode { get; set; }

        [StringLength(25)]
        public string Phone { get; set; }

        [StringLength(100)]
        public string Website { get; set; }

        [MaxLength]
        public string MissionStatement { get; set; }

        [StringLength(25)]
        public string ContactFirstName { get; set; }

        [StringLength(25)]
        public string ContactLastName { get; set; }

        [StringLength(50)]
        public string ContactTitle { get; set; }

        [StringLength(25)]
        public string ContactPhone { get; set; }

        [StringLength(100)]
        public string ContactEmail { get; set; }

        [StringLength(25)]
        public string ReferralFirstName { get; set; }

        [StringLength(25)]
        public string ReferralLastName { get; set; }

        [StringLength(50)]
        public string ReferralTitle { get; set; }

        [StringLength(25)]
        public string ReferralPhone { get; set; }

        [StringLength(100)]
        public string ReferralEmail { get; set; }

        public bool? IsNonProfit { get; set; }

        public virtual ICollection<AgencyCode> Codes { get; set; }
    };
}
