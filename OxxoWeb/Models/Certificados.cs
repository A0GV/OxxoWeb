// Certificados.cs
namespace OxxoWeb.Models;
public class Cerificados
{
    public int IdCertificado { get; set; }
    public int IdUsuario { get; set; }
    public string Titulo { get; set; }
    public DateTime FechaSubido { get; set; }
    public string Descripcion { get; set; }
}