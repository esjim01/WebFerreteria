using Microsoft.AspNetCore.Mvc;

namespace WebFerreteria.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
