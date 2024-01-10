using Microsoft.AspNetCore.Mvc;

namespace Order.API.Models
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
