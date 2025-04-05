using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Org.BouncyCastle.Asn1.Cms;
using System;

namespace OxxoWeb.Models
{
    public class Recordatorio
    {
        public required string titulo { get; set; }
        public string? descripcion { get; set; }
        public required string lugar { get; set; }
        public DateOnly fecha {get; set;}
        public required TimeSpan hora_inicio { get; set; }
        public required TimeSpan hora_final { get; set; }
        public char tipo {get; set;}
        public int? id_tienda {get;set;} //Solamente necesario con el tipo tienda

        public Recordatorio(string titulo, string lugar, TimeSpan hora_inicio, TimeSpan hora_final, DateOnly fecha, char tipo, int? id_tienda = null, string? descripcion = null)
        {
            this.titulo = titulo;
            this.lugar = lugar;
            this.hora_inicio = hora_inicio;
            this.hora_final = hora_final;
            this.fecha = fecha;
            this.tipo = tipo;
            this.id_tienda = id_tienda;
            this.descripcion = descripcion;
        }
        public Recordatorio(){}
    }
}