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


        // Obtener por zona
    }
}


    

