using E_Banking_MVC.Models;

namespace E_Banking_MVC.ViewModels
{
    public class PaymentAccountsViewModel
    {
        public Payment Payment { get; set; }
        public List<Account> Accounts { get; set; }
    }
}
