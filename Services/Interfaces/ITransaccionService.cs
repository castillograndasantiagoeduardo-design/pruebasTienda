// Services/Interfaces/ITransaccionService.cs
using PruebaBackendTienda.Models.Domain;
using PruebaBackendTienda.Models.ViewModels;

namespace PruebaBackendTienda.Services.Interfaces
{
    public interface ITransaccionService
    {
        Task<ResultadoCompra> ProcesarCompraAsync(int idAprendiz, CarritoViewModel carrito);
    }
}