using KhumaloCraft_Part2.Models;

namespace KhumaloCraft_Part2.ViewModels
{
    public class OrderHistoryView
    {
        public List<OrderHistory> OrderHistories {  get; set; }
        public string FilterUserId {  get; set; }
        public string FilterUserEmail { get; set; }
        public List<Delivery> Deliveries { get; set; }
        public List<Transaction> Transactions { get; set; }
        public OrderHistoryView()
        {
            Transactions = new List<Transaction>(); 
        }
    }
}
