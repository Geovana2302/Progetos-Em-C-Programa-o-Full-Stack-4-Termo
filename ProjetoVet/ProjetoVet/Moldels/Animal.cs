// Em Models/Animal.cs
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjetoVet.Models
{
    public class Animal
    {
        [Key]
        public int AnimalID { get; set; }

        [Required(ErrorMessage = "O nome do animal é obrigatório")] // Sugestão 1: Validação
        [StringLength(100)]                                     // Sugestão 1: Validação
        public string Nome { get; set; }

        // Sugestão 2: Permitir data nula (caso não saiba a data exata)
        public DateTime? DataNascimento { get; set; }

        [StringLength(10)] // Definir um tamanho (ex: "Macho", "Fêmea")
        public string Sexo { get; set; }

        // Chave Estrangeira para Tutor
        public int TutorID { get; set; }

        // Chave Estrangeira para Especie
        public int EspecieID { get; set; }

        // Chave Estrangeira para Raca
        public int RacaID { get; set; }

        // --- Propriedades de Navegação ---

        [ForeignKey("TutorID")]
        public virtual Tutor Tutor { get; set; }

        [ForeignKey("EspecieID")]
        public virtual Especie Especie { get; set; }

        [ForeignKey("RacaID")]
        public virtual Raca Raca { get; set; }
    }
}