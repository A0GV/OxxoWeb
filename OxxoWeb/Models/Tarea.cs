//para pantalla gali

namespace OxxoWeb.Models
{
    public class Tarea
    {
        public int IdTarea { get; set; }
        public string Titulo { get; set; }
        public string Tipo { get; set; }
        public DateTime FechaLimite { get; set; }
        public int IdUsuario { get; set; } // quien asignó la tarea (gerente)
    }
}
