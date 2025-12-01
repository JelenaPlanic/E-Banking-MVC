using E_Banking_MVC.Repository;
using E_Banking_MVC.Repository.Interfaces;
using E_Banking_MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using E_Banking_MVC.Models;

namespace E_Banking_MVC.Controllers
{
    public class AccountController : Controller
    {
        private IConfiguration Configuration { get; }
        private IAccountRepository AccountRepository { get; }

        public AccountController(IConfiguration configuration) // transient
        {
            Configuration = configuration;
            AccountRepository = new AccountRepository(Configuration);
        }

        [HttpGet]
        public IActionResult Index()
        {
            AccountAccountsViewModel vm = new AccountAccountsViewModel();
            vm.Account = new Account();
            vm.Accounts = new Dictionary<Account, decimal>();
            List<Account> accounts = AccountRepository.GetAll();

            foreach(Account account in accounts)
            {
                vm.Accounts.Add(account, AccountRepository.GetTotal(account.Id));
            }
            return View(vm);
        }

        [HttpPost]
        public IActionResult Create(Account account) // model binding
        {
            if (!ModelState.IsValid)
            {
                AccountAccountsViewModel vm = new AccountAccountsViewModel();
                vm.Account = account;
                vm.Accounts = new Dictionary<Account, decimal>();
                List<Account> accounts = AccountRepository.GetAll();
                foreach(Account acc in accounts)
                {
                    vm.Accounts.Add(acc, AccountRepository.GetTotal(acc.Id));
                }
                return View("Index", vm);
            }

            AccountRepository.Create(account);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int AccountId)
        {
            Account foundedAccount = AccountRepository.GetOne(AccountId);
            return View(foundedAccount);
        }

        [HttpPost]
        public IActionResult Edit(Account account)
        {
            if (!ModelState.IsValid) // obrati paznju
            {
                return View("Edit", account);
            }

            AccountRepository.Update(account);
            return RedirectToAction("Index");

        }

        [HttpGet]
        public IActionResult Delete(int AccountId)
        {
            AccountRepository.Delete(AccountId);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Activate(int AccountId)
        {
            AccountRepository.UpdateStatus(AccountId, true);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Deactivate(int AccountId)
        {
            AccountRepository.UpdateStatus(AccountId, false);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Search()
        {
            AccountsSearchViewModel vm = new AccountsSearchViewModel();
            vm.Accounts = new Dictionary<Account, decimal>();
            List<Account> accounts = AccountRepository.GetAll();

            foreach(Account acc in accounts)
            {
                vm.Accounts.Add(acc, AccountRepository.GetTotal(acc.Id));
            }

            return View(vm);
        }

        [HttpPost]
        public IActionResult Search(string AccountHolder, string AccountNumber, decimal? AmountFrom, decimal? AmountTo, bool? Active)
        {
            AccountsSearchViewModel vm = new AccountsSearchViewModel();
            vm.Accounts = new Dictionary<Account, decimal>();

            List<Account> filteredAccounts = AccountRepository.SearchAccountsBy(AccountHolder,AccountNumber, AmountFrom,AmountTo, Active);
            
            foreach(Account account in filteredAccounts)
            {
                vm.Accounts.Add(account,AccountRepository.GetTotal(account.Id));
            }

            vm.AccountHolder = AccountHolder;
            vm.AccountNumber = AccountNumber;
            vm.AmountFrom = AmountFrom;
            vm.AmountTo = AmountTo;
            vm.Active = Active;

            return View("Search", vm);

        }
    }
}
