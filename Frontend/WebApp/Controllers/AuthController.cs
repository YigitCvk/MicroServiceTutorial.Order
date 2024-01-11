using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult SignIn()
        {
            return View();
        }
    }
}
