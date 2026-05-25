using Microsoft.EntityFrameworkCore;
using PruebaBackendTienda.Data.Repositories.Interfaces;
using PruebaBackendTienda.Data.Repositories;
using PruebaBackendTienda.Data;
using PruebaBackendTienda.Services.Interfaces;
using PruebaBackendTienda.Services;


var builder = WebApplication.CreateBuilder(args);

// ═══════════════════════════════════════════════════════
//  1. BASE DE DATOS
//     Registramos ApplicationDbContext con la cadena de
//     conexión definida en appsettings.json
// ═══════════════════════════════════════════════════════
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("SenaPayConnection"),
        sqlOptions =>
        {
            // Reintentos automáticos si hay fallas de conexión transitoria
            sqlOptions.EnableRetryOnFailure(maxRetryCount: 3, maxRetryDelay: TimeSpan.FromSeconds(5), errorNumbersToAdd: null);
        }
    )
);

// ═══════════════════════════════════════════════════════
//  2. REPOSITORIOS
//     Scoped = una instancia por request HTTP (recomendado para EF Core)
// ═══════════════════════════════════════════════════════
builder.Services.AddScoped<IProductoRepository, ProductoRepository>();
builder.Services.AddScoped<IAprendizRepository, AprendizRepository>();
builder.Services.AddScoped<ITransaccionRepository, TransaccionRepository>();

// ═══════════════════════════════════════════════════════
//  3. SERVICIOS DE NEGOCIO
// ═══════════════════════════════════════════════════════
builder.Services.AddScoped<ITransaccionService, TransaccionService>();

// ═══════════════════════════════════════════════════════
//  4. SESIONES (para el carrito de compras)
// ═══════════════════════════════════════════════════════
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.Name = ".SenaPay.Cafeteria.Session";
});

// ═══════════════════════════════════════════════════════
//  5. MVC
// ═══════════════════════════════════════════════════════
builder.Services.AddControllersWithViews();

var app = builder.Build();

// ── Pipeline de Middleware ────────────────────────────────────────────────────
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();       // ← Debe ir ANTES de UseAuthorization y el routing de controladores
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Productos}/{action=Index}/{id?}");

app.Run();
