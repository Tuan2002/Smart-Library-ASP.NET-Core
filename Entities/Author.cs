
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Smart_Library.Entities
{
    [Table("Authors")]
    public class Author
    {
        [Key]
        public int AuthorId { get; set; }
        public string Name { get; set; } = null!;
        public string? Email { get; set; }
        public string? Slug { get; set; }
        public string? ImageURL { get; set; } = null!;
        public string? Address { get; set; } = null!;
        public string? Title { get; set; }
        public DateTime AddedAt { get; set; }
        [ForeignKey("User")]
        public string AddedById { get; set; } = null!;
        public virtual ApplicationUser AddedBy { get; set; } = null!;
        public virtual ICollection<Books> Books { get; set; } = null!;
    }
}