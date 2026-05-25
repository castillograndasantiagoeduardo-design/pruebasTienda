using Microsoft.AspNetCore.Mvc;
using PruebaBackendTienda.Data.Repositories.Interfaces;

namespace PruebaBackendTienda.Controllers
{
    public class ProductosController : Controller
    {
        private readonly IProductoRepository _productoRepo;

        public ProductosController(IProductoRepository productoRepo)
        {
            _productoRepo = productoRepo;
        }

        // GET /Productos
        public async Task<IActionResult> Index(int? categoriaId, int? tiendaId)
        {
            var productos = categoriaId.HasValue
                ? await _productoRepo.ObtenerPorCategoriaAsync(categoriaId.Value)
                : tiendaId.HasValue
                    ? await _productoRepo.ObtenerPorTiendaAsync(tiendaId.Value)
                    : await _productoRepo.ObtenerTodosActivosAsync();

            return View(productos);
        }

        // GET /Productos/Detalle/5
        public async Task<IActionResult> Detalle(int id)
        {
            var producto = await _productoRepo.ObtenerPorIdAsync(id);
            if (producto == null) return NotFound();

            return View(producto);
        }
    }
}

