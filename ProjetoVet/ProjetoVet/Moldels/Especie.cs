using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ProjetoVet.Models
{
    public class Especie
    {
        [Key]
        public int EspecieID { get; set; }
        public string Nome { get; set; }

        // Navegação: Uma Espécie tem muitas Raças
        public virtual ICollection<Raca> Racas { get; set; }

        // Navegação: Uma Espécie tem muitos Animais
        public virtual ICollection<Animal> Animais { get; set; }
    }
}