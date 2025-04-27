// Para pantalla de Gali - representa el progreso de cada asesor
namespace OxxoWeb.Models
{
    public class ProgresoAsesor
    {
        public string Nombre { get; set; } = "";        
        public string ApellidoPat { get; set; } = "";   
        public string ApellidoMat { get; set; } = "";   
        public string Reto { get; set; } = "";          
        public string Estado { get; set; } = "";        // Estado de la tarea (Completado, En progreso, Sin empezar)
        public DateTime FechaLimite { get; set; }

        // Para mostrar el nombre completo en el frontend
        public string NombreCompleto => $"{Nombre} {ApellidoPat} {ApellidoMat}";
    }
}
