using System; // Para poder hacer conexiones
using MySql.Data.MySqlClient; // Agregar MySQL, need to do desde NuGet
using System.Collections.Generic; 

namespace OxxoWeb.Models 
{
    public class AdolfoDatabaseContext
    {
        public string ConnectionString { get; set; }

        public AdolfoDatabaseContext()
        {
            ConnectionString = "Server=127.0.0.1;Port=3306;Database=oxxo_base_e1;Uid=root;password=root;";
        }

        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }

        public List<Usuario> GetAllUsuarios()
        {
            List<Usuario> listaUsuarios = new List<Usuario>();
            MySqlConnection conexion = GetConnection();
            conexion.Open();
            MySqlCommand cmd = new MySqlCommand("SELECT nombre, correo FROM usuario;", conexion);

            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    Usuario u = new Usuario();
                    u.nombre = reader["nombre"].ToString();
                    u.correo = reader["correo"].ToString();
                    listaUsuarios.Add(u);
                }
            }
            conexion.Close();
            return listaUsuarios;
        }

        public List<Contrasena> GetAllContrasenas()
        {
            List<Contrasena> listaContrasenas = new List<Contrasena>();
            MySqlConnection conexion = GetConnection();
            conexion.Open();
            MySqlCommand cmd = new MySqlCommand("SELECT IdContrasena, ValorContrasena FROM contrasena;", conexion);

            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    Contrasena c = new Contrasena(
                        Convert.ToInt32(reader["IdContrasena"]),
                        reader["ValorContrasena"].ToString()
                    );
                    listaContrasenas.Add(c);
                }
            }
            conexion.Close();
            return listaContrasenas;
        }

        public bool CheckCredentials(string user, string contrasena)
        {
            bool credencialesCorrectas = false;
            MySqlConnection conexion = GetConnection();
            conexion.Open();
            MySqlCommand cmd = new MySqlCommand(
                "SELECT COUNT(*) > 0 AS credenciales_correctas FROM usuario u INNER JOIN contrasena c ON u.id_contrasena = c.id_contrasena WHERE u.correo = @user AND c.contrasena = @contrasena;",conexion);
            cmd.Parameters.AddWithValue("@user", user);
            cmd.Parameters.AddWithValue("@contrasena", contrasena);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    credencialesCorrectas = Convert.ToBoolean(reader["credenciales_correctas"]);
                }
            }
            conexion.Close();
            return credencialesCorrectas;
        }
    }
}




