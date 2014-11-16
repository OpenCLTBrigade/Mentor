using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mentor
{
    /// <summary>
    /// Stores content page data for website
    /// </summary>
    [Table("Article")]
    public class Article
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; private set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Body { get; set; }
        public DateTime Modified { get; set; }
    };
}
