using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

// Conectar a modelo oxxo web
namespace OxxoWeb.Models
{
    public class Plazas
    {
        // Valores de entidad
        public int id_plaza {get;set;}
        public string nombre {get;set;}
        public string ciudad {get;set;}
        public string estado {get;set;}

        // Constructor con parametros
        public Plazas(int id_plaza_,string nombre_,string ciudad_,string estado_)
        {
            this.id_plaza = id_plaza_;
            this.nombre = nombre_;
            this.ciudad = ciudad_;
            this.estado = estado_;
        }
        
        // Empty constructor
        public Plazas() {}
    }
}


