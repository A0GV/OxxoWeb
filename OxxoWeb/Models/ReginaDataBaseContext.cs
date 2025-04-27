using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;


namespace OxxoWeb.Models
{
    public class ReginaDataBaseContext
    {
        public string ConnectionString { get; set; }

        public ReginaDataBaseContext()
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

        public List<AsesorInfo> GetAsesoresInfo()
        {
            List<AsesorInfo> asesores = new List<AsesorInfo>();
            using (MySqlConnection conexion = GetConnection())
            {
                conexion.Open();
                string query = @"
                    SELECT u.id_usuario, u.nombre, u.apellido_pat, u.apellido_mat, u.correo,
                           u.foto, p.nombre AS plaza_nombre, p.ciudad, p.estado, t.descripcion AS tipo_usuario
                    FROM usuario u
                    JOIN plaza p ON u.id_plaza = p.id_plaza
                    JOIN tipo t ON u.id_tipo = t.id_tipo
                    WHERE u.id_tipo = 1;";

                MySqlCommand cmd = new MySqlCommand(query, conexion);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        AsesorInfo info = new AsesorInfo
                        {
                            IdUsuario = Convert.ToInt32(reader["id_usuario"]),
                            Nombre = reader["nombre"].ToString(),
                            ApellidoPat = reader["apellido_pat"].ToString(),
                            ApellidoMat = reader["apellido_mat"].ToString(),
                            Correo = reader["correo"].ToString(),
                            TipoUsuario = reader["tipo_usuario"].ToString(),
                            Foto = reader["foto"].ToString()
                        };
                        asesores.Add(info);
                    }
                }
            }
            return asesores;
        }

        public Dictionary<int, int> GetTiendasNum(){
        Dictionary<int, int> tiendasPorAsesor = new();

        using (MySqlConnection conexion = GetConnection())
        {
            conexion.Open();
            string query = @"
                SELECT id_usuario, COUNT(*) AS tiendas_count
                FROM usuario_tienda
                GROUP BY id_usuario;";

            using (MySqlCommand cmd = new MySqlCommand(query, conexion))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    int idUsuario = Convert.ToInt32(reader["id_usuario"]);
                    int count = Convert.ToInt32(reader["tiendas_count"]);
                    tiendasPorAsesor[idUsuario] = count;
                }
            }
        }
        return tiendasPorAsesor;
    }


    public int GetTipoUsuario(string correo){
        int tipo = 0;
        MySqlConnection conexion = GetConnection();
        conexion.Open();
        MySqlCommand cmd = new MySqlCommand("SELECT id_tipo FROM usuario WHERE correo = @correo;", conexion);
        cmd.Parameters.AddWithValue("@correo", correo);

        using (var reader = cmd.ExecuteReader())
        {
            if (reader.Read())
            {
                tipo = Convert.ToInt32(reader["id_tipo"]);
            }
        }
    conexion.Close();
    return tipo;
}



        
    }
}