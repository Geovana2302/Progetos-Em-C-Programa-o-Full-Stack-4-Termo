using System;
using System.ComponentModel.DataAnnotations;

namespace ProjetoVet.Models
{
    public abstract class Pessoa // Marcar como 'abstract'
    {
        [Key]
        public int PessoaID { get; set; }
        public string Nome { get; set; }
        public string CPF { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }

        // Remova o ICollection<Funcionario> daqui
    }
}