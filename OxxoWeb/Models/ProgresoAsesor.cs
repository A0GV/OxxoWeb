// Para pantalla Gali - representa el progreso de cada asesor
namespace OxxoWeb.Models
{
    public class ProgresoAsesor
    {
        public string Nombre { get; set; } = "";        // Nombre del asesor
        public string ApellidoPat { get; set; } = "";   // Apellido paterno
        public string ApellidoMat { get; set; } = "";   // Apellido materno
        public string Reto { get; set; } = "";          // Nombre del reto o tarea asignada
        public string Estado { get; set; } = "";        // Estado de la tarea (Completado, En progreso, Sin empezar)
        public DateTime FechaLimite { get; set; }

        // Propiedad opcional para mostrar el nombre completo en el frontend
        public string NombreCompleto => $"{Nombre} {ApellidoPat} {ApellidoMat}";
    }
}
