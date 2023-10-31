namespace Smart_Library.Areas.Admin.Models
{
    public class UserViewModel
    {
        public string? Id { get; set; }
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? ProfileImage { get; set; }
        public string? Address { get; set; }
        public string? WorkspaceName { get; set; } = null!;
        public string? Role { get; set; }
        public DateTime? CreatedAt { get; set; }
        public bool IsLocked { get; set; }
    }
}