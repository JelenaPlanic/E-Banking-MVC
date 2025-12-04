using E_Banking_MVC.Models;

namespace E_Banking_MVC.ViewModels
{
    public class PaymentsSearchViewModel
    {
        public List<Payment> Payments { get; set; } // results
        public Account Account { get; set; } // data account
        public string Transant { get; set; } // filter parameter
        public decimal? AmountFrom { get; set; }  // filter parameter
        public decimal? AmountTo { get; set; } // filter parameter
        public DateTime? DateFrom { get; set; } // filter parameter
        public DateTime? DateTo { get; set; } // filter parameter
    }
}
