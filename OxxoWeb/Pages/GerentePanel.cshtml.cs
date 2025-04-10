using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OxxoWeb.Models;
using System.Collections.Generic;

namespace GerenteTareas.Pages
{
    public class GerentePanelModel : PageModel
    {
        // ==========================================
        // Instancia del contexto personalizado para acceder a la base de datos
        // ==========================================
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
        // MÉTODO PARA CARGAR TODOS LOS DATOS DEL PANEL
        // Reutilizado en OnGet() y OnPost() para evitar duplicación
        // ==========================================
        private void CargarDatosPanel()
        {
            TotalAsesores = _db.GetTotalAsesores();
            MetasActivas = _db.GetMetasActivas();
            PlazasRegistradas = _db.GetPlazasRegistradas();
            TiendasRegistradas = _db.GetTiendasRegistradas();
            TareasAsignadas = _db.GetTareasTotales();
            TareasProximas = _db.GetTareasProximasAVencer();

            ProgresoAsesores = _db.GetProgresoAsesores();
            Asesores = _db.GetAsesores();

            CapacitacionesFinalizadas = _db.GetCapacitacionesFinalizadas();
            TotalEXP = _db.GetTotalEXP();
            TotalCertificados = _db.GetTotalCertificados();
            PublicacionesRecientes = _db.GetPublicacionesRecientes();

            LogroCapacitacionPorTodos = _db.Logro_CapacitacionPorTodos();
            LogroEXP5000 = _db.Logro_EXP5000();
            LogroCincoCertificados = _db.Logro_CincoCertificados();
            LogroCincoPublicaciones = _db.Logro_CincoPublicacionesRecientes();
            LogroAsesorCincoMetas = _db.Logro_AsesorCincoMetas();
            LogroTodosTiposCompletados = _db.Logro_TodosTiposCompletados();

            RankingAsesores = _db.ObtenerTopAsesoresPorMetas();
        }

        // ==========================================
        // MÉTODO QUE SE EJECUTA AL CARGAR LA PÁGINA
        // ==========================================
        public void OnGet()
        {
            // Probar conexión y mostrar resultado en consola
            bool conectado = _db.ProbarConexion();
            Console.WriteLine("¿Conexión exitosa? " + conectado);

            // Cargar todos los datos del panel
            CargarDatosPanel();
        }

        // ==========================================
        // MÉTODO PARA INSERTAR UNA NUEVA TAREA
        // ==========================================
        public IActionResult OnPost()
        {
            // Validación del modelo del lado del servidor
            if (!ModelState.IsValid)
            {
                TempData["Mensaje"] = "Por favor completa todos los campos correctamente.";

                // Recargar los datos para evitar que se borre el contenido
                CargarDatosPanel();

                return Page(); // vuelve a mostrar la página con errores
            }

            // Insertar nueva tarea a la base de datos
            _db.InsertarTarea(NuevaTarea);

            // Limpiar el formulario
            ModelState.Clear();

            // Mostrar mensaje de éxito
            TempData["Mensaje"] = "Tarea asignada correctamente.";

            // Redirigir para evitar reenvío de formulario y refrescar datos
            return RedirectToPage();
        }
    }
}
