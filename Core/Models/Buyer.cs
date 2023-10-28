namespace Core.Models
{
    public class Buyer
    {
        public int Id { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
        public ICollection<ProductBuyer> ProductBuyers { get; set; }
    }
}

