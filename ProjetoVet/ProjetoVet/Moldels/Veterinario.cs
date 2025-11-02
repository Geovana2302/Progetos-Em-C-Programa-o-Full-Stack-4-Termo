using System;

namespace ProjetoVet.Models
{
    public class Veterinario : Pessoa
    {
        public string CRMV { get; set; }
        public string Especialidade { get; set; }
        public decimal Salario { get; set; }
        public DateTime DataAdmissao { get; set; }
        public DateTime? DataDemissao { get; set; } // '?' permite valor nulo
        public string Situacao { get; set; }
    }
}