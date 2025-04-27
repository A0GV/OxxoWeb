using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OxxoWeb.Models;
using System.Collections.Generic;

namespace GerenteTareas.Pages
{
    public class GerentePanelModel : PageModel
    {
        // ==========================================
        // Instancia a mi database context para acceder a la base de datos
        // ===========================================
        private readonly GaliDatabaseContext _db;

        // ==========================================
        // Constructor que inicializa el contexto de base de datos
        // ==========================================
        public GerentePanelModel()
        {
            _db = new GaliDatabaseContext();
        }

        // ==========================================
        // Propiedades para las KPIs 
        // ==========================================
        public int TotalAsesores { get; set; }
        public int MetasActivas { get; set; }
        public int PlazasRegistradas { get; set; }
        public int TiendasRegistradas { get; set; }
        public int TareasAsignadas { get; set; }
        public int TareasProximas { get; set; }

        // ==========================================
        // Propiedades para las progress bars
        // ==========================================
        public int CapacitacionesFinalizadas { get; set; }
        public int TotalEXP { get; set; }
        public int TotalCertificados { get; set; }
        public int PublicacionesRecientes { get; set; }

        // ==========================================
        // Propiedades para logros en equipo
        // ==========================================
        public bool LogroCapacitacionPorTodos { get; set; }
        public bool LogroEXP5000 { get; set; }
        public bool LogroCincoCertificados { get; set; }
        public bool LogroCincoPublicaciones { get; set; }
        public bool LogroAsesorCincoMetas { get; set; }
        public bool LogroTodosTiposCompletados { get; set; }

        // ==========================================
        // Propiedad para logros individuales - Ranking
        // ==========================================
        public List<(string NombreCompleto, int Total)> RankingAsesores { get; set; } = new();

        // ==========================================
        // Propiedad para la tabla de progreso
        // ==========================================
        public List<ProgresoAsesor> ProgresoAsesores { get; set; } = new();

        // ==========================================
        // Propiedad para capturar una nueva tarea para el formulario
        // ==========================================
        [BindProperty]
        public Tarea NuevaTarea { get; set; } = new Tarea(); // trae DateTime.Today por defecto

        public List<Usuario> Asesores { get; set; } = new List<Usuario>();

        // ==========================================
        // Método que se ejecuta al cargar la página
        // ==========================================
         public void OnGet()
        {
            // Mensaje en la terminal 
            bool conectado = _db.ProbarConexion();
            Console.WriteLine("¿Conexión exitosa? " + conectado);

            // Obtiene los valores para los KPIs
            TotalAsesores = _db.GetTotalAsesores();
            MetasActivas = _db.GetMetasActivas();
            PlazasRegistradas = _db.GetPlazasRegistradas();
            TiendasRegistradas = _db.GetTiendasRegistradas();
            TareasAsignadas = _db.GetTareasTotales();
            TareasProximas = _db.GetTareasProximasAVencer();

            // Obtener datos para la tabla "Progreso de Asesores"
            ProgresoAsesores = _db.GetProgresoAsesores();
            
            // Obtener asesores para el select del formulario
            Asesores = _db.GetAsesores();

            // Obtener métricas para las barras de progreso
            CapacitacionesFinalizadas = _db.GetCapacitacionesFinalizadas();
            TotalEXP = _db.GetTotalEXP();
            TotalCertificados = _db.GetTotalCertificados();
            PublicacionesRecientes = _db.GetPublicacionesRecientes();

            // Obtener logros del equipo
            LogroCapacitacionPorTodos = _db.Logro_CapacitacionPorTodos();
            LogroEXP5000 = _db.Logro_EXP5000();
            LogroCincoCertificados = _db.Logro_CincoCertificados();
            LogroCincoPublicaciones = _db.Logro_CincoPublicacionesRecientes();
            LogroAsesorCincoMetas = _db.Logro_AsesorCincoMetas();
            LogroTodosTiposCompletados = _db.Logro_TodosTiposCompletados();

           // Obtener ranking de asesores destacados
           RankingAsesores = _db.ObtenerTopAsesoresPorMetas();
        }
       
        // ==========================================
        // Método para insertar una nueva tarea
        // ==========================================
        public IActionResult OnPost()
         {
             if (!ModelState.IsValid || NuevaTarea.FechaLimite == DateTime.MinValue || NuevaTarea.IdUsuario == 0)
             {
                 TempData["Mensaje"] = "Por favor completa todos los campos correctamente.";
               
                TotalAsesores      = _db.GetTotalAsesores();
                MetasActivas       = _db.GetMetasActivas();
                PlazasRegistradas  = _db.GetPlazasRegistradas();
                TiendasRegistradas = _db.GetTiendasRegistradas();
                TareasAsignadas    = _db.GetTareasTotales();
                TareasProximas     = _db.GetTareasProximasAVencer();
                ProgresoAsesores   = _db.GetProgresoAsesores();
                Asesores           = _db.GetAsesores();

                CapacitacionesFinalizadas = _db.GetCapacitacionesFinalizadas();
                TotalEXP                  = _db.GetTotalEXP();
                TotalCertificados         = _db.GetTotalCertificados();
                PublicacionesRecientes   = _db.GetPublicacionesRecientes();

                LogroCapacitacionPorTodos  = _db.Logro_CapacitacionPorTodos();
                LogroEXP5000               = _db.Logro_EXP5000();
                LogroCincoCertificados     = _db.Logro_CincoCertificados();
                LogroCincoPublicaciones    = _db.Logro_CincoPublicacionesRecientes();
                LogroAsesorCincoMetas      = _db.Logro_AsesorCincoMetas();
                LogroTodosTiposCompletados = _db.Logro_TodosTiposCompletados();
                RankingAsesores            = _db.ObtenerTopAsesoresPorMetas();

                return Page(); 
                 
             }
 
             _db.InsertarTarea(NuevaTarea);
             ModelState.Clear(); 
             TempData["Mensaje"] = "Tarea asignada correctamente.";
 
             // Redirigir a GET para limpiar el formulario y recargar datos
             return RedirectToPage();
        }
    }
}
