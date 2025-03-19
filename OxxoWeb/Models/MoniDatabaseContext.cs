using System; // Para poder hacer conexiones
using MySql.Data.MySqlClient; // Agregar MySQL, need to do desde NuGet
using System.Collections.Generic; 

namespace OxxoWeb.Models 
{
    public class MoniDataBaseContext
    {
        public string ConnectionString { get; set; } // String de conexión

        // Constructor para hacer la conexión (despues de deal con NuGet)
        /*public MoniDataBaseContext()
        {
            // Running on localhost port, using database default MySQL 3306, Uid root, and password mod
            ConnectionString = "Server=127.0.0.1;Port=3306;Database=oxxo_base_e1_2;Uid=root;password=80mB*%7aEf;";
        }*/


        // Returns connection with connection string, private
        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }

        public MoniDataBaseContext()
        {
            var objBuilder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            IConfiguration conManager = objBuilder.Build();

            var conn = conManager.GetConnectionString("myDb1");
            if (conn == null)
            {
            conn = "";
            }
            this.ConnectionString = conn;
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

        // Juego 3
        public List<LeaderJ3> GetLeaderJ3() 
        {
            List<LeaderJ3> ListaJ3 = new List<LeaderJ3>(); // List to store players
            MySqlConnection conexion = GetConnection(); // Initialize connection
            conexion.Open(); // Open connection

            string queryJ3 = @"
                SELECT 
                    u.id_usuario, 
                    u.id_plaza, 
                    u.nombre, 
                    u.apellido_pat, 
                    u.apellido_mat, 
                    SUM(j3.exp) AS total_exp
                FROM usuario u
                JOIN usuario_historial uh ON u.id_usuario = uh.id_usuario
                JOIN historial h ON uh.id_historial = h.id_historial
                JOIN juego3 j3 ON h.id_juego3 = j3.id_juego3
                GROUP BY u.id_usuario, u.id_plaza, u.nombre, u.apellido_pat, u.apellido_mat
                ORDER BY total_exp DESC;";

            MySqlCommand cmd = new MySqlCommand(queryJ3, conexion);

            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    LeaderJ3 ranked3 = new LeaderJ3();

                    // User Data
                    ranked3.id_usuario = Convert.ToInt32(reader["id_usuario"]);
                    ranked3.id_plaza = Convert.ToInt32(reader["id_plaza"]);
                    ranked3.nombre = reader["nombre"].ToString(); 
                    ranked3.apellido_pat = reader["apellido_pat"].ToString(); 
                    ranked3.apellido_mat = reader["apellido_mat"].ToString(); 

                    // Total Exp (SUM)
                    ranked3.total_exp = Convert.ToInt32(reader["total_exp"]); 

                    // Add to list
                    ListaJ3.Add(ranked3); 
                }
            }

