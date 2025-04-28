using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Org.BouncyCastle.Bcpg;
using OxxoWeb.Models;

namespace OxxoWeb.Pages;

public class HomeModel : PageModel
{
    private readonly RecordatoriosDataBaseContext context;
    public List<Categoria> categorias { get; set; }
    public List<Recordatorio> recc { get; set; }
    public Dictionary<int, string> colores = new Dictionary<int, string>{
        {1,"morado"},{2,"amarillo"},{3,"rosa"},{0,"verde"}
    };
    //Diccionario para no repetir titulo de fecha
    public Dictionary<DateOnly, bool> Checkfecha = new Dictionary<DateOnly, bool>();
    public bool Checkfooter = false;
    public HomeModel(RecordatoriosDataBaseContext _context)
    {
        context = _context;
        //Inicializar
        categorias = new List<Categoria>();
        recc = new List<Recordatorio>();
    }
    public void OnGet()
    {
        // Checks if it has a user ID stored as int
        int? userId = HttpContext.Session.GetInt32("Id");
        if (userId == null)
        {
            // Takes user to login page
            Response.Redirect("/Index");
        }
        else
        {
            //Obtiene las categorías de tienda
            categorias = context.GetCategorias(userId.Value);
            //Asigna un color al boton de las categorías
            categorias.ForEach(categoria => categoria.color = colores[categoria.id_tienda % 4]);
            //Obtiene los recordatorios
            recc = context.GetRecordatorios(userId.Value);

            //Guarda la fechas de los recordatorios y si no esta en la lista las agrega
            foreach (var recordatorio in recc)
            {
                if (!Checkfecha.ContainsKey(recordatorio.fecha))
                {
                    Checkfecha.Add(recordatorio.fecha, false);
                }
            }
        }
    }
}
