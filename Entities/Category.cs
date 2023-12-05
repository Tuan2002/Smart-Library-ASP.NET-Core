
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Smart_Library.Entities
{
    [Table("Categories")]
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        public string Name { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        [ForeignKey("User")]
        public string CreatedById { get; set; } = null!;
        public virtual ApplicationUser CreatedBy { get; set; } = null!;
        public virtual ICollection<Books> Books { get; set; } = null!;
        public bool Status { get; set; } = true;
        public string Slug { get; set; } = null!;
    }
}