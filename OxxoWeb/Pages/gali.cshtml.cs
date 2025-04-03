using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GerenteTareas.Pages
{
    public class GaliModel : PageModel
    {
        // Aquí puedes definir propiedades para pasar datos a la vista
        public int TotalAsesores { get; set; } = 5;
        public int MetasActivas { get; set; } = 5;
        public int MetasCompletadas { get; set; } = 5;

        public List<AsesorProgreso> ProgresoAsesores { get; set; } = new();

        [BindProperty]
        public NuevaTareaModel NuevaTarea { get; set; }

        public void OnGet()
        {
            // Simulación de datos (puedes conectar con una BD más adelante)
            ProgresoAsesores = new List<AsesorProgreso>
            {
                new() { Nombre = "Rodrigo Ahumada", Reto = "Capacitación", Estado = "Completado" },
                new() { Nombre = "Valentina Carrillo", Reto = "EXP mínimo", Estado = "En progreso" },
                new() { Nombre = "Carolina García", Reto = "Capacitación", Estado = "Sin empezar" },
                new() { Nombre = "Luis Vega", Reto = "Capacitación", Estado = "Sin empezar" },
                new() { Nombre = "Korinna Ramírez", Reto = "Capacitación", Estado = "Completado" },
                new() { Nombre = "Valentina Carrillo", Reto = "Capacitación", Estado = "En progreso" }
            };
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Aquí podrías guardar la tarea en una base de datos o lista
            // Ejemplo: Guardar en base de datos (lógica pendiente)

            TempData["Mensaje"] = "Tarea asignada correctamente.";
            return RedirectToPage("Gali");
        }

        public class AsesorProgreso
        {
            public string Nombre { get; set; }
            public string Reto { get; set; }
            public string Estado { get; set; }
        }

        public class NuevaTareaModel
        {
            public string Titulo { get; set; }
            public string Tipo { get; set; }
            public DateTime FechaLimite { get; set; }
            public List<string> AsesoresSeleccionados { get; set; }
        }
    }
}
