using System; // Para poder hacer conexiones
using MySql.Data.MySqlClient; // Agregar MySQL, need to do desde NuGet
using System.Collections.Generic;

namespace OxxoWeb.Models
{
    public class Contrasena
    {
        public int IdContrasena { get; set; }
        public string ValorContrasena { get; set; }

        public Contrasena(int idContrasena, string valorContrasena)
        {
            this.IdContrasena = idContrasena;
            this.ValorContrasena = valorContrasena;
        }
    }
}