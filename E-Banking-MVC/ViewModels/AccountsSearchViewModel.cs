using E_Banking_MVC.Models;

namespace E_Banking_MVC.ViewModels
{
    public class AccountsSearchViewModel
    {
        public Dictionary<Account,decimal> Accounts { get; set; }
        public string AccountHolder { get; set; }
        public string AccountNumber { get; set; }
        public decimal? AmountFrom { get; set; }
        public decimal? AmountTo { get; set; }
        public bool? Active { get; set; }

    }
}
