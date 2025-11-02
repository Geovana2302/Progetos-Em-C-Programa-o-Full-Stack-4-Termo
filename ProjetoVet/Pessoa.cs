using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjetoVet.Models
{
    [Table("Pessoa")]
    public class Pessoa
    {
        [Key]
        public int PessoaID { get; set; }

        public string Nome { get; set; }
        public string CPF { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
    }
}
