using Microsoft.AspNetCore.Mvc.RazorPages;

public class GerenteModel : PageModel
{
    public List<AsesorTienda> ListaAsesores { get; set; } = new();

    public void OnGet()
    {
        ListaAsesores = new List<AsesorTienda>
        {
            new AsesorTienda { Nombre = "Luis Vega González", Correo = "luisg@oxxo.com", Tienda = "Tamaulipas", Foto = "perfil1.png" },
            new AsesorTienda { Nombre = "Rodrigo Ahumada Serrano", Correo = "rodrigo@oxxo.com", Tienda = "Sonora", Foto = "perfil2.png" },
            new AsesorTienda { Nombre = "Valentina Carrillo Godoy", Correo = "valeca@oxxo.com", Tienda = "Chihuahua", Foto = "perfil3.png" }
            // Agrega más si quieres
        };
    }

    public class AsesorTienda
    {
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public string Tienda { get; set; }
        public string Foto { get; set; }
    }
}
