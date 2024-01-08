using System.ComponentModel.DataAnnotations;

namespace Smart_Library.Entities
{
    public class OrderStatus
    {
        [Key]
        public int StatusId { get; set; }
        public string StatusName { get; set; } = null!;
        public string Description { get; set; } = null!;
    }
}