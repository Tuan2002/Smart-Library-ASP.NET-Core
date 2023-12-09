
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Smart_Library.Entities
{
    [Table("Publishers")]
    public class Publisher
    {
        [Key]
        public int PublisherId { get; set; }
        public string Name { get; set; } = null!;
        public string? Slug { get; set; }
        public string? Address { get; set; } = null!;
        public DateTime AddedAt { get; set; }
        [Required]
        [ForeignKey("User")]
        public string AddedById { get; set; } = null!;
        public virtual ApplicationUser AddedBy { get; set; } = null!;
        public virtual ICollection<Book> Books { get; set; } = null!;
    }
}