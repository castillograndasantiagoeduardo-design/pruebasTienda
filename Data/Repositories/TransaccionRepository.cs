using Microsoft.EntityFrameworkCore;
using PruebaBackendTienda.Data.Repositories.Interfaces;
using PruebaBackendTienda.Models.Domain;

namespace PruebaBackendTienda.Data.Repositories
{
    public class TransaccionRepository : ITransaccionRepository
    {
        private readonly ApplicationDbContext _context;

        public TransaccionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Transaccion> CrearTransaccionAsync(Transaccion transaccion)
        {
            _context.Transacciones.Add(transaccion);
            await _context.SaveChangesAsync();
            return transaccion;  // EF Core rellena el Id_Transaccion generado por SQL Server
        }

        public async Task<IEnumerable<Transaccion>> ObtenerHistorialAprendizAsync(int idAprendiz)
        {
            return await _context.Transacciones
                .Include(t => t.Detalles)
                    .ThenInclude(d => d.Producto)  // JOIN anidado: Transaccion → Detalles → Producto
                .Where(t => t.Id_Aprendiz == idAprendiz)
                .OrderByDescending(t => t.Fecha)
                .ToListAsync();
        }

        public async Task<Transaccion?> ObtenerConDetallesAsync(int idTransaccion)
        {
            return await _context.Transacciones
                .Include(t => t.Detalles)
                    .ThenInclude(d => d.Producto)
                .Include(t => t.Aprendiz)
                .FirstOrDefaultAsync(t => t.Id_Transaccion == idTransaccion);
        }
    }

}
