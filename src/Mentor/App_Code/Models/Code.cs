using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mentor
{
    /// <summary>
    /// Generic table to store codes for lookups and such
    /// </summary>
    [Table("Code")]
    public class Code
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Index("IX_Code_TypeValue", IsUnique = true, Order = 1)]
        [StringLength(100)]
        public string Type { get; set; }

        [Required]
        [Index("IX_Code_TypeValue", IsUnique = true, Order = 2)]
        [StringLength(100)]
        public string Value { get; set; }

        [StringLength(100)]
        public string Label { get; set; }

        public int Seq { get; set; }

        [MaxLength]
        public string Data { get; set; }

        [NotMapped]
        public bool IsNew { get { return Id == 0; } }
    };
}
