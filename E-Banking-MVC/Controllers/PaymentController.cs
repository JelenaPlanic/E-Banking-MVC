using E_Banking_MVC.Repository;
using E_Banking_MVC.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }

    }
}
