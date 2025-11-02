using ProjetoVet.Data;
using ProjetoVet.Models;
using System.Collections.Generic;
using System.Linq;

namespace ProjetoVet.Repositories
{
    // 1. Mudei de 'internal' para 'public'
    public class TutorRepository
    {
        // Métodos copiados do Funcionario/VeterinarioRepository
        // e adaptados para 'Tutor' e 'Tutores'

        public void Adicionar(Tutor tutor)
        {
            using (var context = new VetContext())
            {
                context.Tutores.Add(tutor);
                context.SaveChanges();
            }
        }

        public List<Tutor> Consultar()
        {
            using (var context = new VetContext())
            {
                return context.Tutores.ToList();
            }
        }

        public void Alterar(Tutor tutor)
        {
            using (var context = new VetContext())
            {
                context.Tutores.Update(tutor);
                context.SaveChanges();
            }
        }

        public void Excluir(Tutor tutor)
        {
            using (var context = new VetContext())
            {
                context.Tutores.Remove(tutor);
                context.SaveChanges();
            }
        }
    }
}