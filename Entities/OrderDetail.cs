using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Smart_Library.Entities
{
    public class OrderDetail
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("OrderId")]
        public int? OrderId { get; set; }
        public virtual Order Order { get; set; } = null!;
        [ForeignKey("BookId")]
        public int? BookId { get; set; }
        public virtual Book Book { get; set; } = null!;
        public int Quantity { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public int NumOfDay { get; set; }
        [ForeignKey("StatusId")]
        public int? StatusId { get; set; }
        public virtual OrderStatus Status { get; set; } = null!;

    }
}