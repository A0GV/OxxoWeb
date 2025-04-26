// Certificados.cs
namespace OxxoWeb.Models;

public class AyudaVideojuego
{
    public int IdAyuda { get; set; }
    public string Descripcion { get; set; }
    public string Creditos { get; set; }
    public string Licencias { get; set; }
    public List<Personaje> Personajes { get; set; }
    public List<Minijuego> Minijuegos { get; set; }
}

public class Personaje
{
    public int IdPersonaje { get; set; }
    public string Nombre { get; set; }
    public string Descripcion { get; set; }
    public int IdAyuda { get; set; }
}

public class Minijuego
{
    public int IdMinijuego { get; set; }
    public string Nombre { get; set; }
    public string Descripcion { get; set; }
    public int IdAyuda { get; set; }
    public List<ControlMinijuego> Controles { get; set; }
    public List<CondicionMinijuego> Condiciones { get; set; }
}

public class ControlMinijuego
{
    public int IdControl { get; set; }
    public string Control { get; set; }
    public string Descripcion { get; set; }
    public int IdMinijuego { get; set; }
}

public class CondicionMinijuego
{
    public int IdCondicion { get; set; }
    public string TipoCondicion { get; set; } // "Ganar" o "Perder"
    public string Descripcion { get; set; }
    public int IdMinijuego { get; set; }
}
