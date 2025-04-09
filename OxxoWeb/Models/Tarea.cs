// para pantalla gali
using System;
using System.ComponentModel.DataAnnotations;

namespace OxxoWeb.Models
{
    public class Tarea
    {
        public int IdTarea { get; set; } = 0;
        public string Titulo { get; set; } = "";
        public string Tipo { get; set; } = "";

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "La fecha límite es obligatoria")]
        public DateTime FechaLimite { get; set; } = DateTime.Today;

        public int IdUsuario { get; set; } = 0; // NUEVO: ID del asesor asignado
    }
}
