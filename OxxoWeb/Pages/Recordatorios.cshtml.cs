using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OxxoWeb.Models;
using ZstdSharp.Unsafe; // Busca en carpeta de Models

namespace OxxoWeb.Pages;

public class RecordatoriosModel : PageModel
{
    private readonly RecordatoriosDataBaseContext context;
    public List<Categoria> categorias {get;set;}
    public List<Recordatorio> recc {get;set;}
    public Dictionary<int,string> colores = new Dictionary<int, string>{
        {1,"morado"},{2,"amarillo"},{3,"rosa"},{0,"verde"}
    };
    public Dictionary<DateOnly, bool> Checkfecha = new Dictionary<DateOnly, bool>();

    public RecordatoriosModel(RecordatoriosDataBaseContext _context){
        context = _context;
        //Inicializar
        categorias = new List<Categoria>();
        recc = new List<Recordatorio>();
    }
    public void OnGet(){
        // Checks if it has a user ID stored as int
        int? userId = HttpContext.Session.GetInt32("Id");
        if (userId == null)
        {
            // Takes user to login page
            Response.Redirect("/Index");
        } else{
            //Obtiene las categorías de tienda
            categorias = context.GetCategorias(userId.Value);
            categorias.ForEach(categoria => categoria.color = colores[categoria.id_tienda % 4]);
            recc = context.GetRecordatorios(userId.Value);
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