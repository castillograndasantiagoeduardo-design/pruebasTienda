using PruebaBackendTienda.Data.Repositories.Interfaces;
using PruebaBackendTienda.Models.Domain;
using PruebaBackendTienda.Models.ViewModels;
using PruebaBackendTienda.Services.Interfaces;

namespace PruebaBackendTienda.Services
{
    /// <summary>
    /// Servicio principal de compras. Orquesta los repositorios para
    /// ejecutar una compra de forma segura y atómica.
    /// </summary>
    public class TransaccionService : ITransaccionService
    {
        private readonly ITransaccionRepository _transaccionRepo;
        private readonly IAprendizRepository _aprendizRepo;
        private readonly IProductoRepository _productoRepo;

        public TransaccionService(
            ITransaccionRepository transaccionRepo,
            IAprendizRepository aprendizRepo,
            IProductoRepository productoRepo)
        {
            _transaccionRepo = transaccionRepo;
            _aprendizRepo = aprendizRepo;
            _productoRepo = productoRepo;
        }

        public async Task<ResultadoCompra> ProcesarCompraAsync(int idAprendiz, CarritoViewModel carrito)
        {
            // ── VALIDACIÓN 1: Carrito no vacío ────────────────────────────────
            if (carrito == null || !carrito.Items.Any())
                return new ResultadoCompra { Exitosa = false, Mensaje = "El carrito está vacío." };

            // ── VALIDACIÓN 2: Saldo suficiente ────────────────────────────────
            var saldoActual = await _aprendizRepo.ConsultarSaldoAsync(idAprendiz);
            if (saldoActual < carrito.Total)
                return new ResultadoCompra
                {
                    Exitosa = false,
                    Mensaje = $"Saldo insuficiente. Tienes ${saldoActual:N2} y el total es ${carrito.Total:N2}."
                };

            // ── VALIDACIÓN 3: Stock disponible para todos los productos ────────
            foreach (var item in carrito.Items)
            {
                var hayStock = await _productoRepo.TieneStockSuficienteAsync(item.Id_Producto, item.Cantidad);
                if (!hayStock)
                    return new ResultadoCompra
                    {
                        Exitosa = false,
                        Mensaje = $"Stock insuficiente para '{item.NombreProducto}'. Reduce la cantidad."
                    };
            }

            // ── PROCESAMIENTO: Crear la Transacción ───────────────────────────
            var transaccion = new Transaccion
            {
                Total = carrito.Total,
                Fecha = DateTime.Now,
                Id_Aprendiz = idAprendiz,
                Detalles = carrito.Items.Select(item => new DetalleTransaccion
                {
                    Id_Producto = item.Id_Producto,
                    Cantidad = item.Cantidad,
                    Precio_Unitario = item.PrecioUnitario  // Precio fijo al momento de compra
                }).ToList()
            };

            // Guardamos en BD — EF Core crea Transaccion + todos sus Detalles en un solo comando
            var transaccionCreada = await _transaccionRepo.CrearTransaccionAsync(transaccion);

            // ── INTEGRACIÓN SENAPAY: Descontar saldo del Aprendiz ─────────────
            await _aprendizRepo.DescontarSaldoAsync(idAprendiz, carrito.Total);

            // ── Reducir stock de cada producto comprado ───────────────────────
            foreach (var item in carrito.Items)
                await _productoRepo.ReducirStockAsync(item.Id_Producto, item.Cantidad);

            var saldoFinal = await _aprendizRepo.ConsultarSaldoAsync(idAprendiz);

            return new ResultadoCompra
            {
                Exitosa = true,
                Mensaje = "¡Compra realizada con éxito!",
                IdTransaccion = transaccionCreada.Id_Transaccion,
                SaldoRestante = saldoFinal
            };
        }

        public async Task<IEnumerable<Transaccion>> ObtenerHistorialAsync(int idAprendiz)
        {
            return await _transaccionRepo.ObtenerHistorialAprendizAsync(idAprendiz);
        }
    }
}
