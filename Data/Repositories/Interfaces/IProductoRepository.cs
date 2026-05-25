using PruebaBackendTienda.Models.Domain;

namespace PruebaBackendTienda.Data.Repositories.Interfaces
{
    /// <summary>
    /// Contrato (interfaz) que define QUÉ operaciones existen para Producto.
    /// El controlador solo conoce esta interfaz, nunca la implementación.
    /// Principio de Inversión de Dependencias (D de SOLID).
    /// </summary>
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
