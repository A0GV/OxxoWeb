using Microsoft.AspNetCore.Mvc.RazorPages;
using OxxoWeb.Models;

namespace OxxoWeb.Pages
{
    public class JuegoModel : PageModel
    {
        public AyudaVideojuego Ayuda { get; set; }

        public void OnGet()
        {
            // Instancia tu context (ajusta el nombre si cambiaste la clase)
            DatabaseContextJuego db = new DatabaseContextJuego();
            Ayuda = db.GetAyudaVideojuego();
        }
    }
}
