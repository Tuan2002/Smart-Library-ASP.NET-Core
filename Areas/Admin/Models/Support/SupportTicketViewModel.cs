
namespace Smart_Library.Areas.Admin.Models
{
    public class SupportTicketViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string RequestDescription { get; set; } = null!;
        public string? StudyMajor { get; set; }
        public string? StudyGrade { get; set; }
        public string? EnglishLevel { get; set; }
        public string? SupportType { get; set; }
        public bool Status { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}