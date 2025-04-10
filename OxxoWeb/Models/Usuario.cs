using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

// Conectar a modelo oxxo web
namespace OxxoWeb.Models
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public string nombre { get; set; } = ""; // para quitar CS8618 warnings de null 
        public string apellido_pat { get; set; } = ""; // para quitar CS8618 warnings de null
        public string apellido_mat { get; set; } = ""; // para quitar CS8618 warnings de null
        public string correo { get; set; } = ""; // para quitar CS8618 warnings de null

        public Usuario(string nombre_, string apellido_pat_, string apellido_mat_, string correo_)
        {
            this.nombre = nombre_;
            this.apellido_pat = apellido_pat_;
            this.apellido_mat = apellido_mat_;
            this.correo = correo_;
        }

        public Usuario() {}
    }
}