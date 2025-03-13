using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

// Conectar a modelo oxxo web
namespace OxxoWeb.Models
{
    public class Juego2
    {
        // Valores de entidad
        public int id_juego2 {get;set;}
        public int monedas {get;set;}
        public int exp {get;set;}
        public DateTime fecha_juego {get;set;}

        // Constructor con parametros
        public Juego2(int id_juego2_,int monedas_,int exp_,DateTime fecha_juego_)
        {
            this.id_juego2 = id_juego2_;
            this.monedas = monedas_;
            this.exp = exp_;
            this.fecha_juego = fecha_juego_;
        }

        // Constructor sin parametros
        public Juego2() {}
    }
}