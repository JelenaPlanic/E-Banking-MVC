using Microsoft.AspNetCore.Mvc;

namespace E_Banking_MVC.Controllers
{
    public class PaymentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
