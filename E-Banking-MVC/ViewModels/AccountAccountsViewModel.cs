using E_Banking_MVC.Models;

namespace E_Banking_MVC.ViewModels
{
    public class AccountAccountsViewModel
    {
        public Account Account { get; set; }
        public Dictionary<Account,decimal> Accounts { get; set; }
    }
}
