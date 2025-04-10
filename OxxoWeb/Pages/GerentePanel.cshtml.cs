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
        // ==============================================================
        private readonly GaliDatabaseContext _db;

        // ==========================================
        // Constructor que inicializa el contexto de base de datos
        // ==========================================
        public GerentePanelModel()
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
        // MÉTRICAS PARA LAS BARRAS DE PROGRESO
        // ==========================================
        public int CapacitacionesFinalizadas { get; set; }
        public int TotalEXP { get; set; }
        public int TotalCertificados { get; set; }
        public int PublicacionesRecientes { get; set; }

        // ==========================================
        // PROPIEDADES PARA LA TARJETA DE LOGROS - EQUIPO
        // ==========================================
        public bool LogroCapacitacionPorTodos { get; set; }
        public bool LogroEXP5000 { get; set; }
        public bool LogroCincoCertificados { get; set; }
        public bool LogroCincoPublicaciones { get; set; }
        public bool LogroAsesorCincoMetas { get; set; }
        public bool LogroTodosTiposCompletados { get; set; }

        // ==========================================
        // PROPIEDADES PARA LA TARJETA DE LOGROS - INDIVIDUAL
        // ==========================================
        public List<(string NombreCompleto, int Total)> RankingAsesores { get; set; } = new();

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
            TotalAsesores = _db.GetTotalAsesores();
            MetasActivas = _db.GetMetasActivas();
            PlazasRegistradas = _db.GetPlazasRegistradas();
            TiendasRegistradas = _db.GetTiendasRegistradas();
            TareasAsignadas = _db.GetTareasTotales();
            TareasProximas = _db.GetTareasProximasAVencer();

            // Obtener datos reales para la tabla "Progreso de Asesores"
            ProgresoAsesores = _db.GetProgresoAsesores();
            
            // Obtener asesores para el select del formulario
            Asesores = _db.GetAsesores();

            // Obtener métricas para las barras de progreso
            CapacitacionesFinalizadas = _db.GetCapacitacionesFinalizadas();
            TotalEXP = _db.GetTotalEXP();
            TotalCertificados = _db.GetTotalCertificados();
            PublicacionesRecientes = _db.GetPublicacionesRecientes();

            // Obtener logros del equipo (true/false)
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
