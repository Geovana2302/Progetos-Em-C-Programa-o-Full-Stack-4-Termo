using ProjetoVet.Data;
using ProjetoVet.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore; // Adicione este using, se necessário

namespace ProjetoVet.Repositories
{
    public class VeterinarioRepository
    {
        // 1. Adicionar (Salva na tabela Pessoas)
        public void Adicionar(Veterinario veterinario)
        {
            using (var context = new VetContext())
            {
                // CORREÇÃO: Usar a tabela-pai 'Pessoas'
                context.Pessoas.Add(veterinario);
                context.SaveChanges();
            }
        }

        // 2. Consultar (Busca na tabela Pessoas e filtra por 'Veterinario')
        public List<Veterinario> Consultar()
        {
            using (var context = new VetContext())
            {
                // CORREÇÃO: Usar 'Pessoas' e filtrar com 'OfType'
                return context.Pessoas.OfType<Veterinario>().ToList();
            }
        }

        // 3. Alterar (Atualiza na tabela Pessoas)
        public void Alterar(Veterinario veterinario)
        {
            using (var context = new VetContext())
            {
                // CORREÇÃO: Usar 'Pessoas.Update'
                context.Pessoas.Update(veterinario);
                context.SaveChanges();
            }
        }

        // 4. Excluir (Remove da tabela Pessoas)
        public void Excluir(Veterinario veterinario)
        {
            using (var context = new VetContext())
            {
                // CORREÇÃO: Usar 'Pessoas.Remove'
                context.Pessoas.Remove(veterinario);
                context.SaveChanges();
            }
        }
    }
}