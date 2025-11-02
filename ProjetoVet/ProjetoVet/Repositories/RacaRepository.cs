using Microsoft.EntityFrameworkCore; // Necessário para o .Include()
using ProjetoVet.Data;
using ProjetoVet.Models;
using System.Collections.Generic;
using System.Linq;

namespace ProjetoVet.Repositories
{
    public class RacaRepository
    {
        private readonly VetContext _context;

        public RacaRepository()
        {
            _context = new VetContext();
        }

        /// <summary>
        /// Busca todas as Raças, incluindo a Espécie relacionada.
        /// </summary>
        public List<Raca> GetAll(string pesquisa = "")
        {
            var query = _context.Racas
                                .Include(r => r.Especie) // Inclui o objeto Especie
                                .OrderBy(r => r.Nome);

            if (string.IsNullOrWhiteSpace(pesquisa))
            {
                return query.ToList();
            }
            else
            {
                string lowerPesquisa = pesquisa.ToLower();
                return query.Where(r => r.Nome.ToLower().Contains(lowerPesquisa) ||
                                        r.Especie.Nome.ToLower().Contains(lowerPesquisa))
                            .ToList();
            }
        }

        /// <summary>
        /// Busca a lista de Espécies (para o ComboBox).
        /// </summary>
        public List<Especie> GetEspecies()
        {
            // Reutiliza o contexto, não precisa de outro repositório
            return _context.Especies.OrderBy(e => e.Nome).ToList();
        }

        public void Add(Raca raca)
        {
            _context.Racas.Add(raca);
            _context.SaveChanges();
        }

        public void Update(Raca raca)
        {
            _context.Racas.Update(raca);
            _context.SaveChanges();
        }

        public void Delete(int racaId)
        {
            // Verifica se a raça está sendo usada por um animal
            bool temAnimal = _context.Animais.Any(a => a.RacaID == racaId);

            if (temAnimal)
            {
                throw new System.Exception("Não é possível excluir esta raça, pois ela já está sendo utilizada por Animais.");
            }

            var raca = _context.Racas.Find(racaId);
            if (raca != null)
            {
                _context.Racas.Remove(raca);
                _context.SaveChanges();
            }
        }
    }
}