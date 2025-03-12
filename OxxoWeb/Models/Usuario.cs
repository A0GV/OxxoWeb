using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

// Conectar a modelo oxxo web
namespace OxxoWeb.Models
{
    public class Usuario
    {
        // Valores de entidad, tuve que modificar algunos
        public int id_usuario {get;set;}
        public int id_tipo {get;set;}
        public int id_plaza {get;set;}
        public int id_contrasena {get;set;}
        public string nombre {get;set;}
        public string apellido_pat {get;set;}
        public string apellido_mat {get;set;}
        public DateTime fecha_nacimiento {get;set;}
        public string foto {get;set;}
        public string correo {get;set;}

        public DateTime fecha_inicio {get;set;}

        public Usuario(int id_usuario_,int id_tipo_,int id_plaza_,int id_contrasena_,string nombre_,string apellido_pat_,string apellido_mat_,DateTime fecha_nacimiento_,string foto_,string correo_,DateTime fecha_inicio_)
        {
            this.id_usuario = id_usuario_;
            this.id_tipo = id_tipo_;
            this.id_plaza = id_plaza_;
            this.id_contrasena = id_contrasena_;
            this.nombre = nombre_;
            this.apellido_pat = apellido_pat_;
            this.apellido_mat = apellido_mat_;
            this.fecha_nacimiento = fecha_nacimiento_;
            this.foto = foto_;
            this.correo = correo_;
            this.fecha_inicio = fecha_inicio_;
        }

        public Usuario() {}
    }
}