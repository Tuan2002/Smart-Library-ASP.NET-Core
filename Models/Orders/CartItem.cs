namespace Smart_Library.Models
{
    public class CartItem
    {
        public int BookId { get; set; }
        public BookViewModel Book { get; set; } = null!;
        public int Quantity { get; set; }
        public int NumOfDay { get; set; }
    }
}