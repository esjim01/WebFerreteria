﻿using System;
using System.Collections.Generic;

namespace WebApiFerreteria.Models;

public partial class Empresa
{
    public int Id { get; set; }

    public string? Nombre { get; set; }

    public string? Nit { get; set; }

    public string? Direccion { get; set; }

    public string? Telefono { get; set; }

    public string? Correo { get; set; }
}
