// ─────────────────────────────────────────────────────────────────────
// ARCHIVO NUEVO: Data/Repositories/Interfaces/ICategoriaRepository.cs
// ─────────────────────────────────────────────────────────────────────
using PruebaBackendTienda.Models.Domain;

namespace PruebaBackendTienda.Data.Repositories.Interfaces
{
    public interface ICategoriaRepository
    {
        Task<IEnumerable<Categoria>> ObtenerTodasAsync();
        Task<Categoria?> ObtenerPorIdAsync(int id);
    }
}
