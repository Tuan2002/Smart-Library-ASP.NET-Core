using Microsoft.AspNetCore.Mvc;

namespace Smart_Library.Areas.Admin.Models
{
    [BindProperties]
    public class UpdatePublisherModel
    {
        public int PublisherId { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
    }
}