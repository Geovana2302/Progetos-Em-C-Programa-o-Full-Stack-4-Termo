using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjetoVet.Models
{
    [Table("Funcionario")]
    public class Funcionario
    {
        [Key]
        public int FuncionarioID { get; set; }

        [ForeignKey("Pessoa")]
        public int PessoaID { get; set; }

        public string Login { get; set; }
        public string Senha { get; set; }

        // Relação 1:1 com Pessoa
        public virtual Pessoa Pessoa { get; set; }
    }
}
