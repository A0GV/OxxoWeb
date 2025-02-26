using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OxxoWeb.Pages;

public class PerfilModel : PageModel
{
    [BindProperty]
    public string Nombre { get; set;} = string.Empty;
    [BindProperty]
    public int? Edad { get; set;}
    public bool hasError { get; set;}
    public void OnGet()
    {
        hasError = false;
        Nombre = "the starwalker";
        ViewData["Mensaje"] = "Kris, where are we?";
        ViewData["Fecha"] = DateTime.Now.ToString("dd/MM/yyyy");
    }

        public void OnPost()
    {
        if (Edad == null){
            ViewData["Mensaje"] = "Error";
            hasError = true;
        }else if(Nombre=="Kris"){
            Response.Redirect("Pagina2?Nombre="+Nombre+"&Edad="+Edad);
        }else{
            ViewData["Mensaje"] = "Dark, so dark...:";
            hasError = false;
        }
    }
}
