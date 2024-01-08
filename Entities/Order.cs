
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Smart_Library.Entities
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; } = null!;
        public virtual ApplicationUser User { get; set; } = null!;
        public DateTime OrderDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public int TotalBook { get; set; }
        [ForeignKey("StatusId")]
        public int? StatusId { get; set; }
        public virtual OrderStatus Status { get; set; } = null!;
        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = null!;
    }
}