using System; // Para poder hacer conexiones
using MySql.Data.MySqlClient; // Agregar MySQL, need to do desde NuGet
using System.Collections.Generic; 

namespace OxxoWeb.Models 
{
    public class MoniDataBaseContext
    {
        public string ConnectionString { get; set; } // String de conexión

        // Constructor para hacer la conexión (despues de deal con NuGet)
        public MoniDataBaseContext()
        {
            // Running on localhost port, using database default MySQL 3306, Uid root, and password mod
            ConnectionString = "Server=127.0.0.1;Port=3306;Database=oxxo_base_e1_2;Uid=root;password=80mB*%7aEf;";
        } 

        // Returns connection with connection string, private
        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }

        public List<Plazas> GetAllPlazas() 
        {
            List<Plazas> ListaPlazas = new List<Plazas>(); // list to store plazas
            MySqlConnection conexion = GetConnection(); // Initialize connection w previous method
            conexion.Open(); // Opens connection
            MySqlCommand cmd = new MySqlCommand("SELECT id_plaza, nombre FROM plaza;", conexion); // SQL query to conexion

            Plazas plaza1 = new Plazas(); // Instancia de plaza vacía

            // Command para ejecutar
            using (var reader = cmd.ExecuteReader())
            {
                // Returns a row as long as can read smth and returns false when done
                while (reader.Read())
                {
                    plaza1 = new Plazas(); // Guarda plaza
                    // Query de aqui = query dbeaver (nombre de columna)
                    // Si hacemos un join necesitamos incluir un AS para tener el nombre y que sí aparezca
                    plaza1.id_plaza = Convert.ToInt32(reader["id_plaza"]); // reader y el nombre de lo q tiene q obtener, como lo vemos en el query de dbeaver (no tienen q ser igual)
                    plaza1.nombre = reader["Nombre"].ToString(); // Convierte nombre de plaza a string

                    // Agrega dato completo a lista de plazas
                    ListaPlazas.Add(plaza1); 
                }
            }
            conexion.Close(); // Cierra conexión
            return ListaPlazas; // Regresa lista
        }

        // Obtener general
        // Juego 1
        public List<LeaderJ1> GetLeaderJ1() 
        {
            List<LeaderJ1> ListaJ1 = new List<LeaderJ1>(); // List to store players
            MySqlConnection conexion = GetConnection(); // Initialize connection
            conexion.Open(); // Open connection

            string queryJ1 = @"
                SELECT 
                    u.id_usuario, 
                    u.id_plaza, 
                    u.nombre, 
                    u.apellido_pat, 
                    u.apellido_mat, 
                    SUM(j1.exp) AS total_exp
                FROM usuario u
                JOIN usuario_historial uh ON u.id_usuario = uh.id_usuario
                JOIN historial h ON uh.id_historial = h.id_historial
                JOIN juego1 j1 ON h.id_juego1 = j1.id_juego1
                GROUP BY u.id_usuario, u.id_plaza, u.nombre, u.apellido_pat, u.apellido_mat
                ORDER BY total_exp DESC;
            ";

            MySqlCommand cmd = new MySqlCommand(queryJ1, conexion);

            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    LeaderJ1 ranked1 = new LeaderJ1();

                    // User Data
                    ranked1.id_usuario = Convert.ToInt32(reader["id_usuario"]);
                    ranked1.id_plaza = Convert.ToInt32(reader["id_plaza"]);
                    ranked1.nombre = reader["nombre"].ToString(); 
                    ranked1.apellido_pat = reader["apellido_pat"].ToString(); 
                    ranked1.apellido_mat = reader["apellido_mat"].ToString(); 

                    // Total Exp (SUM)
                    ranked1.total_exp = Convert.ToInt32(reader["total_exp"]); 

                    // Add to list
                    ListaJ1.Add(ranked1); 
                }
            }

            conexion.Close(); // Close connection
            return ListaJ1; // Return list
        }

        // Juego 2
        public List<LeaderJ2> GetLeaderJ2() 
        {
            List<LeaderJ2> ListaJ2 = new List<LeaderJ2>(); // List to store players
            MySqlConnection conexion = GetConnection(); // Initialize connection
            conexion.Open(); // Open connection

            string queryJ2 = @"
                SELECT 
                    u.id_usuario, 
                    u.id_plaza, 
                    u.nombre, 
                    u.apellido_pat, 
                    u.apellido_mat, 
                    SUM(j2.exp) AS total_exp
                FROM usuario u
                JOIN usuario_historial uh ON u.id_usuario = uh.id_usuario
                JOIN historial h ON uh.id_historial = h.id_historial
                JOIN juego2 j2 ON h.id_juego2 = j2.id_juego2
                GROUP BY u.id_usuario, u.id_plaza, u.nombre, u.apellido_pat, u.apellido_mat
                ORDER BY total_exp DESC;
            ";

            MySqlCommand cmd = new MySqlCommand(queryJ2, conexion);

            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    LeaderJ2 ranked2 = new LeaderJ2();

                    // User Data
                    ranked2.id_usuario = Convert.ToInt32(reader["id_usuario"]);
                    ranked2.id_plaza = Convert.ToInt32(reader["id_plaza"]);
                    ranked2.nombre = reader["nombre"].ToString(); 
                    ranked2.apellido_pat = reader["apellido_pat"].ToString(); 
                    ranked2.apellido_mat = reader["apellido_mat"].ToString(); 

                    // Total Exp (SUM)
                    ranked2.total_exp = Convert.ToInt32(reader["total_exp"]); 

                    // Add to list
                    ListaJ2.Add(ranked2); 
                }
            }

            conexion.Close(); // Close connection
            return ListaJ2; // Return list
        }
    }
}


    

