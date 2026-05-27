using PruebaBackendTienda.Models.Domain;

namespace PruebaBackendTienda.Data.Repositories.Interfaces
{
    /// <summary>
    /// Contrato (interfaz) que define QUÉ operaciones existen para Producto.
    /// El controlador solo conoce esta interfaz, nunca la implementación.
    /// Principio de Inversión de Dependencias (D de SOLID).
    /// </summary>
    /// 

    // ──────────────────────────────────────────────────────────────────────
    // AGREGA estos métodos a tu IProductoRepository.cs existente
    // ──────────────────────────────────────────────────────────────────────


    namespace PruebaBackendTienda.Data.Repositories.Interfaces
    {
        public interface IProductoRepository
        {
            // ── Métodos que ya tienes ──────────────────────────────────────
            Task<IEnumerable<Producto>> ObtenerTodosAsync();
            Task<Producto?> ObtenerPorIdAsync(int id);
            Task<bool> TieneStockSuficienteAsync(int idProducto, int cantidad);
            Task ReducirStockAsync(int idProducto, int cantidad);

            // ── Métodos NUEVOS para el Admin ───────────────────────────────
            Task<IEnumerable<Producto>> ObtenerTodosAdminAsync(); // incluye inactivos
            Task CrearAsync(Producto producto);
            Task ActualizarStockAsync(int idProducto, int nuevoStock);
            Task ActivarDesactivarAsync(int idProducto, bool activo);
            Task EliminarAsync(int idProducto);
        }
    }

    public interface IProductoRepository
    {
        Task<IEnumerable<Producto>> ObtenerTodosActivosAsync();
        Task<IEnumerable<Producto>> ObtenerPorTiendaAsync(int idTienda);
        Task<IEnumerable<Producto>> ObtenerPorCategoriaAsync(int idCategoria);
        Task<Producto?> ObtenerPorIdAsync(int idProducto);
        Task<bool> ReducirStockAsync(int idProducto, int cantidad);
        Task<bool> TieneStockSuficienteAsync(int idProducto, int cantidad);
    }

}
