using PruebaBackendTienda.Models.Domain;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace PruebaBackendTienda.Data
{
    /// <summary>
    /// Contexto principal de Entity Framework Core para el módulo Cafetería.
    /// Conecta con la base de datos central de SenaPay.
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        // Constructor: recibe la configuración (cadena de conexión) por inyección de dependencias
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        // ─── Tablas de la Base de Datos ───────────────────────────────────────
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Tienda> Tiendas { get; set; }
        public DbSet<Aprendiz> Aprendices { get; set; }
        public DbSet<Transaccion> Transacciones { get; set; }
        public DbSet<DetalleTransaccion> DetallesTransaccion { get; set; }

        /// <summary>
        /// Configuración explícita del mapeo. Aquí resolvemos cualquier diferencia
        /// entre los nombres de C# y los nombres reales en SQL Server.
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ── Tabla Productos ───────────────────────────────────────────────
            modelBuilder.Entity<Producto>(entity =>
            {
                entity.ToTable("Productos");
                entity.HasKey(p => p.Id_Producto);
                entity.Property(p => p.Precio).HasColumnType("decimal(18,2)");
                entity.Property(p => p.Nombre_Producto).HasMaxLength(150).IsRequired();

                // Relación: Producto → Tienda (muchos a uno)
                entity.HasOne(p => p.Tienda)
                      .WithMany(t => t.Productos)
                      .HasForeignKey(p => p.Id_Tienda)
                      .OnDelete(DeleteBehavior.Restrict); // No borrar tienda si tiene productos

                // Relación: Producto → Categoria (muchos a uno)
                entity.HasOne(p => p.Categoria)
                      .WithMany(c => c.Productos)
                      .HasForeignKey(p => p.Id_Categoria)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // ── Tabla Detalle_Transaccion ──────────────────────────────────────
            modelBuilder.Entity<DetalleTransaccion>(entity =>
            {
                entity.ToTable("Detalle_Transaccion");  // Nombre real en SQL Server
                entity.HasKey(d => d.Id_Detalle);
                entity.Property(d => d.Precio_Unitario).HasColumnType("decimal(18,2)");

                entity.HasOne(d => d.Transaccion)
                      .WithMany(t => t.Detalles)
                      .HasForeignKey(d => d.Id_Transaccion)
                      .OnDelete(DeleteBehavior.Cascade); // Si se borra la transacción, se borran los detalles

                entity.HasOne(d => d.Producto)
                      .WithMany()
                      .HasForeignKey(d => d.Id_Producto)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // ── Tabla Transacciones ────────────────────────────────────────────
            modelBuilder.Entity<Transaccion>(entity =>
            {
                entity.ToTable("Transacciones");
                entity.HasKey(t => t.Id_Transaccion);
                entity.Property(t => t.Total).HasColumnType("decimal(18,2)");

                entity.HasOne(t => t.Aprendiz)
                      .WithMany(a => a.Transacciones)
                      .HasForeignKey(t => t.Id_Aprendiz)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // ── Tabla Aprendices ───────────────────────────────────────────────
            modelBuilder.Entity<Aprendiz>(entity =>
            {
                entity.ToTable("Aprendices");
                entity.HasKey(a => a.Id_Aprendiz);
                entity.Property(a => a.Saldo).HasColumnType("decimal(18,2)");
                entity.Property(a => a.Nombre).HasMaxLength(200).IsRequired();
            });

            // ── Tabla Tienda ───────────────────────────────────────────────────
            modelBuilder.Entity<Tienda>(entity =>
            {
                entity.ToTable("Tienda");
                entity.HasKey(t => t.Id_Tienda);
            });

            // ── Tabla Categoria ────────────────────────────────────────────────
            modelBuilder.Entity<Categoria>(entity =>
            {
                entity.ToTable("Categoria");
                entity.HasKey(c => c.Id_Categoria);
            });
        }
    }

}
