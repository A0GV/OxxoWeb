using System; // Para poder hacer conexiones
using MySql.Data.MySqlClient; // Agregar MySQL, need to do desde NuGet
using System.Collections.Generic; 

namespace OxxoWeb.Models {
    public class RecordatoriosDataBaseContext
    {
        public string ConnectionString { get; set; }

        public RecordatoriosDataBaseContext()
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

        public List<Categoria> GetCategorias(int idUSuario){
            List<Categoria> categorias = new List<Categoria>();
            MySqlConnection conexion = GetConnection();
            conexion.Open();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conexion;
            cmd.Parameters.AddWithValue("@id", idUSuario);
            cmd.CommandText = @"select ut.id_tienda, t.nombre_calle from usuario u
                            join usuario_tienda ut on ut.id_usuario = u.id_usuario
                            join tienda t on t.id_tienda = ut.id_tienda
                            where u.id_usuario = @id;";
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    Categoria cat = new Categoria(
                        reader.GetInt32("id_tienda"),
                        reader.GetString("nombre_calle")
                    );
                    categorias.Add(cat);
                }
            }                
            cmd.Prepare();
            cmd.ExecuteNonQuery();
            conexion.Close();
            return categorias;
        }
    }
}