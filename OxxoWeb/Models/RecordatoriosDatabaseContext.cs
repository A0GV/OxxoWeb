using System; // Para poder hacer conexiones
using MySql.Data.MySqlClient; // Agregar MySQL, need to do desde NuGet
using System.Collections.Generic;

namespace OxxoWeb.Models
{
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

        public List<Categoria> GetCategorias(int idUSuario)
        {
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

        public List<Recordatorio> GetRecordatorios(int idUSuario)
        {
            List<Recordatorio> recordatorioss = new List<Recordatorio>();
            MySqlConnection conexion = GetConnection();
            conexion.Open();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conexion;
            cmd.Parameters.AddWithValue("@id", idUSuario);
            cmd.CommandText = @"select r.titulo, r.dia, r.lugar, r.descripcion, r.hora_inicio, r.hora_final, tr.tipo, r.id_tienda
                                from recordatorio r
                                join tiporec tr on tr.id_tipo =r.id_tipo
                                where r.id_usuario = 6 and r.dia >= CURRENT_DATE
                                order by r.dia >= CURRENT_DATE DESC, r.dia, r.hora_inicio;";
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    Recordatorio rec = new Recordatorio
                    {
                        titulo = reader.GetString("titulo"),
                        lugar = reader.GetString("lugar"),
                        fecha = DateOnly.FromDateTime(reader.GetDateTime("dia")),
                        hora_inicio = reader.GetTimeSpan("hora_inicio"),
                        hora_final = reader.GetTimeSpan("hora_final"),
                        descripcion = reader.IsDBNull(reader.GetOrdinal("descripcion")) ? null : reader.GetString(reader.GetOrdinal("descripcion")),
                        tipo = reader.GetChar("tipo"),
                        id_tienda = reader.IsDBNull(reader.GetOrdinal("id_tienda")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("id_tienda"))
                    };
                    recordatorioss.Add(rec);
                }
            }
            cmd.Prepare();
            cmd.ExecuteNonQuery();
            conexion.Close();
            return recordatorioss;
        }
    }
}