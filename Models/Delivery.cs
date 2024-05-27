namespace KhumaloCraft_Part2.Models
{
    public class Delivery
    {
        public int DeliveryId { get; set; }
        public string UserId { get; set; }
        public string Country { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public ICollection<OrderHistory>? OrderHistories { get; set; }
    }
}
