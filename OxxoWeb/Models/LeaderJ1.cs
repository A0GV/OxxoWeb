using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

// Conectar a modelo oxxo web
namespace OxxoWeb.Models
{
    public class LeaderJ1
    {
        // J1 valores
        public int id_juego1 {get;set;}
        public int monedas {get;set;}
        public int exp {get;set;}
        public DateTime fecha_juego {get;set;}

        // Historial
        

        // Usuario_historial

        // Usuario

        // Constructor con parametros
        public LeaderJ1(int id_juego1_,int monedas_,int exp_,DateTime fecha_juego_)
        {
            this.id_juego1 = id_juego1_;
            this.monedas = monedas_;
            this.exp = exp_;
            this.fecha_juego = fecha_juego_;
        }

        // Constructor sin parametros
        public LeaderJ1() {}
    }
}