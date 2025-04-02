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

        public Perfiles GetPerfilPorNombre(string nombre)
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
                                WHERE CONCAT(u.nombre, ' ', u.apellido_pat, ' ', u.apellido_mat) LIKE @nombre";
                MySqlCommand cmd = new MySqlCommand(query, conexion);
                cmd.Parameters.AddWithValue("@nombre", $"%{nombre}%");
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
                            Foto = reader["foto"].ToString()
                        };
                    }
                }
            }
            return perfil;
        }

        // Obtener estadísticas del usuario
        public Estadisticas GetEstadisticas(int idUsuario)
        {
            Estadisticas estadisticas = new Estadisticas
            {
                IdUsuario = idUsuario,
                RachaDias = 0, // Valor predeterminado
                RankingNacional = 0, // Valor predeterminado
                TiendasAsesoradas = 0 // Valor predeterminado
            };

            using (MySqlConnection conexion = GetConnection())
            {
                conexion.Open();

                // Obtener el número de tiendas asesoradas
                string queryTiendas = "SELECT COUNT(*) FROM usuario_tienda WHERE id_usuario = @idUsuario";
                MySqlCommand cmdTiendas = new MySqlCommand(queryTiendas, conexion);
                cmdTiendas.Parameters.AddWithValue("@idUsuario", idUsuario);
                var resultTiendas = cmdTiendas.ExecuteScalar();
                if (resultTiendas != null)
                {
                    estadisticas.TiendasAsesoradas = Convert.ToInt32(resultTiendas);
                }

                // Obtener la posición en el ranking general
                string queryRanking = @"
                    SELECT COUNT(*) + 1 AS ranking
                    FROM (
                        SELECT id_usuario, SUM(exp) AS total_exp
                        FROM usuario_historial uh
                        JOIN historial h ON uh.id_historial = h.id_historial
                        GROUP BY id_usuario
                    ) subquery
                    WHERE total_exp > (
                        SELECT SUM(exp)
                        FROM usuario_historial uh
                        JOIN historial h ON uh.id_historial = h.id_historial
                        WHERE uh.id_usuario = @idUsuario
                    )";
                MySqlCommand cmdRanking = new MySqlCommand(queryRanking, conexion);
                cmdRanking.Parameters.AddWithValue("@idUsuario", idUsuario);
                var resultRanking = cmdRanking.ExecuteScalar();
                if (resultRanking != null)
                {
                    estadisticas.RankingNacional = Convert.ToInt32(resultRanking);
                }

                // Calcular la racha de días consecutivos
                string queryFechas = @"
                    SELECT DISTINCT DATE(h.fecha) AS fecha
                    FROM usuario_historial uh
                    JOIN historial h ON uh.id_historial = h.id_historial
                    WHERE uh.id_usuario = @idUsuario
                    ORDER BY fecha DESC";
                MySqlCommand cmdFechas = new MySqlCommand(queryFechas, conexion);
                cmdFechas.Parameters.AddWithValue("@idUsuario", idUsuario);

                List<DateTime> fechas = new List<DateTime>();
                using (var reader = cmdFechas.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        fechas.Add(Convert.ToDateTime(reader["fecha"]));
                    }
                }

                // Calcular la racha
                DateTime fechaHoy = DateTime.Today;
                int racha = 0;

                foreach (var fecha in fechas)
                {
                    if ((fechaHoy - fecha).TotalDays == 0 || (fechaHoy - fecha).TotalDays == 1)
                    {
                        racha++;
                        fechaHoy = fecha; // Actualizar la fecha para verificar la siguiente en la racha
                    }
                    else
                    {
                        break; // Si no es consecutiva, detener el cálculo
                    }
                }

                estadisticas.RachaDias = racha;
            }

            return estadisticas;
        }

        // Obtener publicaciones del usuario
        public List<Publicacion> GetPublicaciones(int idUsuario)
        {
            List<Publicacion> publicaciones = new List<Publicacion>();
            using (MySqlConnection conexion = GetConnection())
            {
                conexion.Open();
                string query = "SELECT id_pub, titulo, fecha_publicado, contenido FROM publicacion WHERE id_usuario = @idUsuario ORDER BY fecha_publicado DESC";
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
                string query = "SELECT id_certificado, titulo, fecha_publicado, descripcion FROM certificado WHERE id_usuario = @idUsuario ORDER BY fecha_publicado DESC";
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
                            FechaPublicado = Convert.ToDateTime(reader["fecha_publicado"]), // Cambiado
                            Descripcion = reader["descripcion"].ToString()
                        });
                    }
                }
            }
            return certificados;
        }
    }
}
