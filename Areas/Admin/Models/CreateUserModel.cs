
using Microsoft.AspNetCore.Mvc;

namespace Smart_Library.Admin.Models
{
    [BindProperties]
    public class CreateUserModel
    {
        public string Email { get; set; } = null!;
        public string? FirstName { get; set; }
        public string LastName { get; set; } = null!;
        public string? Phone { get; set; }
        public IFormFile? ProfileImage { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string? Province { get; set; }
        public string? District { get; set; }
        public string WorkspaceId { get; set; } = null!;
        public string RoleName { get; set; } = null!;
        public string GetAddress()
        {
            return District + ", " + Province;
        }
        public string GeneratePassword()
        {
            string? defaultUsername = Email?.Split("@")[0];
            string? defaultPassword = defaultUsername + "@" + DateOfBirth.ToString("yyyy");
            return defaultPassword;
        }
    }
}