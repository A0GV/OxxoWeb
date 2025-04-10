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

        // ==========================================
        // Constructor que lee el appsettings.json
        // para obtener la cadena de conexión a la BD
        // ==========================================
        public GaliDatabaseContext()
        {
            var objBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            IConfiguration conManager = objBuilder.Build();

            var conn = conManager.GetConnectionString("MyDb1");
            this.ConnectionString = conn ?? "";
        }

        // ==========================================
        // Método para obtener la conexión lista
        // ==========================================
        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }

        // ==========================================
        // MÉTODO PARA PROBAR SI LA CONEXIÓN FUNCIONA
        // ==========================================
        public bool ProbarConexion()
        {
            try
            {
                using var connection = GetConnection();
                connection.Open();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error de conexión: " + ex.Message);
                return false;
            }
        }

        // ==========================================
        // MÉTODOS PARA LOS KPIs DEL PANEL DE GERENTE
        // ==========================================

        // 1. Obtener número total de asesores
        public int GetTotalAsesores()
        {
            using var connection = GetConnection();
            connection.Open();
            var cmd = new MySqlCommand("SELECT COUNT(*) FROM usuario WHERE id_tipo = 1;", connection);
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        // 2. Obtener número total de tareas (metas) activas
        public int GetMetasActivas()
        {
            using var connection = GetConnection();
            connection.Open();
            var cmd = new MySqlCommand("SELECT COUNT(*) FROM tarea WHERE fecha_limite >= CURDATE();", connection);
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        // 3. Obtener número total de plazas registradas
        public int GetPlazasRegistradas()
        {
            using var connection = GetConnection();
            connection.Open();
            var cmd = new MySqlCommand("SELECT COUNT(*) FROM plaza;", connection);
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        // 4. Obtener número total de tiendas registradas
        public int GetTiendasRegistradas()
        {
            using var connection = GetConnection();
            connection.Open();
            var cmd = new MySqlCommand("SELECT COUNT(*) FROM tienda;", connection);
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        // 5. Obtener número total de tareas asignadas
        public int GetTareasTotales()
        {
            using var connection = GetConnection();
            connection.Open();
            var cmd = new MySqlCommand("SELECT COUNT(*) FROM tarea;", connection);
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        // 6. Obtener número de tareas próximas a vencer (en menos de 3 días)
        public int GetTareasProximasAVencer()
        {
            using var connection = GetConnection();
            connection.Open();
            var cmd = new MySqlCommand(@"
                SELECT COUNT(*) 
                FROM tarea 
                WHERE fecha_limite BETWEEN CURDATE() AND DATE_ADD(CURDATE(), INTERVAL 3 DAY);", connection);
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        // ==========================================
        // Obtener progreso de asesores
        // ==========================================
        public List<ProgresoAsesor> GetProgresoAsesores()
        {
            var lista = new List<ProgresoAsesor>();

            using var connection = GetConnection();
            connection.Open();

            var query = @"
                SELECT u.nombre, u.apellido_pat, u.apellido_mat, t.tipo, t.estado, t.fecha_limite 
                FROM usuario u
                JOIN tarea t ON u.id_usuario = t.id_usuario
                WHERE u.id_tipo = 1;";

            using var cmd = new MySqlCommand(query, connection);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                var asesor = new ProgresoAsesor
                {
                    Nombre = reader["nombre"].ToString() ?? "",
                    ApellidoPat = reader["apellido_pat"].ToString() ?? "",
                    ApellidoMat = reader["apellido_mat"].ToString() ?? "",
                    Reto = reader["tipo"].ToString() ?? "",
                    Estado = reader["estado"].ToString() ?? "",
                    FechaLimite = Convert.ToDateTime(reader["fecha_limite"])
                };
                lista.Add(asesor);
            }

            return lista;
        }

        // ==========================================
        // PARA ASIGNAR TAREA
        // ==========================================
        public void InsertarTarea(Tarea tarea)
    {
    using var connection = GetConnection();
    connection.Open();

    var query = @"INSERT INTO tarea (titulo, tipo, fecha_limite, id_usuario, estado)
                  VALUES (@titulo, @tipo, @fecha_limite, @id_usuario, @estado)";

    using var cmd = new MySqlCommand(query, connection);
    cmd.Parameters.AddWithValue("@titulo", tarea.Titulo);
    cmd.Parameters.AddWithValue("@tipo", tarea.Tipo);
    cmd.Parameters.AddWithValue("@fecha_limite", tarea.FechaLimite);
    cmd.Parameters.AddWithValue("@id_usuario", tarea.IdUsuario);
    cmd.Parameters.AddWithValue("@estado", "Sin empezar"); // ✅ nuevo campo

    cmd.ExecuteNonQuery();
}


        // ==========================================
        // Obtener lista de asesores para el formulario
        // ==========================================
        public List<Usuario> GetAsesores()
        {
            var asesores = new List<Usuario>();

            using var connection = GetConnection();
            connection.Open();

            var query = @"SELECT id_usuario, nombre, apellido_pat, apellido_mat FROM usuario WHERE id_tipo = 1";
            using var cmd = new MySqlCommand(query, connection);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                var asesor = new Usuario
                {
                    IdUsuario = Convert.ToInt32(reader["id_usuario"]),
                    nombre = reader["nombre"].ToString() ?? "",
                    apellido_pat = reader["apellido_pat"].ToString() ?? "",
                    apellido_mat = reader["apellido_mat"].ToString() ?? ""
                };
                asesores.Add(asesor);
            }

            return asesores;
        }

        // ==========================================
        // Obtener tipos de tarea únicos para el formulario
        // ==========================================
        public List<string> GetTiposTarea()
        {
            var tipos = new List<string>();

            using var connection = GetConnection();
            connection.Open();

            var query = "SELECT DISTINCT tipo FROM tarea";
            using var cmd = new MySqlCommand(query, connection);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                tipos.Add(reader["tipo"].ToString() ?? "");
            }

            return tipos;
        }

        // ==========================================
        // MÉTRICAS PARA BARRAS DE PROGRESO (NUEVAS)
        // ==========================================

        // 1. Total de tareas de capacitación finalizadas
        public int GetCapacitacionesFinalizadas()
        {
            using var connection = GetConnection();
            connection.Open();

            var query = @"SELECT COUNT(*) 
                          FROM tarea 
                          WHERE tipo = 'Capacitación' AND estado = 'Completado';";

            using var cmd = new MySqlCommand(query, connection);
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        // 2. EXP acumulado por todos los asesores
        public int GetTotalEXP()
        {
            using var connection = GetConnection();
            connection.Open();

            var query = @"
                SELECT COALESCE(SUM(h.exp), 0)
                FROM historial h
                JOIN usuario_historial uh ON h.id_historial = uh.id_historial
                JOIN usuario u ON u.id_usuario = uh.id_usuario
                WHERE u.id_tipo = 1;";

            using var cmd = new MySqlCommand(query, connection);
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        // 3. Total de certificados obtenidos por asesores
        public int GetTotalCertificados()
        {
            using var connection = GetConnection();
            connection.Open();

            var query = @"
                SELECT COUNT(*) 
                FROM certificado c
                JOIN usuario u ON c.id_usuario = u.id_usuario
                WHERE u.id_tipo = 1;";

            using var cmd = new MySqlCommand(query, connection);
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        // 4. Total de publicaciones recientes (últimos 30 días)
        public int GetPublicacionesRecientes()
        {
            using var connection = GetConnection();
            connection.Open();

            var query = @"
                SELECT COUNT(*) 
                FROM publicacion 
                WHERE fecha_publicado >= DATE_SUB(NOW(), INTERVAL 30 DAY);";

            using var cmd = new MySqlCommand(query, connection);
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        // ==========================================
        // Logros (sección EQUIPO)
        // ==========================================

        // 1. ¿Todos los asesores completaron al menos una capacitación?
        public bool Logro_CapacitacionPorTodos()
        {
            using var connection = GetConnection();
            connection.Open();

            var query = @"
                SELECT COUNT(DISTINCT u.id_usuario)
                FROM usuario u
                WHERE u.id_tipo = 1
                AND EXISTS (
                    SELECT 1
                    FROM tarea t
                    WHERE t.id_usuario = u.id_usuario
                    AND t.tipo = 'Capacitación'
                    AND t.estado = 'Completado'
                );";

            var cmd = new MySqlCommand(query, connection);
            int asesoresConCapacitacion = Convert.ToInt32(cmd.ExecuteScalar());

            // Comparamos contra el total de asesores
            return asesoresConCapacitacion == GetTotalAsesores();
        }

        // 2. ¿El equipo ha acumulado más de 5000 EXP?
        public bool Logro_EXP5000()
        {
            return GetTotalEXP() >= 5000;
        }

        // 3. ¿Hay al menos 5 certificados?
        public bool Logro_CincoCertificados()
        {
            using var connection = GetConnection();
            connection.Open();

            var query = "SELECT COUNT(*) FROM certificado;";
            var cmd = new MySqlCommand(query, connection);
            return Convert.ToInt32(cmd.ExecuteScalar()) >= 5;
        }

        // 4. ¿Hay al menos 5 publicaciones en los últimos 30 días?
        public bool Logro_CincoPublicacionesRecientes()
        {
            return GetPublicacionesRecientes() >= 5;
        }

        // 5. ¿Algún asesor ha completado al menos 5 tareas?
        public bool Logro_AsesorCincoMetas()
        {
            using var connection = GetConnection();
            connection.Open();

            var query = @"
                SELECT MAX(total) FROM (
                    SELECT COUNT(*) AS total
                    FROM tarea t
                    JOIN usuario u ON u.id_usuario = t.id_usuario
                    WHERE u.id_tipo = 1 AND t.estado = 'Completado'
                    GROUP BY u.id_usuario
                ) AS sub;";

            var cmd = new MySqlCommand(query, connection);
            var result = cmd.ExecuteScalar();
            return result != DBNull.Value && Convert.ToInt32(result) >= 5;
        }

        // 6. ¿Se ha completado al menos un reto de cada tipo?
        public bool Logro_TodosTiposCompletados()
        {
            using var connection = GetConnection();
            connection.Open();

            var query = @"
                SELECT COUNT(DISTINCT tipo)
                FROM tarea
                WHERE estado = 'Completado' 
                AND tipo IN ('Capacitación', 'Reto EXP', 'Registro diario');";

            var cmd = new MySqlCommand(query, connection);
            return Convert.ToInt32(cmd.ExecuteScalar()) == 3;
        }

        // ==========================================
        //  Logros (sección INDIVIDUAL - ranking)
        // ==========================================
        public List<(string NombreCompleto, int Total)> ObtenerTopAsesoresPorMetas()
        {
            var ranking = new List<(string, int)>();

            using var connection = GetConnection();
            connection.Open();

            var query = @"
                SELECT CONCAT(nombre, ' ', apellido_pat, ' ', apellido_mat) AS nombre_completo,
                COUNT(*) AS total_metas
                FROM usuario u
                JOIN tarea t ON u.id_usuario = t.id_usuario
                WHERE u.id_tipo = 1 AND t.estado = 'Completado'
                GROUP BY u.id_usuario
                ORDER BY total_metas DESC
                LIMIT 6;";

            using var cmd = new MySqlCommand(query, connection);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                var nombre = reader["nombre_completo"].ToString() ?? "";
                var total = Convert.ToInt32(reader["total_metas"]);
                ranking.Add((nombre, total));
            }

            return ranking;
        }

    }
}
