using E_Banking_MVC.Repository;
using E_Banking_MVC.Repository.Interfaces;
using E_Banking_MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using E_Banking_MVC.Models;

namespace E_Banking_MVC.Controllers
{
    public class PaymentController : Controller
    {
        private IConfiguration Configuration { get; }
        private IAccountRepository AccountRepository { get; }
        private IPaymentRepository PaymentRepository { get; }

        public PaymentController(IConfiguration configuration)
        {
            this.Configuration = configuration;
            this.AccountRepository = new AccountRepository(Configuration);
            this.PaymentRepository = new PaymentRepository(Configuration);
        }
        public IActionResult Index(int accountId)
        {
            PaymentPaymentsViewModel viewModel = new PaymentPaymentsViewModel();
            viewModel.Payment = new Payment();
            viewModel.Account = AccountRepository.GetOne(accountId);
            viewModel.Payments = PaymentRepository.GetAll(accountId);

            decimal total = 0;
            foreach(Payment payment in viewModel.Payments)
            {
                total += payment.Amount;
            }
            viewModel.Total = total;

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Create(Payment Payment)
        {
            if (!ModelState.IsValid)
            {
                PaymentPaymentsViewModel vm = new PaymentPaymentsViewModel();
                vm.Payment = Payment;
                vm.Account = AccountRepository.GetOne(vm.Payment.AccountId);
                vm.Payments = PaymentRepository.GetAll(vm.Payment.AccountId);
                decimal total = 0;
                foreach(Payment payment in vm.Payments)
                {
                    total += payment.Amount;
                }
                vm.Total = total;

                return View("Index", vm);
            }
            PaymentRepository.Create(Payment);
            return RedirectToAction("Index",new {@accountId = Payment.AccountId} );
        }


        public IActionResult Edit(int paymentId )
        {
            PaymentAccountsViewModel vm = new PaymentAccountsViewModel();
            vm.Payment = PaymentRepository.GetOne(paymentId);
            vm.Accounts = AccountRepository.GetAll();
            return View(vm);
        }

        [HttpPost]
        public IActionResult Edit(Payment Payment)
        {
            if (!ModelState.IsValid)
            {
                PaymentAccountsViewModel vm = new PaymentAccountsViewModel();
                vm.Payment = Payment;
                vm.Accounts = AccountRepository.GetAll();
                return View("Index", vm);
            }
            PaymentRepository.Update(Payment);
            return RedirectToAction("Index", new {@accountId = Payment.AccountId});
        }

        public IActionResult Delete(int paymentId, int accountId)
        {
            PaymentRepository.Delete(paymentId);
            return RedirectToAction("Index", new { @accountId = accountId });
        }

    }
}
