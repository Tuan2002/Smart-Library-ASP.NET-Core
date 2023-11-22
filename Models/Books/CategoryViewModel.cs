
namespace Smart_Library.Models
{
    public class CategoryViewModel
    {
        public int CategoryId { get; set; }
        public string Name { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public string? CreatedById { get; set; }
        public string CreatedByName { get; set; } = null!;
        public bool Status { get; set; } = true;
    }
}