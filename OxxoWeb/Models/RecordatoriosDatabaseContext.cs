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
    }
}