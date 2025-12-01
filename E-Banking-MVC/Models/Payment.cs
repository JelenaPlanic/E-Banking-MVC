using System.ComponentModel.DataAnnotations;

namespace E_Banking_MVC.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public int AccountId { get; set; }

        [Required(ErrorMessage = "The Amount Field is required")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "The TransactionDate Field is required")]
        public DateTime TransactionDate { get; set; }

        [Required(ErrorMessage = "The Purpose Field is required")]
        public string Purpose { get; set; }

        [Required(ErrorMessage = "The PayerName Field is required")]
        public string PayerName { get; set; }
        public bool IsUrgent { get; set; }
        
    }
}
