
namespace Smart_Library.Models
{
    public class PublisherViewModel
    {
        public int PublisherId { get; set; }
        public string Name { get; set; } = null!;
        public string? Slug { get; set; }
        public string? Address { get; set; }
        public DateTime? AddedAt { get; set; }
        public string? AddedById { get; set; }
        public string? AddedByName { get; set; } = null!;
    }
}