using Microsoft.EntityFrameworkCore;
using PruebaBackendTienda.Data.Repositories.Interfaces;
using PruebaBackendTienda.Models.Domain;

namespace PruebaBackendTienda.Data.Repositories
{
    /// <summary>
    /// Implementación concreta del repositorio de productos.
    /// Aquí está el único lugar donde escribimos LINQ / EF Core para Productos.
    /// </summary>
    public class ProductoRepository : IProductoRepository
    {
        private readonly ApplicationDbContext _context;

        // El DbContext se inyecta automáticamente por el contenedor de DI
        public ProductoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Producto>> ObtenerTodosActivosAsync()
        {
            return await _context.Productos
                .Include(p => p.Categoria)    // Trae también la categoría (JOIN)
                .Include(p => p.Tienda)       // Trae también la tienda (JOIN)
                .Where(p => p.Estado == true && p.Stock > 0)
                .OrderBy(p => p.Nombre_Producto)
                .ToListAsync();
        }

        public async Task<IEnumerable<Producto>> ObtenerPorTiendaAsync(int idTienda)
        {
            return await _context.Productos
                .Include(p => p.Categoria)
                .Where(p => p.Id_Tienda == idTienda && p.Estado == true)
                .ToListAsync();
        }

        public async Task<IEnumerable<Producto>> ObtenerPorCategoriaAsync(int idCategoria)
        {
            return await _context.Productos
                .Include(p => p.Tienda)
                .Where(p => p.Id_Categoria == idCategoria && p.Estado == true)
                .ToListAsync();
        }

        public async Task<Producto?> ObtenerPorIdAsync(int idProducto)
        {
            return await _context.Productos
                .Include(p => p.Categoria)
                .Include(p => p.Tienda)
                .FirstOrDefaultAsync(p => p.Id_Producto == idProducto);
        }

        public async Task<bool> ReducirStockAsync(int idProducto, int cantidad)
        {
            var producto = await _context.Productos.FindAsync(idProducto);
            if (producto == null || producto.Stock < cantidad) return false;

            producto.Stock -= cantidad;

            // Si el stock llega a 0, desactivamos el producto automáticamente
            if (producto.Stock == 0)
                producto.Estado = false;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> TieneStockSuficienteAsync(int idProducto, int cantidad)
        {
            var producto = await _context.Productos.FindAsync(idProducto);
            return producto != null && producto.Stock >= cantidad && producto.Estado;
        }
    }
}


