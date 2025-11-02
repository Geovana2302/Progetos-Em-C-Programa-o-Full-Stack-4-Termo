using System; // <-- Precisa disso para o DateTime


namespace ProjetoVet.Models
{
    public class Funcionario : Pessoa // Deve herdar de Pessoa
    {
        // O PessoaID, Nome, etc. já vêm da classe Pessoa

        public string Login { get; set; }
        public string Senha { get; set; }
        public string Cargo { get; set; }
        public decimal Salario { get; set; }
        public DateTime DataAdmissao { get; set; }

  
    }
}