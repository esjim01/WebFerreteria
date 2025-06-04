// Controllers/AccountController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using WebFerreteria.Models;
using WebFerreteria.Models.ViewModels;

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

    //Lo ideal es que tengan una vista (carpeta) llamada Account y dentro de la vista una pagina llamada Login
    // Deben tener un método llamado Login(), que sólo va a retornar la vista
    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
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
        return RedirectToAction("Login", "Account");
    }
}