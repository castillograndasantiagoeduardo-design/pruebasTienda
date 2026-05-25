namespace PruebaBackendTienda.Models.Domain
{
    /// <summary>
    /// Mapea la tabla [Aprendices]. 
    /// IMPORTANTE: El Saldo es el núcleo de la integración con SenaPay.
    /// </summary>
    public class Aprendiz
    {
        public int Id_Aprendiz { get; set; }
        public decimal Saldo { get; set; }           // ← Este es el "dinero" del ecosistema SenaPay
        public string Nombre { get; set; } = string.Empty;
        public string? Correo { get; set; }
        public string? Telefono { get; set; }
        public int Id_Usuario { get; set; }          // FK → Usuarios (tabla de autenticación)

        public ICollection<Transaccion> Transacciones { get; set; } = new List<Transaccion>();
    }

}
