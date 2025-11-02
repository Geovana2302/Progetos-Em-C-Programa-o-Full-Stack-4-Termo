using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstoquePerfumes.Models
{
    // Models/Usuario.cs
    public class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string SenhaHash { get; set; } = null!;
        public string NivelAcesso { get; set; } = null!; // Ex: "Admin", "Vendedor"
    }
}

