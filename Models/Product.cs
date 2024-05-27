namespace KhumaloCraft_Part2.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string? Name { get; set; }
        public double? Price { get; set; }
        public string? Category { get; set; }
        public int? Quantity { get; set; }
        public string? ImageUrl { get; set; }
        public string? Description { get; set; }
        public ICollection<OrderHistory>? OrderHistories { get; set; } 

    }
}
