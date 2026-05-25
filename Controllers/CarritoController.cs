using Microsoft.AspNetCore.Mvc;
using PruebaBackendTienda.Models.ViewModels;
using System.Text.Json;

namespace PruebaBackendTienda.Controllers
{
    public class CarritoController : Controller
    {
        private const string CarritoSessionKey = "Carrito_SenaPay";

        // ── Helpers de sesión ─────────────────────────────────────────────────
        private CarritoViewModel ObtenerCarrito()
        {
            var json = HttpContext.Session.GetString(CarritoSessionKey);
            return json == null
                ? new CarritoViewModel()
                : JsonSerializer.Deserialize<CarritoViewModel>(json) ?? new CarritoViewModel();
        }

        private void GuardarCarrito(CarritoViewModel carrito)
        {
            HttpContext.Session.SetString(CarritoSessionKey, JsonSerializer.Serialize(carrito));
        }

        // GET /Carrito
        public IActionResult Index()
        {
            var carrito = ObtenerCarrito();
            return View(carrito);
        }

        // POST /Carrito/Agregar
        [HttpPost]
        public IActionResult Agregar(int idProducto, string nombreProducto, decimal precio, int cantidad = 1)
        {
            var carrito = ObtenerCarrito();
            var itemExistente = carrito.Items.FirstOrDefault(i => i.Id_Producto == idProducto);

            if (itemExistente != null)
                itemExistente.Cantidad += cantidad;
            else
                carrito.Items.Add(new ItemCarrito
                {
                    Id_Producto = idProducto,
                    NombreProducto = nombreProducto,
                    PrecioUnitario = precio,
                    Cantidad = cantidad
                });

            GuardarCarrito(carrito);
            TempData["Mensaje"] = $"'{nombreProducto}' agregado al carrito.";
            return RedirectToAction("Index", "Productos");
        }

        // POST /Carrito/Eliminar
        [HttpPost]
        public IActionResult Eliminar(int idProducto)
        {
            var carrito = ObtenerCarrito();
            carrito.Items.RemoveAll(i => i.Id_Producto == idProducto);
            GuardarCarrito(carrito);
            return RedirectToAction("Index");
        }

        // POST /Carrito/Vaciar
        [HttpPost]
        public IActionResult Vaciar()
        {
            HttpContext.Session.Remove(CarritoSessionKey);
            return RedirectToAction("Index");
        }
    }
}
