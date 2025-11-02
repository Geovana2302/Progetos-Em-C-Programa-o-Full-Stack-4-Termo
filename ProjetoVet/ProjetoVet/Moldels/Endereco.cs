using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjetoVet.Models
{
    public class Endereco
    {
        [Key]
        public int EnderecoID { get; set; }

        public string Logradouro { get; set; } // O diagrama dizia "Loucadoura"
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string CEP { get; set; }
        public string Numero { get; set; }
        public string UF { get; set; }

        // Chave Estrangeira para o Tutor
        public int TutorID { get; set; }

        // Propriedade de Navegação (Para o EF Core entender o vínculo)
        [ForeignKey("TutorID")]
        public virtual Tutor Tutor { get; set; }
    }
}