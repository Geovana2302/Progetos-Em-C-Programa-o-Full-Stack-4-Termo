using ProjetoVet.Data;
using ProjetoVet.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace ProjetoVet.Repositories
{
    public class FuncionarioRepository
    {
        public void Adicionar(Funcionario funcionario)
        {
            using (var context = new VetContext())
            {
                // Salva na tabela PAI
                context.Pessoas.Add(funcionario);
                context.SaveChanges();
            }
        }

        public List<Funcionario> Consultar()
        {
            using (var context = new VetContext())
            {
                // Pega da tabela PAI, mas filtra APENAS
                // os que são do tipo Funcionario
                return context.Pessoas.OfType<Funcionario>().ToList();
            }
        }

        public void Alterar(Funcionario funcionario)
        {
            using (var context = new VetContext())
            {
                // O método .Update() é inteligente o suficiente
                // para lidar com a herança TPH corretamente.
                context.Pessoas.Update(funcionario);
                context.SaveChanges();
            }
        }

        public void Excluir(Funcionario funcionario)
        {
            using (var context = new VetContext())
            {
                // O .Remove() também funciona corretamente.
                context.Pessoas.Remove(funcionario);
                context.SaveChanges();
            }
        }
    }
}