// Perfil.cs
namespace OxxoWeb.Model;
public class Perfiles
{
    public int IdUsuario { get; set; }
    public string Nombre { get; set; }
    public string ApellidoPat { get; set; }
    public string ApellidoMat { get; set; }
    public DateTime FechaNacimiento { get; set; }
    public string Correo { get; set; }
    public DateTime FechaInicio { get; set; }
    public string PlazaNombre { get; set; }
    public string Ciudad { get; set; }
    public string Estado { get; set; }
    public string TipoUsuario { get; set; }
}
