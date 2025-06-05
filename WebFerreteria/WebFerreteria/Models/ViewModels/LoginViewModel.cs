// Ubicación sugerida: Models/ViewModels/LoginViewModel.cs
using System.ComponentModel.DataAnnotations;

namespace WebFerreteria.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "El correo es obligatorio.")]
        [EmailAddress(ErrorMessage = "Correo inválido.")]
        public string Correo { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        public string Contrasena { get; set; }
    }
}
