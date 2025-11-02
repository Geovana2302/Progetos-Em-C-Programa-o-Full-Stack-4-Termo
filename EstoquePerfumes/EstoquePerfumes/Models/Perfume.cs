using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstoquePerfumes.Models
{
    // Models/Perfume.cs
    public class Perfume
    {
        public int Id { get; set; }
        public string Nome { get; set; } = null!;
        public decimal Valor { get; set; }
        public string Genero { get; set; } = null!; // "Masculino", "Feminino", "Unissex"
        public int Ml { get; set; } // Quantidade em mililitros
    }
}
