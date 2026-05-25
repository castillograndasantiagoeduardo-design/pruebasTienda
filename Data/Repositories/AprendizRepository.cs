using Microsoft.EntityFrameworkCore;
using PruebaBackendTienda.Data.Repositories.Interfaces;
using PruebaBackendTienda.Models.Domain;

namespace PruebaBackendTienda.Data.Repositories
{
    public class AprendizRepository : IAprendizRepository
    {
        private readonly ApplicationDbContext _context;

        public AprendizRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Aprendiz?> ObtenerPorIdUsuarioAsync(int idUsuario)
        {
            return await _context.Aprendices
                .FirstOrDefaultAsync(a => a.Id_Usuario == idUsuario);
        }

        public async Task<bool> DescontarSaldoAsync(int idAprendiz, decimal monto)
        {
            var aprendiz = await _context.Aprendices.FindAsync(idAprendiz);
            if (aprendiz == null || aprendiz.Saldo < monto) return false;

            aprendiz.Saldo -= monto;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<decimal> ConsultarSaldoAsync(int idAprendiz)
        {
            var aprendiz = await _context.Aprendices.FindAsync(idAprendiz);
            return aprendiz?.Saldo ?? 0m;
        }
    }

}
