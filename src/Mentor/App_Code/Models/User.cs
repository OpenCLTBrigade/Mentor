using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mentor
{
    [Table("User")]
    public class User
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(75)]
        [Index("IX_User_Email", IsUnique = true)]
        public string Email { get; set; }

        [Required]
        [StringLength(25)]
        public string Password { get; set; }

        [StringLength(35)]
        public string ResetCode { get; set; }

        public bool Active { get; set; }
    };
}
