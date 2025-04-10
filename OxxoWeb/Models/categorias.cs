using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

//Clase de categorías
namespace OxxoWeb.Models
{
    public class Categoria
    {
        public int id_tienda { get; set; }
        public string? nombre_calle { get; set; }
        public string? color{get;set;}
        public Categoria(int id_tienda, string? nombre_calle)
        {
            this.id_tienda = id_tienda;
            this.nombre_calle = nombre_calle;
        }
    };
}