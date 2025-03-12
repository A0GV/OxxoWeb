using System; // Para poder hacer conexiones
using MySql.Data.MySqlClient; // Agregar MySQL, need to do desde NuGet
using System.Collections.Generic; 

namespace OxxoWeb.Model 
{
    public class DataBaseContext
    {
            public string ConnectionString { get; set; } // String de conexión

            // Constructor para hacer la conexión (despues de deal con NuGet)
            public DataBaseContext()
            {
                // Running on localhost port, using database default MySQL 3306, Uid root, and password mod
                ConnectionString = "Server=127.0.0.1;Port=3306;Database=oxxo_base_e1_2;Uid=root;password=80mB*%7aEf;";
            } 

            // Returns connection with connection string, private
            private MySqlConnection GetConnection()
            {
                return new MySqlConnection(ConnectionString);
            }

            /*
            public List<Usuarios> GetAllUsers()
            {
                List<Usuarios> ListaUsuarios = new List<Usuarios>(); // Empty user list
                MySqlConnection conexion = GetConnection(); // Initialize connection w previous method
                conexion.Open(); // Opens connection
                MySqlCommand cmd = new MySqlCommand("Select * from Usuarios", conexion); // SQL query to conexion

                Usuarios usr1 = new Usuarios(); // Instancia de usuario vacío

                // Instruccion ejecuta command y lo guarda en reader
                using (var reader = cmd.ExecuteReader())
                {
                    // Returns a row as long as can read smth and returns false when done
                    while (reader.Read())
                    {
                        usr1 = new Usuarios(); // Gets el id de usuario
                        // Query de aqui = query dbeaver (nombre de columna)
                        // Si hacemos un join necesitamos incluir un AS para tener el nombre y que sí aparezca
                        usr1.idUsuario = Convert.ToInt32(reader["IdUsuario"]); // reader y el nombre de lo q tiene q obtener, como lo vemos en el query de dbeaver (no tienen q ser igual)
                        usr1.Nombre = reader["Nombre"].ToString();
                        usr1.Edad = Convert.ToInt32(reader["Edad"]);
                        ListaUsuarios.Add(usr1); // Agrega completo a lista de usuarios
                    }
                }
                conexion.Close();
                return ListaUsuarios; // Regresa lista
            }
            */
    }
}


    

