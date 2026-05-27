using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PruebaBackendTienda.Data.Repositories.Interfaces;
using PruebaBackendTienda.Models.Domain;
using PruebaBackendTienda.Models.ViewModels;

namespace PruebaBackendTienda.Controllers
{
    // ══════════════════════════════════════════════════════════════════
    // SEGURIDAD — cuando integres autenticación, descomenta esta línea:
    // [Authorize(Roles = "3")]
    // O si usas sesiones, el filtro va en cada action así:
    // if (HttpContext.Session.GetInt32("RolId") != 3) return RedirectToAction("Index", "Home");
    // ══════════════════════════════════════════════════════════════════
    public class AdminTiendaController : Controller
    {
        private readonly IProductoRepository _productoRepo;
        private readonly ICategoriaRepository _categoriaRepo; // agrega esta interfaz si no la tienes

        public AdminTiendaController(
            IProductoRepository productoRepo,
            ICategoriaRepository categoriaRepo)
        {
            _productoRepo = productoRepo;
            _categoriaRepo = categoriaRepo;
        }

        // ── GET /AdminTienda — Panel principal con lista de productos ──
        public async Task<IActionResult> Index()
        {
            // FILTRO DE ROL — descomenta cuando tengas sesiones:
            // if (HttpContext.Session.GetInt32("RolId") != 3)
            //     return RedirectToAction("Index", "Home");

            var productos = await _productoRepo.ObtenerTodosAdminAsync();

            var vm = productos.Select(p => new AdminProductoViewModel
            {
                Id_Producto = p.Id_Producto,
                Nombre = p.Nombre,
                Precio = p.Precio,
                Stock = p.Stock,
                NombreCategoria = p.Categoria?.Nombre,
                Descripcion = p.Descripcion,
                ImagenUrl = p.ImagenUrl,
                Activo = p.Activo
            }).ToList();

            return View(vm);
        }

        // ── GET /AdminTienda/Crear ─────────────────────────────────────
        public async Task<IActionResult> Crear()
        {
            await CargarCategorias();
            return View(new CrearProductoViewModel());
        }

        // ── POST /AdminTienda/Crear ────────────────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(CrearProductoViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                await CargarCategorias();
                return View(vm);
            }

            string? rutaImagen = null;
            if (vm.Imagen != null && vm.Imagen.Length > 0)
            {
                var carpeta = Path.Combine("wwwroot", "images", "productos");
                Directory.CreateDirectory(carpeta);
                var archivo = $"{Guid.NewGuid()}{Path.GetExtension(vm.Imagen.FileName)}";
                using var stream = new FileStream(Path.Combine(carpeta, archivo), FileMode.Create);
                await vm.Imagen.CopyToAsync(stream);
                rutaImagen = $"/images/productos/{archivo}";
            }

            var producto = new Producto
            {
                Nombre = vm.Nombre,
                Precio = vm.Precio,
                Stock = vm.Stock,
                Id_Categoria = vm.Id_Categoria,
                Descripcion = vm.Descripcion,
                ImagenUrl = rutaImagen,
                Activo = true
            };

            await _productoRepo.CrearAsync(producto);
            TempData["Exito"] = $"Producto '{vm.Nombre}' creado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        // ── GET /AdminTienda/EditarStock/5 ────────────────────────────
        public async Task<IActionResult> EditarStock(int id)
        {
            var producto = await _productoRepo.ObtenerPorIdAsync(id);
            if (producto == null) return NotFound();

            var vm = new EditarStockViewModel
            {
                Id_Producto = producto.Id_Producto,
                NombreProducto = producto.Nombre,
                StockActual = producto.Stock
            };
            return View(vm);
        }

        // ── POST /AdminTienda/EditarStock ─────────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarStock(EditarStockViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var producto = await _productoRepo.ObtenerPorIdAsync(vm.Id_Producto);
            if (producto == null) return NotFound();

            int nuevoStock = vm.Operacion switch
            {
                "Sumar" => producto.Stock + vm.Cantidad,
                "Restar" => Math.Max(0, producto.Stock - vm.Cantidad),
                "Fijar" => vm.Cantidad,
                _ => producto.Stock
            };

            await _productoRepo.ActualizarStockAsync(vm.Id_Producto, nuevoStock);
            TempData["Exito"] = $"Stock de '{producto.Nombre}' actualizado a {nuevoStock} unidades.";
            return RedirectToAction(nameof(Index));
        }

        // ── POST /AdminTienda/ToggleActivo/5 ─────────────────────────
        // Activa o desactiva el producto (soft delete — recomendado)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleActivo(int id, bool activo)
        {
            var producto = await _productoRepo.ObtenerPorIdAsync(id);
            if (producto == null) return NotFound();

            await _productoRepo.ActivarDesactivarAsync(id, activo);
            var estado = activo ? "activado" : "desactivado";
            TempData["Exito"] = $"Producto '{producto.Nombre}' {estado} correctamente.";
            return RedirectToAction(nameof(Index));
        }

        // ── POST /AdminTienda/Eliminar/5 ──────────────────────────────
        // Eliminar permanente — solo si no tiene transacciones relacionadas
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Eliminar(int id)
        {
            var producto = await _productoRepo.ObtenerPorIdAsync(id);
            if (producto == null) return NotFound();

            await _productoRepo.EliminarAsync(id);
            TempData["Exito"] = $"Producto '{producto.Nombre}' eliminado.";
            return RedirectToAction(nameof(Index));
        }

        // ── Helper privado ─────────────────────────────────────────────
        private async Task CargarCategorias()
        {
            var categorias = await _categoriaRepo.ObtenerTodasAsync();
            ViewBag.Categorias = new SelectList(categorias, "Id_Categoria", "Nombre");
        }
    }
}
