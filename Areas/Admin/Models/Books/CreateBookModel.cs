using Microsoft.AspNetCore.Mvc;

namespace Smart_Library.Areas.Admin.Models
{
    [BindProperties]
    public class CreateBookModel
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public IFormFile? Image { get; set; }
        public string? Language { get; set; }
        public string? Pages { get; set; }
        public int CategoryId { get; set; }
        public int AuthorId { get; set; }
        public int PublisherId { get; set; }
        public bool? IsEBook { get; set; }
        public IFormFile? Pdf { get; set; }
        public bool IsPublish { get; set; }

    }
}