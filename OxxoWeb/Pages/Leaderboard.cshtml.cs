using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OxxoWeb.Models; // Busca en carpeta de Models

namespace OxxoWeb.Pages;

public class LeaderboardModel : PageModel
{
    private readonly MoniDataBaseContext _context; // Can only read my context

    public List<Plazas> listaZonas1 {get; set;} // Lista de zonas
    public List<LeaderJ1> listaJuego1 {get; set;} // Lista de leaderboard para juego 1 general
    public List<LeaderJ2> listaJuego2 {get; set;} // Lista de leaderboard para juego 2 general
    public List<LeaderJ3> listaJuego3 {get; set;} // Lista de leaderboard para juego 3 general
    public List<LeaderGen4On> listaJuego4 {get; set;} // Lista de leaderboard para general de lugares 4 en adelante

    public List<LeaderGen1> listaGen1 {get; set;} // Lista de general en lugar 1
    public List<LeaderGen2> listaGen2 {get; set;} // Lista de general en lugar 2
    public List<LeaderGen3> listaGen3 {get; set;} // Lista de general en lugar 3
    

    // Para incluir el servicio
    public LeaderboardModel(MoniDataBaseContext context)
    {
        _context = context;

        // Inicializar para poder handle null pointer exceptions de sesión no iniciada
        listaZonas1 = new List<Plazas>();
        listaJuego1 = new List<LeaderJ1>();
        listaJuego2 = new List<LeaderJ2>();
        listaJuego3 = new List<LeaderJ3>();
        listaJuego4 = new List<LeaderGen4On>();
        listaGen1 = new List<LeaderGen1>();
        listaGen2 = new List<LeaderGen2>();
        listaGen3 = new List<LeaderGen3>();
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
            // Trae lugares jugadores individuales
            listaJuego1 = _context.GetLeaderJ1();
            listaJuego2 = _context.GetLeaderJ2();
            listaJuego3 = _context.GetLeaderJ3();
            // Para ranking general
            listaJuego4 = _context.GetLeaderGen4On();
            listaGen1 = _context.GetLeaderGen1(); 
            listaGen2 = _context.GetLeaderGen2(); 
            listaGen3 = _context.GetLeaderGen3();
            // Lista de zonas
            listaZonas1 = _context.GetAllPlazas(); // Va a capa model y trae todos las plazas
            //nick = HttpContext.Session.GetString("nickname");
        }
    }
}