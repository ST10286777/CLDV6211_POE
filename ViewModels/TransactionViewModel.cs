using KhumaloCraft_Part2.Models;
using System.ComponentModel.DataAnnotations;

namespace KhumaloCraft_Part2.ViewModels
{
    public class TransactionViewModel
    {
        public int OrderHistoryId { get; set; }
        public string NameonCard { get; set; }
        public string CardNumber { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int SecurityCode { get; set; }
    }
}
