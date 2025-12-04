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
        //public IActionResult Index()
        //{
        //    return View();
        //}
        //public IActionResult Index()
        //{
        //    return View();
        //}

        //public IActionResult Index()
        //{
        //    return View();
        //}

        //public IActionResult Index()
        //{
        //    return View();
        //}

    }
}
