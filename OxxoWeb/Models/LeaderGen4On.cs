using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

// Conectar a modelo oxxo web
namespace OxxoWeb.Models
{
    public class LeaderGen4On
    {
        // Usuario
        public int id_usuario { get; set; }
        public int id_plaza { get; set; }
        public string nombre { get; set; }
        public string apellido_pat { get; set; }
        public string apellido_mat { get; set; }

        // Total exp calculado de SUM (j1.exp)
        public int total_exp { get; set; }

        // Constructor with parameters
        public LeaderGen4On(int id_usuario_, int id_plaza_, string nombre_, string apellido_pat_, string apellido_mat_, int total_exp_)
        {
            this.id_usuario = id_usuario_;
            this.id_plaza = id_plaza_;
            this.nombre = nombre_;
            this.apellido_pat = apellido_pat_;
            this.apellido_mat = apellido_mat_;
            this.total_exp = total_exp_;
        }

        // Default Constructor
        public LeaderGen4On() { }
    }
}

