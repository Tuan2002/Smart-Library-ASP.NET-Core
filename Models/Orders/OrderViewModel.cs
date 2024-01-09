using Smart_Library.Entities;

namespace Smart_Library.Models
{
    public class OrderViewModel
    {
        public int OrderId { get; set; }
        public string UserId { get; set; } = null!;
        public virtual ApplicationUser User { get; set; } = null!;
        public int TotalBook { get; set; }
        public int? StatusId { get; set; }
        public DateTime CreateDate { get; set; }
        public virtual OrderStatus Status { get; set; } = null!;
        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = null!;
    }
}