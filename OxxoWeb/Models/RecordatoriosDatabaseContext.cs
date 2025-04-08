using System; // Para poder hacer conexiones
using MySql.Data.MySqlClient; // Agregar MySQL, need to do desde NuGet
using System.Collections.Generic;
using System.ComponentModel;

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
            cmd.CommandText = @"select r.titulo, r.dia, r.lugar, r.descripcion, r.hora_inicio, r.hora_final, tr.tipo, r.id_tienda, r.id_recordatorio
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
                        id_tienda = reader.IsDBNull(reader.GetOrdinal("id_tienda")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("id_tienda")),
                        id_recordatorio=reader.GetInt32("id_recordatorio")
                    };
                    recordatorioss.Add(rec);
                }
            }
            cmd.Prepare();
            cmd.ExecuteNonQuery();
            conexion.Close();
            return recordatorioss;
        }

        public void EliminarRec(int id_reco){
            MySqlConnection conexion = GetConnection();
            conexion.Open();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conexion;
            cmd.Parameters.AddWithValue("@id_rec", id_reco);
            cmd.CommandText = @"delete from recordatorio where id_recordatorio = @id_rec;";
            cmd.Prepare();
            cmd.ExecuteNonQuery();
            conexion.Close();
        }

        public void AgregarRec(int id_usu, string titu, DateOnly diaa, string lug,string desc, TimeSpan hi, TimeSpan hf, int? id_tien, int idtip){
            MySqlConnection conexion = GetConnection();
            conexion.Open();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conexion;
            cmd.Parameters.AddWithValue("@idus", id_usu);
            cmd.Parameters.AddWithValue("@tit", titu);
            cmd.Parameters.AddWithValue("@day", diaa.ToString("yyyy-MM-dd"));
            cmd.Parameters.AddWithValue("@place", lug);
            cmd.Parameters.AddWithValue("@des", desc);
            cmd.Parameters.AddWithValue("@hi", hi);
            cmd.Parameters.AddWithValue("@hf", hf);
            cmd.Parameters.AddWithValue("@idtipo", idtip);
            //Set null cuado sea 0 el valor, DBNull combierte el valor a nulo
            cmd.Parameters.AddWithValue("@idtienda", id_tien == 0 ? (object)DBNull.Value : id_tien);
            cmd.CommandText = @"insert into recordatorio (id_usuario, titulo, dia, lugar, descripcion, hora_inicio, hora_final, id_tipo, id_tienda) values
                                (@idus, @tit, @day, @place, @des, @hi, @hf, @idtipo, @idtienda);";
            cmd.Prepare();
            cmd.ExecuteNonQuery();
            conexion.Close();
        }

        public void ModificarRec(int id_reco, string titu, DateOnly diaa, string lug,string desc, TimeSpan hi, TimeSpan hf, int? id_tien, int idtip){
            MySqlConnection conexion = GetConnection();
            conexion.Open();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conexion;
            cmd.Parameters.AddWithValue("@id_rec", id_reco);
            cmd.Parameters.AddWithValue("@tit", titu);
            cmd.Parameters.AddWithValue("@day", diaa.ToString("yyyy-MM-dd"));
            cmd.Parameters.AddWithValue("@place", lug);
            cmd.Parameters.AddWithValue("@des", desc);
            cmd.Parameters.AddWithValue("@hi", hi);
            cmd.Parameters.AddWithValue("@hf", hf);
            cmd.Parameters.AddWithValue("@idtipo", idtip);
            //Set null cuado sea 0 el valor, DBNull combierte el valor a nulo
            cmd.Parameters.AddWithValue("@idtienda", id_tien == 0 ? (object)DBNull.Value : id_tien);
            cmd.CommandText = @"UPDATE recordatorio
                                SET titulo=@tit, dia = @day, lugar = @place, descripcion = @des, hora_inicio = @hi, hora_final = @hf, id_tipo = @idtipo, id_tienda = @idtienda
                                WHERE id_recordatorio=@id_rec;";
            cmd.Prepare();
            cmd.ExecuteNonQuery();
            conexion.Close();
        }
    }
}