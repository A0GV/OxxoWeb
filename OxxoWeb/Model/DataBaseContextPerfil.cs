// DataBaseContext.cs
using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using OxxoWeb.Model;

namespace OxxoWeb.Model
{
    public class DataBaseContext
    {
        public string ConnectionString { get; set; }

        public DataBaseContext()
        {
            ConnectionString = "Server=127.0.0.1;Port=3306;Database=oxxo_base_e1;Uid=root;Password=Equs2004!!!!;";
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
                                        u.correo, u.fecha_inicio, p.nombre AS plaza_nombre, p.ciudad, p.estado, 
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
                            PlazaNombre = reader["plaza_nombre"].ToString(),
                            Ciudad = reader["ciudad"].ToString(),
                            Estado = reader["estado"].ToString(),
                            TipoUsuario = reader["tipo_usuario"].ToString()
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

                // Calcular racha de días jugando
                string rachaQuery = @"SELECT COUNT(DISTINCT fecha_juego) AS racha FROM (
                                        SELECT fecha_juego FROM juego1 WHERE id_usuario = @idUsuario
                                        UNION ALL
                                        SELECT fecha_juego FROM juego2 WHERE id_usuario = @idUsuario
                                        UNION ALL
                                        SELECT fecha_juego FROM juego3 WHERE id_usuario = @idUsuario
                                    ) juegos
                                    WHERE fecha_juego = CURDATE() OR fecha_juego = DATE_SUB(CURDATE(), INTERVAL 1 DAY);";
                MySqlCommand cmdRacha = new MySqlCommand(rachaQuery, conexion);
                cmdRacha.Parameters.AddWithValue("@idUsuario", idUsuario);
                stats.RachaDias = Convert.ToInt32(cmdRacha.ExecuteScalar() ?? 0);

                // Calcular ranking nacional por EXP
                string rankingQuery = @"SELECT RANK() OVER (ORDER BY total_exp DESC) AS ranking
                                        FROM (
                                            SELECT id_usuario, SUM(exp) AS total_exp FROM (
                                                SELECT id_usuario, exp FROM juego1
                                                UNION ALL
                                                SELECT id_usuario, exp FROM juego2
                                                UNION ALL
                                                SELECT id_usuario, exp FROM juego3
                                            ) juegos GROUP BY id_usuario
                                        ) ranking_general
                                        WHERE id_usuario = @idUsuario;";
                MySqlCommand cmdRanking = new MySqlCommand(rankingQuery, conexion);
                cmdRanking.Parameters.AddWithValue("@idUsuario", idUsuario);
                stats.RankingNacional = Convert.ToInt32(cmdRanking.ExecuteScalar() ?? 0);

                // Contar tiendas asesoradas
                string tiendasQuery = @"SELECT COUNT(*) FROM tienda t
                                        JOIN usuario u ON t.id_plaza = u.id_plaza
                                        WHERE u.id_usuario = @idUsuario;";
                MySqlCommand cmdTiendas = new MySqlCommand(tiendasQuery, conexion);
                cmdTiendas.Parameters.AddWithValue("@idUsuario", idUsuario);
                stats.TiendasAsesoradas = Convert.ToInt32(cmdTiendas.ExecuteScalar() ?? 0);
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
                string query = "SELECT id_pub, titulo, fecha_publicado, contenido FROM publicacion WHERE id_usuario = @idUsuario";
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
                string query = "SELECT id_certificado, titulo, fecha_subido, descripcion FROM certificado WHERE id_usuario = @idUsuario";
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