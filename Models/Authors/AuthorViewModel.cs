
namespace Smart_Library.Models
{
    public class AuthorViewModel
    {
        public int AuthorId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Slug { get; set; }
        public string? ImageURL { get; set; }
        public int? TotalBooks { get; set; }
        public string? Address { get; set; }
        public string? Title { get; set; }
        public DateTime? AddedAt { get; set; }
        public string? AddedByName { get; set; }

    }
}