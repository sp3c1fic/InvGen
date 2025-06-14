using Microsoft.AspNetCore.Mvc;

namespace InvoiceGenerator.Controllers
{
    public class PaymentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public void MakePayment()
        {

        }
    }
}
