using E_Banking_MVC.Models;

namespace E_Banking_MVC.Repository.Interfaces
{
    public interface IAccountRepository
    {
        List<Account> GetAll();
        Account GetOne(int AccountId);
    }
}
