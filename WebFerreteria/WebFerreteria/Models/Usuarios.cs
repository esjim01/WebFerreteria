using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebFerreteria.Models;

public partial class Usuarios
{
    public int Id { get; set; }

    public string? NombreCompleto { get; set; }

    public string? Correo { get; set; }

    public string? Contrasena { get; set; }

    public string? Rol { get; set; }

    public bool? Estado { get; set; }

    public virtual ICollection<Entregas> Entregas { get; set; } = new List<Entregas>();

    public class LoginModel
    {
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "El correo electrónico es obligatorio")]
            [EmailAddress(ErrorMessage = "El correo electrónico no es válido")]
            public string Correo { get; set; }

            [Required(ErrorMessage = "La contraseña es obligatoria")]
            [DataType(DataType.Password)]
            public string Contrasena { get; set; }
        }
    }
}
