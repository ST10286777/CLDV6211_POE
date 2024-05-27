using KhumaloCraft_Part2.Models;

namespace KhumaloCraft_Part2.ViewModels
{
    public class CartViewModel
    {
        public OrderHistory CurrentCart { get; set; }
        public List<Delivery> PreviousDeliveries { get; set; }
    }
}
