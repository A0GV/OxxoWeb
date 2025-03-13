// Publicaciones.cs
namespace OxxoWeb.Models;
public class Publicacion
{
    public int IdPublicacion { get; set; }
    public int IdUsuario { get; set; }
    public string Titulo { get; set; }
    public DateTime FechaPublicado { get; set; }
    public string Contenido { get; set; }
}