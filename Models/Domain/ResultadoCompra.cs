// Models/Domain/ResultadoCompra.cs
namespace PruebaBackendTienda.Models.Domain
{
    public class ResultadoCompra
    {
        public bool Exitosa { get; set; }
        public string Mensaje { get; set; } = string.Empty;
        public int IdTransaccion { get; set; }
        public decimal SaldoRestante { get; set; }  // ← agrega esta línea

        public static ResultadoCompra Fallo(string mensaje) =>
            new() { Exitosa = false, Mensaje = mensaje };

        public static ResultadoCompra Exito(int idTransaccion, string mensaje = "Compra procesada correctamente.") =>
            new() { Exitosa = true, IdTransaccion = idTransaccion, Mensaje = mensaje };
    }
}