using PruebaBackendTienda.Models.Domain;

namespace PruebaBackendTienda.Data.Repositories.Interfaces
{
    public interface IAprendizRepository
    {
        Task<Aprendiz?> ObtenerPorIdUsuarioAsync(int idUsuario);
        Task<bool> DescontarSaldoAsync(int idAprendiz, decimal monto);
        Task<decimal> ConsultarSaldoAsync(int idAprendiz);
    }
}
