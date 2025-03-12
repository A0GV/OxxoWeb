using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

// Conectar a modelo oxxo web
namespace OxxoWeb.Models
{
    public class Historial
    {
        // Valores de entidad
        public int id_historial {get;set;}
        public int id_juego1 {get;set;}
        public int id_juego2 {get;set;}
        public int id_juego3 {get;set;}

        // Con parámetros
        public Historial(int id_historial_,int id_juego1_,int id_juego2_,int id_juego3_)
        {
            this.id_historial = id_historial_;
            this.id_juego1 = id_juego1_;
            this.id_juego2 = id_juego2_;
            this.id_juego3 = id_juego3_;
        }

        // Constructor sin parametros
        public Historial() {}
    }
}