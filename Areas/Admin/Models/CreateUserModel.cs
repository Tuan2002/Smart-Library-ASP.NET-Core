
using Microsoft.AspNetCore.Mvc;

namespace Smart_Library.Admin.Models
{
    [BindProperties]
    public class CreateUserModel
    {
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Phone { get; set; }
        public IFormFile? ProfileImage { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public string? Province { get; set; }
        public string? District { get; set; }
        public string? WorkspaceId { get; set; }
        public string? RoleName { get; set; }
        public string Address
        {
            get
            {
                return $"{District}, {Province}";
            }
        }
    }
}