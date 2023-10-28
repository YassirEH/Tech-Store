namespace Core.Models
{
    public class ProductBuyer
    {
        public int ProductId { get; set; }
        public int BuyerId { get; set; }
        public Product Product { get; set; }
        public Buyer Buyer { get; set; }
    }
}
