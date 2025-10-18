using Microsoft.AspNetCore.Identity;
using System;

namespace NetIdentity.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? Genero { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public string? NombreCompleto { get; set; }
    }
}
