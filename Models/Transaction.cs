using System;
using System.ComponentModel.DataAnnotations;

namespace KhumaloCraft_Part2.Models
{
    public class Transaction
    {
        public int TransactionId { get; set; }
        public string UserId { get; set; }
        public string NameonCard { get; set; }
        public string CardNumber { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int SecurityCode { get; set; }
        public int OrderHistoryId { get; set; }
        public ICollection<OrderHistory>? OrderHistories { get; set; }
    }
}
