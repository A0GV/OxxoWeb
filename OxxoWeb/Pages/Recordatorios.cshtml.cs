using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OxxoWeb.Models;

namespace OxxoWeb.Pages;

public class RecordatoriosModel : PageModel
{
    private readonly RecordatoriosDataBaseContext context;
    public List<Categoria> categorias { get; set; }
    public List<Recordatorio> recc { get; set; }
    public Dictionary<int, string> colores = new Dictionary<int, string>{
        {1,"morado"},{2,"amarillo"},{3,"rosa"},{0,"verde"}
    };
    public Dictionary<DateOnly, bool> Checkfecha = new Dictionary<DateOnly, bool>();

    // Properties to match the fields used in the Razor page
    public string titulo { get; set; }
    public string descripcion { get; set; }
    public int id_tienda { get; set; }
    public DateOnly fecha { get; set; }
    public TimeSpan hora_inicio { get; set; }
    public TimeSpan hora_fin { get; set; }
    public string lugar {get;set;}
    
    public RecordatoriosModel(RecordatoriosDataBaseContext _context)
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

    //Tarea asyncrona de borrar
    public async Task<IActionResult> OnPostEliminar()
    {
        //Request.For para traerse el dato del input que es string y lo pasa a entero
        int id_recordatorio = int.Parse(Request.Form["id_recordatorio"]);
        //Task.Run espera a terminar esta tarea para después proceder, lo ejecuta en un subproceso separado.
        await Task.Run(() => context.EliminarRec(id_recordatorio));
        //Recargar pagina
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostAgregar()
    {
        int id_tipo = int.Parse(Request.Form["id_tienda"]) == 0 ? 1 : 2;
        string descripcion = Request.Form["descripcion"].ToString() ?? string.Empty;

        // Asegúrate de que la fecha se procese correctamente
        string fechaInput = Request.Form["fecha"];
        fecha = DateOnly.ParseExact(fechaInput, "yyyy-MM-dd");

        // Insertar el nuevo recordatorio en la base de datos
        await Task.Run(() => context.AgregarRec(
            HttpContext.Session.GetInt32("Id").Value, 
            Request.Form["titulo"], 
            fecha, 
            Request.Form["lugar"], 
            descripcion, 
            TimeSpan.Parse(Request.Form["hora_inicio"]), 
            TimeSpan.Parse(Request.Form["hora_fin"]), 
            int.TryParse(Request.Form["id_tienda"], out int idTienda) && idTienda != 0 ? idTienda : (int?)null, 
            id_tipo)
        );
        
        return RedirectToPage();
    }
    public async Task<IActionResult> OnPostModificar()
    {
        int id_recordatorio = int.Parse(Request.Form["id_recordatorio"]);
        int id_tipo = int.Parse(Request.Form["id_tienda"]) == 0 ? 1 : 2;
        string descripcion = Request.Form["descripcion"].ToString() ?? string.Empty;

        // Asegúrate de que la fecha se procese correctamente
        string fechaInput = Request.Form["fecha"];
        fecha = DateOnly.ParseExact(fechaInput, "yyyy-MM-dd");

        // Insertar el nuevo recordatorio en la base de datos
        await Task.Run(() => context.ModificarRec(
            id_recordatorio, 
            Request.Form["titulo"], 
            fecha, 
            Request.Form["lugar"], 
            descripcion, 
            TimeSpan.Parse(Request.Form["hora_inicio"]), 
            TimeSpan.Parse(Request.Form["hora_fin"]), 
            int.TryParse(Request.Form["id_tienda"], out int idTienda) && idTienda != 0 ? idTienda : (int?)null, 
            id_tipo)
        );
        return RedirectToPage();
    }
}