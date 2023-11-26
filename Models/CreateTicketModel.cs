
using Microsoft.AspNetCore.Mvc;

namespace Smart_Library.Models
{
    [BindProperties]
    public class CreateTicketModel
    {
        public string Title { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string RequestDescription { get; set; } = null!;
        public string? StudyMajor { get; set; }
        public string? StudyGrade { get; set; }
        public string? EnglishLevel { get; set; }
        public int SupportTypeId { get; set; }
    }
}