
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Smart_Library.Entities
{
    [Table("SupportTypes")]
    public class SupportType
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = null!;
    }
}