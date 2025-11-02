using ProjetoVet.Data;
using ProjetoVet.Models;
using System.Collections.Generic;
using System.Linq;

namespace ProjetoVet.Repositories
{
    public class EspecieRepository
    {
        private readonly VetContext _context;

        public EspecieRepository()
        {
            _context = new VetContext();
        }

        public List<Especie> GetAll(string pesquisa = "")
        {
            if (string.IsNullOrWhiteSpace(pesquisa))
            {
                return _context.Especies.OrderBy(e => e.Nome).ToList();
            }
            else
            {
                return _context.Especies
                               .Where(e => e.Nome.ToLower().Contains(pesquisa.ToLower()))
                               .OrderBy(e => e.Nome)
                               .ToList();
            }
        }

        public void Add(Especie especie)
        {
            _context.Especies.Add(especie);
            _context.SaveChanges();
        }

        public void Update(Especie especie)
        {
            _context.Especies.Update(especie);
            _context.SaveChanges();
        }

        public void Delete(int especieId)
        {
            // Precisamos verificar se a espécie não está sendo usada por Raças ou Animais
            // (O banco de dados já deve bloquear isso se o OnDelete é Restrict, 
            // mas é bom verificar antes)

            bool temRaca = _context.Racas.Any(r => r.EspecieID == especieId);
            bool temAnimal = _context.Animais.Any(a => a.EspecieID == especieId);

            if (temRaca || temAnimal)
            {
                // Joga uma exceção que será capturada na View
                throw new System.Exception("Não é possível excluir esta espécie, pois ela já está sendo utilizada por Raças ou Animais.");
            }

            var especie = _context.Especies.Find(especieId);
            if (especie != null)
            {
                _context.Especies.Remove(especie);
                _context.SaveChanges();
            }
        }
    }
}