using E_Banking_MVC.Models;

namespace E_Banking_MVC.Repository.Interfaces
{
    public interface IAccountRepository
    {
        List<Account> GetAll();
        Account GetOne(int AccountId);

        void Create(Account account);
        void Update(Account account);

        void Delete(int AccountId);

        void UpdateStatus(int AccountId, bool status);

        decimal GetTotal(int AccountId);

        List<Account> SearchAccountsBy(string accountHolder, string accountNum, decimal? ammountFrom, decimal? ammountTo, bool? active);
    }
}
