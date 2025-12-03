using E_Banking_MVC.Models;

namespace E_Banking_MVC.ViewModels
{
    public class PaymentPaymentsViewModel   // container for data
    {
        public List<Payment> Payments { get; set; }
        public Account Account { get; set; }
        public Payment Payment { get; set; }
        public decimal Total { get; set; }

    }
}
