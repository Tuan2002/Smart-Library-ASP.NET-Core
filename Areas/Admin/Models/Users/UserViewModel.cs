namespace Smart_Library.Areas.Admin.Models
{
    public class UserViewModel
    {
        public string? Id { get; set; }
        public string Email { get; set; } = null!;
        public string? FirstName { get; set; }
        public string LastName { get; set; } = null!;
        public string? ProfileImage { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public int? WorkspaceId { get; set; } = null!;
        public string? WorkspaceName { get; set; } = null!;
        public string? Role { get; set; } = null!;
        public DateTime? CreatedAt { get; set; }
        public bool IsLocked { get; set; }
    }
}