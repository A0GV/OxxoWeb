using System;
using System.ComponentModel.DataAnnotations;

namespace OxxoWeb.Models
{
    public class Tarea
    {
        public int IdTarea { get; set; } = 0;

        [Required(ErrorMessage = "El título es obligatorio")]
        public string Titulo { get; set; } = "";

        [Required(ErrorMessage = "El tipo de tarea es obligatorio")]
        public string Tipo { get; set; } = "";

        [DataType(DataType.Date)] // Para formatear mejor la fecha... sin hora
        [Required(ErrorMessage = "La fecha límite es obligatoria")]
        public DateTime FechaLimite { get; set; } = DateTime.Today;

        [Range(1, int.MaxValue, ErrorMessage = "Debes seleccionar un asesor válido")]
        public int IdUsuario { get; set; } = 0; // ID del asesor asignado
    }
}
