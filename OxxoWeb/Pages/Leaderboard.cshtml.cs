using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OxxoWeb.Models; // Busca en carpeta de Models

namespace OxxoWeb.Pages;

public class LeaderboardModel : PageModel
{
    private readonly MoniDataBaseContext _context; // Can only read my context

    public List<Plazas> listaZonas1 {get; set;} // Lista de usuarios

    // Para incluir el servicio
    public LeaderboardModel(MoniDataBaseContext context)
    {
        _context = context;
    }
    

    public void OnGet() 
    {
        listaZonas1 = _context.GetAllPlazas(); // Va a capa model y trae todos las plazas
    }
}