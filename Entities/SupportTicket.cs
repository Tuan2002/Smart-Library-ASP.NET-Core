
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Smart_Library.Entities
{
    [Table("SupportTickets")]
    public class SupportTicket
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string RequestDescription { get; set; } = null!;
        public string? StudyMajor { get; set; }
        public string? StudyGrade { get; set; }
        public string? EnglishLevel { get; set; }
        public bool Status { get; set; }
        [ForeignKey("SupportTypeId")]
        public int SupportTypeId { get; set; }
        public virtual SupportType SupportType { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}