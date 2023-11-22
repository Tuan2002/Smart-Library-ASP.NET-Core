using Microsoft.AspNetCore.Mvc;

namespace Smart_Library.Areas.Admin.Models
{
    [BindProperties]
    public class EditUserModel
    {
        public string? Email { get; set; } = null!;
        public string? FirstName { get; set; }
        public string? LastName { get; set; } = null!;
        public string? Phone { get; set; }
        public IFormFile? ImageFile { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public string? Address { get; set; } = null!;
        public int? WorkspaceId { get; set; }
        public string RoleName { get; set; } = null!;
    }
}