            conexion.Close(); // Close connection
            return ListaJ3; // Return list
        }

        // General a partir de 4to lugar
        public List<LeaderGen4On> GetLeaderGen4On() 
        {
            List<LeaderGen4On> ListaGJ4 = new List<LeaderGen4On>(); // List to store players
            MySqlConnection conexion = GetConnection(); // Initialize connection
            conexion.Open(); // Open connection

            string queryJ4 = @"
            SELECT 
                u.id_usuario, 
                u.id_plaza,
                u.nombre, 
                u.apellido_pat, 
                u.apellido_mat, 
            COALESCE(SUM(j1.exp), 0) + COALESCE(SUM(j2.exp), 0) + COALESCE(SUM(j3.exp), 0) AS total_exp
            FROM usuario u
            JOIN usuario_historial uh ON u.id_usuario = uh.id_usuario
            JOIN historial h ON uh.id_historial = h.id_historial
            LEFT JOIN juego1 j1 ON h.id_juego1 = j1.id_juego1
            LEFT JOIN juego2 j2 ON h.id_juego2 = j2.id_juego2
            LEFT JOIN juego3 j3 ON h.id_juego3 = j3.id_juego3
            GROUP BY u.id_usuario, u.id_plaza, u.nombre, u.apellido_pat, u.apellido_mat
            ORDER BY total_exp DESC
            LIMIT 200 OFFSET 3;";

            MySqlCommand cmd = new MySqlCommand(queryJ4, conexion);

            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    LeaderGen4On ranked4 = new LeaderGen4On();

                    // User Data
                    ranked4.id_usuario = Convert.ToInt32(reader["id_usuario"]);
                    ranked4.id_plaza = Convert.ToInt32(reader["id_plaza"]);
                    ranked4.nombre = reader["nombre"].ToString(); 
                    ranked4.apellido_pat = reader["apellido_pat"].ToString(); 
                    ranked4.apellido_mat = reader["apellido_mat"].ToString(); 

                    // Total Exp (SUM)
                    ranked4.total_exp = Convert.ToInt32(reader["total_exp"]); 

                    // Add to list
                    ListaGJ4.Add(ranked4); 
                }
            }

            conexion.Close(); // Close connection
            return ListaGJ4; // Return list
        }

        // General 1er lugar
        public List<LeaderGen1> GetLeaderGen1() 
        {
            List<LeaderGen1> ListaGen1 = new List<LeaderGen1>(); // List to store players
            MySqlConnection conexion = GetConnection(); // Initialize connection
            conexion.Open(); // Open connection

            string queryG1 = @"
            SELECT 
                u.id_usuario, 
                u.id_plaza,
                u.nombre, 
                u.apellido_pat, 
                u.apellido_mat, 
                u.foto, 
            COALESCE(SUM(j1.exp), 0) + COALESCE(SUM(j2.exp), 0) + COALESCE(SUM(j3.exp), 0) AS total_exp
            FROM usuario u
            JOIN usuario_historial uh ON u.id_usuario = uh.id_usuario
            JOIN historial h ON uh.id_historial = h.id_historial
            LEFT JOIN juego1 j1 ON h.id_juego1 = j1.id_juego1
            LEFT JOIN juego2 j2 ON h.id_juego2 = j2.id_juego2
            LEFT JOIN juego3 j3 ON h.id_juego3 = j3.id_juego3
            GROUP BY u.id_usuario, u.id_plaza, u.nombre, u.apellido_pat, u.apellido_mat, u.foto
            ORDER BY total_exp DESC 
            LIMIT 1;";

            MySqlCommand cmd = new MySqlCommand(queryG1, conexion);

            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    LeaderGen1 lead1 = new LeaderGen1();

                    // User Data
                    lead1.id_usuario = Convert.ToInt32(reader["id_usuario"]);
                    lead1.id_plaza = Convert.ToInt32(reader["id_plaza"]);
                    lead1.nombre = reader["nombre"].ToString(); 
                    lead1.apellido_pat = reader["apellido_pat"].ToString(); 
                    lead1.apellido_mat = reader["apellido_mat"].ToString(); 
                    lead1.foto = reader["foto"].ToString();

                    // Total Exp (sum of three games)
                    lead1.total_exp = Convert.ToInt32(reader["total_exp"]);

                    // Add to list
                    ListaGen1.Add(lead1); 
                }
            }

            conexion.Close(); // Close connection
            return ListaGen1; // Return list
        }

        // General 2do lugar
        public List<LeaderGen2> GetLeaderGen2() 
        {
            List<LeaderGen2> ListaGen2 = new List<LeaderGen2>(); // List to store players
            MySqlConnection conexion = GetConnection(); // Initialize connection
            conexion.Open(); // Open connection

            string queryG2 = @"
            SELECT 
                u.id_usuario, 
                u.id_plaza,
                u.nombre, 
                u.apellido_pat, 
                u.apellido_mat, 
                u.foto, 
            COALESCE(SUM(j1.exp), 0) + COALESCE(SUM(j2.exp), 0) + COALESCE(SUM(j3.exp), 0) AS total_exp
            FROM usuario u
            JOIN usuario_historial uh ON u.id_usuario = uh.id_usuario
            JOIN historial h ON uh.id_historial = h.id_historial
            LEFT JOIN juego1 j1 ON h.id_juego1 = j1.id_juego1
            LEFT JOIN juego2 j2 ON h.id_juego2 = j2.id_juego2
            LEFT JOIN juego3 j3 ON h.id_juego3 = j3.id_juego3
            GROUP BY u.id_usuario, u.id_plaza, u.nombre, u.apellido_pat, u.apellido_mat, u.foto
            ORDER BY total_exp DESC 
            LIMIT 1 OFFSET 1;";

            MySqlCommand cmd = new MySqlCommand(queryG2, conexion);

            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    LeaderGen2 lead2 = new LeaderGen2();

                    // User Data
                    lead2.id_usuario = Convert.ToInt32(reader["id_usuario"]);
                    lead2.id_plaza = Convert.ToInt32(reader["id_plaza"]);
                    lead2.nombre = reader["nombre"].ToString(); 
                    lead2.apellido_pat = reader["apellido_pat"].ToString(); 
                    lead2.apellido_mat = reader["apellido_mat"].ToString(); 
                    lead2.foto = reader["foto"].ToString();

                    // Total Exp (sum of three games)
                    lead2.total_exp = Convert.ToInt32(reader["total_exp"]);

                    // Add to list
                    ListaGen2.Add(lead2); 
                }
            }

            conexion.Close(); // Close connection
            return ListaGen2; // Return list
        }

        // General 3er lugar
        public List<LeaderGen3> GetLeaderGen3() 
        {
            List<LeaderGen3> ListaGen3 = new List<LeaderGen3>(); // List to store players
            MySqlConnection conexion = GetConnection(); // Initialize connection
            conexion.Open(); // Open connection

            string queryG3 = @"
            SELECT 
                u.id_usuario, 
                u.id_plaza,
                u.nombre, 
                u.apellido_pat, 
                u.apellido_mat, 
                u.foto, 
            COALESCE(SUM(j1.exp), 0) + COALESCE(SUM(j2.exp), 0) + COALESCE(SUM(j3.exp), 0) AS total_exp
            FROM usuario u
            JOIN usuario_historial uh ON u.id_usuario = uh.id_usuario
            JOIN historial h ON uh.id_historial = h.id_historial
            LEFT JOIN juego1 j1 ON h.id_juego1 = j1.id_juego1
            LEFT JOIN juego2 j2 ON h.id_juego2 = j2.id_juego2
            LEFT JOIN juego3 j3 ON h.id_juego3 = j3.id_juego3
            GROUP BY u.id_usuario, u.id_plaza, u.nombre, u.apellido_pat, u.apellido_mat, u.foto
            ORDER BY total_exp DESC 
            LIMIT 1 OFFSET 2;";

            MySqlCommand cmd = new MySqlCommand(queryG3, conexion);

            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    LeaderGen3 lead3 = new LeaderGen3();

                    // User Data
                    lead3.id_usuario = Convert.ToInt32(reader["id_usuario"]);
                    lead3.id_plaza = Convert.ToInt32(reader["id_plaza"]);
                    lead3.nombre = reader["nombre"].ToString(); 
                    lead3.apellido_pat = reader["apellido_pat"].ToString(); 
                    lead3.apellido_mat = reader["apellido_mat"].ToString(); 
                    lead3.foto = reader["foto"].ToString();

                    // Total Exp (sum of three games)
                    lead3.total_exp = Convert.ToInt32(reader["total_exp"]);

                    // Add to list
                    ListaGen3.Add(lead3); 
                }
            }

            conexion.Close(); // Close connection
            return ListaGen3; // Return list
        }
    }

    
}


    

