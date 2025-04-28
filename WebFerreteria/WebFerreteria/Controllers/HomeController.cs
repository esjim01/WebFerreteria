using System.Diagnostics;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebFerreteria.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace WebFerreteria.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

    //A partir del este espacio de prueba el codigo
    public class AccountController : Controller
    {
        private readonly FerreteriaDbContext _context;

        public AccountController(FerreteriaDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Usuarios model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                Console.WriteLine($"Intentando autenticar con correo: {model.Correo}");

                var usuario = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.Correo == model.Correo && u.Contrasena == model.Contrasena);

                if (usuario != null)
                {
                    Console.WriteLine($"Usuario encontrado: {usuario.Correo}, Rol: {usuario.Rol}");

                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, usuario.Correo),
                new Claim(ClaimTypes.Role, usuario.Rol ?? "Usuario"),
                new Claim("FullName", usuario.NombreCompleto ?? "Usuario")
            };

                    var claimsIdentity = new ClaimsIdentity(
                        claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    Console.WriteLine("Creando cookie de autenticación...");
                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity));

                    Console.WriteLine("Cookie creada. Redirigiendo...");

                    // Redirige al returnUrl o a la página principal
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        Console.WriteLine($"Redirigiendo a: {returnUrl}");
                        return LocalRedirect(returnUrl);
                    }
                    Console.WriteLine("Redirigiendo a /Home/Index");
                    return RedirectToAction("Index", "Home");
                }

                Console.WriteLine("Usuario no encontrado.");
                ModelState.AddModelError(string.Empty, "Credenciales inválidas");
            }
            else
            {
                Console.WriteLine("ModelState no es válido.");
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"Error de validación: {error.ErrorMessage}");
                }
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Home", "Index");
        }
    }
}
