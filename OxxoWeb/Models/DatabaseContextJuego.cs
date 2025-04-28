using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace OxxoWeb.Models
{
    public class DatabaseContextJuego
    {
        public string ConnectionString { get; set; }

        public DatabaseContextJuego()
        {
            var objBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            IConfiguration conManager = objBuilder.Build();

            var conn = conManager.GetConnectionString("MyDb1");
            if (conn == null)
            {
                conn = "";
            }
            this.ConnectionString = conn;
        }

        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }

        public AyudaVideojuego GetAyudaVideojuego()
        {
            AyudaVideojuego ayuda = new AyudaVideojuego();

            using (MySqlConnection conexion = GetConnection())
            {
                conexion.Open();

                // 1. Obtener la información principal de ayuda
                string queryAyuda = "SELECT descripcion, creditos, licencias FROM ayuda_videojuego LIMIT 1;";
                using (MySqlCommand cmd = new MySqlCommand(queryAyuda, conexion))
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        ayuda.Descripcion = reader["descripcion"].ToString();
                        ayuda.Creditos = reader["creditos"].ToString();
                        ayuda.Licencias = reader["licencias"].ToString();
                    }
                }

                // 2. Obtener personajes
                ayuda.Personajes = new List<Personaje>();
                string queryPersonaje = "SELECT nombre, descripcion FROM personaje;";
                using (MySqlCommand cmd = new MySqlCommand(queryPersonaje, conexion))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ayuda.Personajes.Add(new Personaje
                        {
                            Nombre = reader["nombre"].ToString(),
                            Descripcion = reader["descripcion"].ToString()
                        });
                    }
                }

                // 3. Obtener minijuegos
                ayuda.Minijuegos = new List<Minijuego>();
                string queryMinijuegos = "SELECT id_minijuego, nombre, descripcion FROM minijuego;";
                using (MySqlCommand cmd = new MySqlCommand(queryMinijuegos, conexion))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ayuda.Minijuegos.Add(new Minijuego
                        {
                            IdMinijuego = Convert.ToInt32(reader["id_minijuego"]),
                            Nombre = reader["nombre"].ToString(),
                            Descripcion = reader["descripcion"].ToString(),
                            Controles = new List<ControlMinijuego>(),
                            Condiciones = new List<CondicionMinijuego>()
                        });
                    }
                }
            }

            // 4. Por cada minijuego, obtener sus controles y condiciones
            foreach (var minijuego in ayuda.Minijuegos)
            {
                using (MySqlConnection conexion = GetConnection())
                {
                    conexion.Open();

                    // Controles del minijuego
                    string queryControles = "SELECT control, descripcion FROM control_minijuego WHERE id_minijuego = @id;";
                    using (MySqlCommand cmd = new MySqlCommand(queryControles, conexion))
                    {
                        cmd.Parameters.AddWithValue("@id", minijuego.IdMinijuego);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                minijuego.Controles.Add(new ControlMinijuego
                                {
                                    Control = reader["control"].ToString(),
                                    Descripcion = reader["descripcion"].ToString()
                                });
                            }
                        }
                    }

                    // Condiciones del minijuego
                    string queryCondiciones = "SELECT tipo_condicion, descripcion FROM condicion_minijuego WHERE id_minijuego = @id;";
                    using (MySqlCommand cmd = new MySqlCommand(queryCondiciones, conexion))
                    {
                        cmd.Parameters.AddWithValue("@id", minijuego.IdMinijuego);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                minijuego.Condiciones.Add(new CondicionMinijuego
                                {
                                    TipoCondicion = reader["tipo_condicion"].ToString(),
                                    Descripcion = reader["descripcion"].ToString()
                                });
                            }
                        }
                    }
                }
            }

            return ayuda;
        }
    }
}
