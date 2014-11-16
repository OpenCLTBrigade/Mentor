using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mentor
{
    /// <summary>
    /// Stores criteria to search for agencies
    /// </summary>
    [Table("AgencyCode")]
    public class AgencyCode
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Index("IX_AgencyCode_AgencyTypeCode", IsUnique = true, Order = 1)]
        public int AgencyId { get; set; }

        [Index("IX_AgencyCode_AgencyTypeCode", IsUnique = true, Order = 2)]
        [StringLength(100)]
        public string CodeType { get; set; }

        [Index("IX_AgencyCode_AgencyTypeCode", IsUnique = true, Order = 3)]
        public int CodeId { get; set; }

        public virtual Agency Agency { get; set; }

        public virtual Code Code { get; set; }
    };
}
