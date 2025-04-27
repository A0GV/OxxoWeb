using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OxxoWeb.Models;

namespace OxxoWeb.Pages;

public class DashboardModel : PageModel
{
    private readonly DataBaseContextPerfil _context; // Para poder usar ese database context para el tipo de usuario
    public Perfiles UsuarioPerfil { get; set; } // Obtener el tipo de sesión del usuario

    // Incluir el model en el servicio 
    public DashboardModel(DataBaseContextPerfil context)
    {
        _context = context;
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
            // Cargar el perfil del usuario especificado
            UsuarioPerfil = _context.GetPerfil(userId.Value);

            

        }
    }
}