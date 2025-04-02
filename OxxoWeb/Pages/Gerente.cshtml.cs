using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OxxoWeb.Models;
using System.Collections.Generic;

namespace OxxoWeb.Pages
{
    public class GerenteModel : PageModel
    {
    
        public List<AsesorInfo> ListaAsesores { get; set; } = new();

        public void OnGet()
        {
            ReginaDataBaseContext db = new ReginaDataBaseContext();
            ListaAsesores = db.GetAsesoresConInfo();
        }
    }
}
