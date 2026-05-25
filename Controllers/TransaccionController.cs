using Microsoft.AspNetCore.Mvc;
using PruebaBackendTienda.Models.ViewModels;
using PruebaBackendTienda.Services.Interfaces;
using System.Text.Json;

namespace PruebaBackendTienda.Controllers
{
    public class TransaccionController : Controller
    {
        private readonly ITransaccionService _transaccionService;
        private const string CarritoSessionKey = "Carrito_SenaPay";

        public TransaccionController(ITransaccionService transaccionService)
        {
            _transaccionService = transaccionService;
        }

        // POST /Transaccion/Procesar
        [HttpPost]
        public async Task<IActionResult> Procesar()
        {
            // ── TEMPORAL: Id del aprendiz desde sesión ────────────────────────
            // Cuando integres la autenticación de SenaPay, reemplaza esto por
            // el Id real del usuario logueado: HttpContext.Session.GetInt32("IdAprendiz")
            var idAprendiz = HttpContext.Session.GetInt32("IdAprendiz") ?? 1;

            var json = HttpContext.Session.GetString(CarritoSessionKey);
            var carrito = json == null
                ? new CarritoViewModel()
                : JsonSerializer.Deserialize<CarritoViewModel>(json) ?? new CarritoViewModel();

            var resultado = await _transaccionService.ProcesarCompraAsync(idAprendiz, carrito);

            if (resultado.Exitosa)
            {
                // Limpiar carrito tras compra exitosa
                HttpContext.Session.Remove(CarritoSessionKey);
                return RedirectToAction("Confirmacion", new { id = resultado.IdTransaccion });
            }

            TempData["Error"] = resultado.Mensaje;
            return RedirectToAction("Index", "Carrito");
        }

        // GET /Transaccion/Confirmacion/5
        public IActionResult Confirmacion(int id)
        {
            ViewBag.IdTransaccion = id;
            ViewBag.Mensaje = TempData["MensajeExito"] ?? "Compra procesada correctamente.";
            return View();
        }
    }
}
