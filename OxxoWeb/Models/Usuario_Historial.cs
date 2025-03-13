using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

// Conectar a modelo oxxo web
namespace OxxoWeb.Models
{
    public class Usuario_Historial
    {
        // Valores de entidad
        public int id_historial {get;set;}
        public int id_usuario {get;set;}

        // Con parámetros
        public Usuario_Historial(int id_historial_,int id_usuario_)
        {
            this.id_historial = id_historial_;
            this.id_usuario = id_usuario_;
        }

        // Constructor sin parámetros
        public Usuario_Historial() {}
    }
}