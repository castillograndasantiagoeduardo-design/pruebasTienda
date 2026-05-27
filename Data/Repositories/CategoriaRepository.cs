// ─────────────────────────────────────────────────────────────────────
// ARCHIVO NUEVO: Data/Repositories/CategoriaRepository.cs
// ─────────────────────────────────────────────────────────────────────
using Microsoft.EntityFrameworkCore;
using PruebaBackendTienda.Data.Repositories.Interfaces;
using PruebaBackendTienda.Models.Domain;

namespace PruebaBackendTienda.Data.Repositories
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoriaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Categoria>> ObtenerTodasAsync()
            => await _context.Categorias.OrderBy(c => c.Nombre).ToListAsync();

        public async Task<Categoria?> ObtenerPorIdAsync(int id)
            => await _context.Categorias.FindAsync(id);
    }
}
