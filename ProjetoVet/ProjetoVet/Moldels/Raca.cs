using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace ProjetoVet.Models
{
    public class Raca
    {
        [Key]
        public int RacaID { get; set; } // Mudei de string para int, é mais comum
        public string Nome { get; set; }

        // Chave Estrangeira para Especie
        public int EspecieID { get; set; }

        // Navegação: A Raça pertence a UMA Espécie
        [ForeignKey("EspecieID")]
        public virtual Especie Especie { get; set; }

        // Navegação: Uma Raça tem muitos Animais
        public virtual ICollection<Animal> Animais { get; set; }
    }
}