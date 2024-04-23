using Smart_Library.Entities;

namespace Smart_Library.Models
{
    public class OrderDetailViewModel
    {
        public int Id { get; set; }
        public int? OrderId { get; set; }
        public virtual OrderViewModel Order { get; set; } = null!;
        public int? BookId { get; set; }
        public virtual BookViewModel Book { get; set; } = null!;
        public int Quantity { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public int NumOfDay { get; set; }
        public int? StatusId { get; set; }
        public virtual OrderStatus Status { get; set; } = null!;
    }
}