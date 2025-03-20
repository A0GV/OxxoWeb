// DataBaseContext.cs
using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using OxxoWeb.Models;

namespace OxxoWeb.Models
{
    public class DataBaseContextPerfil
    {
        
        public string ConnectionString { get; set; }

        public DataBaseContextPerfil()
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

        // Obtener información del perfil de usuario
        public Perfiles GetPerfil(int idUsuario)
        {
            Perfiles perfil = null;
            using (MySqlConnection conexion = GetConnection())
            {
                conexion.Open();
                string query = @"SELECT u.id_usuario, u.nombre, u.apellido_pat, u.apellido_mat, u.fecha_nacimiento,
                                        u.correo, u.fecha_inicio, u.foto, p.nombre AS plaza_nombre, p.ciudad, p.estado, 
                                        t.descripcion AS tipo_usuario 
                                FROM usuario u
                                JOIN plaza p ON u.id_plaza = p.id_plaza
                                JOIN tipo t ON u.id_tipo = t.id_tipo
                                WHERE u.id_usuario = @idUsuario";
                MySqlCommand cmd = new MySqlCommand(query, conexion);
                cmd.Parameters.AddWithValue("@idUsuario", idUsuario);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        perfil = new Perfiles
                        {
                            IdUsuario = Convert.ToInt32(reader["id_usuario"]),
                            Nombre = reader["nombre"].ToString(),
                            ApellidoPat = reader["apellido_pat"].ToString(),
                            ApellidoMat = reader["apellido_mat"].ToString(),
                            FechaNacimiento = Convert.ToDateTime(reader["fecha_nacimiento"]),
                            Correo = reader["correo"].ToString(),
                            FechaInicio = Convert.ToDateTime(reader["fecha_inicio"]),
                            PlazaNombre = "Plaza " + reader["plaza_nombre"].ToString().Replace("_", ", "),
                            Ciudad = reader["ciudad"].ToString(),
                            Estado = reader["estado"].ToString(),
                            TipoUsuario = reader["tipo_usuario"].ToString(),
                            Foto = reader["foto"].ToString() // <-- Agregado aquí
                        };
                    }
                }
            }
            return perfil;
        }

        // Obtener estadísticas del usuario
        public Estadisticas GetEstadisticas(int idUsuario)
        {
            Estadisticas stats = new Estadisticas { IdUsuario = idUsuario };
            using (MySqlConnection conexion = GetConnection())
            {
                conexion.Open();

                // Verificar si el usuario es gerente
                string tipoUsuarioQuery = @"SELECT t.descripcion AS tipo_usuario 
                                            FROM usuario u
                                            JOIN tipo t ON u.id_tipo = t.id_tipo
                                            WHERE u.id_usuario = @idUsuario";
                MySqlCommand cmdTipoUsuario = new MySqlCommand(tipoUsuarioQuery, conexion);
                cmdTipoUsuario.Parameters.AddWithValue("@idUsuario", idUsuario);
                string tipoUsuario = cmdTipoUsuario.ExecuteScalar()?.ToString();                

                // Calcular racha de días jugando
                string rachaQuery = @"
                    SELECT COUNT(*) AS racha
                    FROM (
                        SELECT fecha_juego, LAG(fecha_juego) OVER (ORDER BY fecha_juego ASC) AS prev_date
                        FROM (
                            SELECT j1.fecha_juego FROM usuario_historial uh
                            JOIN historial h ON uh.id_historial = h.id_historial
                            JOIN juego1 j1 ON h.id_juego1 = j1.id_juego1
                            WHERE uh.id_usuario = @idUsuario
                            UNION
                            SELECT j2.fecha_juego FROM usuario_historial uh
                            JOIN historial h ON uh.id_historial = h.id_historial
                            JOIN juego2 j2 ON h.id_juego2 = j2.id_juego2
                            WHERE uh.id_usuario = @idUsuario
                            UNION
                            SELECT j3.fecha_juego FROM usuario_historial uh
                            JOIN historial h ON uh.id_historial = h.id_historial
                            JOIN juego3 j3 ON h.id_juego3 = j3.id_juego3
                            WHERE uh.id_usuario = @idUsuario
                        ) juegos
                    ) juegos_ordenados
                    WHERE DATEDIFF(fecha_juego, prev_date) = 1;";

                MySqlCommand cmdRacha = new MySqlCommand(rachaQuery, conexion);
                cmdRacha.Parameters.AddWithValue("@idUsuario", idUsuario);
                stats.RachaDias = Convert.ToInt32(cmdRacha.ExecuteScalar() ?? 0);

                // Calcular ranking nacional por EXP
                string rankingQuery = @"SELECT ranking FROM (
                        SELECT u.id_usuario, DENSE_RANK() OVER (ORDER BY SUM(j1.exp + j2.exp + j3.exp) DESC) AS ranking
                        FROM usuario u
                        JOIN usuario_historial uh ON u.id_usuario = uh.id_usuario
                        JOIN historial h ON uh.id_historial = h.id_historial
                        LEFT JOIN juego1 j1 ON h.id_juego1 = j1.id_juego1
                        LEFT JOIN juego2 j2 ON h.id_juego2 = j2.id_juego2
                        LEFT JOIN juego3 j3 ON h.id_juego3 = j3.id_juego3
                        GROUP BY u.id_usuario
                    ) AS ranking_table
                    WHERE id_usuario = @idUsuario;
                ";

                MySqlCommand cmdRanking = new MySqlCommand(rankingQuery, conexion);
                cmdRanking.Parameters.AddWithValue("@idUsuario", idUsuario);
                stats.RankingNacional = Convert.ToInt32(cmdRanking.ExecuteScalar() ?? 0);

                // Contar tiendas asesoradas
                if (tipoUsuario != "g") // Verificar si el usuario no es gerente
                {
                    string tiendasQuery = @"SELECT COUNT(*) FROM tienda t
                                            JOIN usuario u ON t.id_plaza = u.id_plaza
                                            WHERE u.id_usuario = @idUsuario;";
                    MySqlCommand cmdTiendas = new MySqlCommand(tiendasQuery, conexion);
                    cmdTiendas.Parameters.AddWithValue("@idUsuario", idUsuario);
                    stats.TiendasAsesoradas = Convert.ToInt32(cmdTiendas.ExecuteScalar() ?? 0);
                }
                else
                {
                    stats.TiendasAsesoradas = 0; // Si es gerente, establecer a 0
                }
            }
            return stats;
        }

        // Obtener publicaciones del usuario
        public List<Publicacion> GetPublicaciones(int idUsuario)
        {
            List<Publicacion> publicaciones = new List<Publicacion>();
            using (MySqlConnection conexion = GetConnection())
            {
                conexion.Open();
                string query = "SELECT id_pub, titulo, fecha_publicado, contenido FROM publicacion WHERE id_usuario = @idUsuario ORDER BY fecha_publicado DESC"; // <-- Aquí está la clave"
                MySqlCommand cmd = new MySqlCommand(query, conexion);
                cmd.Parameters.AddWithValue("@idUsuario", idUsuario);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        publicaciones.Add(new Publicacion
                        {
                            IdPublicacion = Convert.ToInt32(reader["id_pub"]),
                            IdUsuario = idUsuario,
                            Titulo = reader["titulo"].ToString(),
                            FechaPublicado = Convert.ToDateTime(reader["fecha_publicado"]),
                            Contenido = reader["contenido"].ToString()
                        });
                    }
                }
            }
            return publicaciones;
        }

        // Obtener certificados del usuario
        public List<Cerificados> GetCertificados(int idUsuario)
        {
            List<Cerificados> certificados = new List<Cerificados>();
            using (MySqlConnection conexion = GetConnection())
            {
                conexion.Open();
                string query = "SELECT id_certificado, titulo, fecha_subido, descripcion FROM certificado WHERE id_usuario = @idUsuario ORDER BY fecha_subido DESC"; // <-- Aquí está la clave";
                MySqlCommand cmd = new MySqlCommand(query, conexion);
                cmd.Parameters.AddWithValue("@idUsuario", idUsuario);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        certificados.Add(new Cerificados
                        {
                            IdCertificado = Convert.ToInt32(reader["id_certificado"]),
                            IdUsuario = idUsuario,
                            Titulo = reader["titulo"].ToString(),
                            FechaSubido = Convert.ToDateTime(reader["fecha_subido"]),
                            Descripcion = reader["descripcion"].ToString()
                        });
                    }
                }
            }
            return certificados;
        }
    }
}