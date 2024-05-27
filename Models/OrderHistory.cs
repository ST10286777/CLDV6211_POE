using Microsoft.AspNetCore.Identity;

namespace KhumaloCraft_Part2.Models
{
    public class OrderHistory
    {
        public int OrderHistoryId { get; set; }
        public bool IsCart { get; set; }
        public string UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public bool IsProcessed { get; set; }
        public ICollection<Product> Products { get; set; }
        public ICollection<Delivery> Deliveries { get; set; }
    }
}