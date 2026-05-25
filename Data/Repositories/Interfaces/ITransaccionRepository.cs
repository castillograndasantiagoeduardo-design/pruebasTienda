using PruebaBackendTienda.Models.Domain;

namespace PruebaBackendTienda.Data.Repositories.Interfaces
{
    public interface ITransaccionRepository
    {
        Task<Transaccion> CrearTransaccionAsync(Transaccion transaccion);
        Task<IEnumerable<Transaccion>> ObtenerHistorialAprendizAsync(int idAprendiz);
        Task<Transaccion?> ObtenerConDetallesAsync(int idTransaccion);
    }

}
