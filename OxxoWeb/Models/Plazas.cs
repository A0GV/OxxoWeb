using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

// Conectar a modelo oxxo web
namespace OxxoWeb.Models
{
    public class Plazas
    {
        public int id_plaza { get; set; } = 0; // para quitar CS8618 warnings de null
        public string nombre { get; set; } = ""; // para quitar CS8618 warnings de null
        public string ciudad { get; set; } = ""; // para quitar CS8618 warnings de null
        public string estado { get; set; } = ""; // para quitar CS8618 warnings de null

        // Constructor con parámetros
        public Plazas(int id_plaza_, string nombre_, string ciudad_, string estado_)
        {
            this.id_plaza = id_plaza_;
            this.nombre = nombre_;
            this.ciudad = ciudad_;
            this.estado = estado_;
        }

        // Constructor vacío
        public Plazas() { }
    }
}
