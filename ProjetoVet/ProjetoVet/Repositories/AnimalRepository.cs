using Microsoft.EntityFrameworkCore;
using ProjetoVet.Data;
using ProjetoVet.Models;
using System.Collections.Generic;
using System.Linq;

namespace ProjetoVet.Repositories
{
    public class AnimalRepository
    {
        private readonly VetContext _context;

        public AnimalRepository()
        {
            _context = new VetContext();
        }

        // 1. O método chama-se 'Consultar'
        public List<Animal> Consultar()
        {
            return _context.Animais
                            .Include(a => a.Tutor)
                            .Include(a => a.Especie)
                            .Include(a => a.Raca)
                            .OrderBy(a => a.Nome)
                            .ToList();
        }

        // (Métodos GetTutores, GetEspecies, GetRacasPorEspecie...)
        public List<Tutor> GetTutores()
        {
            return _context.Pessoas.OfType<Tutor>().OrderBy(t => t.Nome).ToList();
        }
        public List<Especie> GetEspecies()
        {
            return _context.Especies.OrderBy(e => e.Nome).ToList();
        }
        public List<Raca> GetRacasPorEspecie(int especieId)
        {
            return _context.Racas
                            .Where(r => r.EspecieID == especieId)
                            .OrderBy(r => r.Nome)
                            .ToList();
        }

        // 2. O método chama-se 'Adicionar'
        public void Adicionar(Animal animal)
        {
            _context.Animais.Add(animal);
            _context.SaveChanges();
        }

        // 3. O método chama-se 'Alterar'
        public void Alterar(Animal animal)
        {
            _context.Animais.Update(animal);
            _context.SaveChanges();
        }

        // 4. O método chama-se 'Excluir'
        public void Excluir(Animal animal)
        {
            if (animal != null)
            {
                _context.Animais.Remove(animal);
                _context.SaveChanges();
            }
        }
    }
}