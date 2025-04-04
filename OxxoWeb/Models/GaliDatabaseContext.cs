using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace OxxoWeb.Models
{
    public class GaliDatabaseContext
    {
        public string ConnectionString { get; set; }

        public GaliDatabaseContext()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false);
            IConfiguration config = builder.Build();
            ConnectionString = config.GetConnectionString("MyDb1") ?? "";
        }

        private MySqlConnection GetConnection() => new MySqlConnection(ConnectionString);

        public List<(string Nombre, string Reto, string Estado)> GetProgresoAsesores()
        {
            var lista = new List<(string, string, string)>();
            using var conn = GetConnection();
            conn.Open();

            var cmd = new MySqlCommand(@"
                SELECT u.nombre, t.tipo AS reto, 
                    CASE 
                        WHEN t.fecha_limite < CURDATE() THEN 'Sin empezar'
                        WHEN t.fecha_limite = CURDATE() THEN 'En progreso'
                        ELSE 'Completado'
                    END AS estado
                FROM tarea t
                JOIN usuario u ON u.id_usuario = t.id_usuario;", conn);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                lista.Add((
                    reader["nombre"].ToString(),
                    reader["reto"].ToString(),
                    reader["estado"].ToString()
                ));
            }

            return lista;
        }

        public void InsertarTarea(string titulo, string tipo, DateTime fechaLimite, int idUsuario)
        {
            using var conn = GetConnection();
            conn.Open();

            var cmd = new MySqlCommand(@"
                INSERT INTO tarea (titulo, tipo, fecha_limite, id_usuario)
                VALUES (@titulo, @tipo, @fecha, @usuario);", conn);

            cmd.Parameters.AddWithValue("@titulo", titulo);
            cmd.Parameters.AddWithValue("@tipo", tipo);
            cmd.Parameters.AddWithValue("@fecha", fechaLimite);
            cmd.Parameters.AddWithValue("@usuario", idUsuario);

            cmd.ExecuteNonQuery();
        }
    }
}
