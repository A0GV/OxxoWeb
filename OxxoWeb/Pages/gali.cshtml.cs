using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OxxoWeb.Models;
using System.Collections.Generic;

namespace GerenteTareas.Pages
{
    public class GaliModel : PageModel
    {
        // ==========================================
        // Instancia del contexto personalizado para acceder a la base de datos
        // ==========================================
        private readonly GaliDatabaseContext _db;

        // ==========================================
        // Constructor que inicializa el contexto de base de datos
        // ==========================================
        public GaliModel()
        {
            _db = new GaliDatabaseContext();
        }

        // ==========================================
        // PROPIEDADES PARA LAS KPIs DEL PANEL DEL GERENTE
        // ==========================================
        public int TotalAsesores { get; set; }
        public int MetasActivas { get; set; }
        public int PlazasRegistradas { get; set; }
        public int TiendasRegistradas { get; set; }
        public int TareasAsignadas { get; set; }
        public int TareasProximas { get; set; }

        // ==========================================
        // PROPIEDAD PARA LA TABLA DE PROGRESO DE ASESORES
        // ==========================================
        public List<ProgresoAsesor> ProgresoAsesores { get; set; } = new();

        // ==========================================
        // PROPIEDAD PARA CAPTURAR LA NUEVA TAREA DEL FORMULARIO
        // ==========================================
        [BindProperty]
        public Tarea NuevaTarea { get; set; } = new Tarea(); // trae DateTime.Today por defecto

        public List<Usuario> Asesores { get; set; } = new List<Usuario>();


        // ==========================================
        // MÉTODO QUE SE EJECUTA AL CARGAR LA PÁGINA
        // ==========================================
        public void OnGet()
        {
            // Probar conexión y mostrar resultado en consola
            bool conectado = _db.ProbarConexion();
            Console.WriteLine("¿Conexión exitosa? " + conectado);

            // Obtener valores para las tarjetas KPI
            TotalAsesores      = _db.GetTotalAsesores();
            MetasActivas       = _db.GetMetasActivas();
            PlazasRegistradas  = _db.GetPlazasRegistradas();
            TiendasRegistradas = _db.GetTiendasRegistradas();
            TareasAsignadas    = _db.GetTareasTotales();
            TareasProximas     = _db.GetTareasProximasAVencer();

            // Obtener datos reales para la tabla "Progreso de Asesores"
            ProgresoAsesores   = _db.GetProgresoAsesores();

            Asesores = _db.GetAsesores(); // este método debería consultar a la tabla `usuario`

        }

        // ==========================================
        // MÉTODO PARA INSERTAR UNA NUEVA TAREA
        // ==========================================
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid || NuevaTarea.FechaLimite == DateTime.MinValue || NuevaTarea.IdUsuario == 0)
            {
                TempData["Mensaje"] = "Por favor completa todos los campos correctamente.";
                Asesores = _db.GetAsesores(); // para volver a llenar el select
                return Page();
            }


            _db.InsertarTarea(NuevaTarea);
            ModelState.Clear(); // <-- limpia los datos actuales
            TempData["Mensaje"] = "Tarea asignada correctamente.";
            // Redirigir a GET para limpiar el formulario y recargar datos
            return RedirectToPage();
        }
    }
}
