namespace OxxoWeb.Models
{
    public class Tienda
    {
        public int id_tienda { get; set; }
        public int id_plaza { get; set; }
        public string num_calle { get; set; } = "";       // ej. "014"
        public string nombre_calle { get; set; } = "";    // ej. "Avenida Reforma"
        public string horas { get; set; } = "";           // ej. "06:30-21:30"
        public int num_empleados { get; set; }
        public DateTime fecha_inicio { get; set; }
        public int estatus { get; set; }

        public Tienda() { }
    }
}
