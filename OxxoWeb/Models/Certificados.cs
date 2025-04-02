// Certificados.cs
namespace OxxoWeb.Models;
public class Cerificados
{
    public int IdCertificado { get; set; }
    public int IdUsuario { get; set; }
    public string Titulo { get; set; }
    public DateTime FechaPublicado { get; set; } // Cambiado
    public string Descripcion { get; set; }
}
