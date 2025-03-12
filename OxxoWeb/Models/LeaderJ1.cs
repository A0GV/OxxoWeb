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
        public int exp {get;set;}
        public DateTime fecha_juego {get;set;}

        // Historial
        public int id_historial {get;set;}

        // Usuario_Historial
        public int id_usuario {get;set;}

        // Usuario
        public int id_tipo {get;set;}
        public int id_plaza {get;set;}
        public string nombre {get;set;}
        public string apellido_pat {get;set;}
        public string apellido_mat {get;set;}


        // Constructor con parametros
        public LeaderJ1(int id_juego1_,int exp_,DateTime fecha_juego_, 
        int id_historial_,
        int id_usuario_, 
        int id_tipo_, int id_plaza_, string nombre_,string apellido_pat_,string apellido_mat_)
        {
            // Juego
            this.id_juego1 = id_juego1_;
            this.exp = exp_;
            this.fecha_juego = fecha_juego_;

            // Historial
            this.id_historial = id_historial_;

            // Usuario_Historial
            this.id_usuario = id_usuario_;

            // Usuario
            this.id_tipo = id_tipo_;
            this.id_plaza = id_plaza_;
            this.nombre = nombre_;
            this.apellido_pat = apellido_pat_;
            this.apellido_mat = apellido_mat_;
        }

        // Constructor sin parametros
        public LeaderJ1() {}
    }
}