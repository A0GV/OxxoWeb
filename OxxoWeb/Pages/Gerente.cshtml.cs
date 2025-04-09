using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OxxoWeb.Models;
using System.Collections.Generic;

namespace OxxoWeb.Pages
{
    public class GerenteModel : PageModel
    {
    
        public List<AsesorInfo> ListaAsesores { get; set; } = new();
        public Dictionary<int, int> tiendasPorAsesor { get; set; } = new();



        public void OnGet(){
            ReginaDataBaseContext db = new ReginaDataBaseContext();
            ListaAsesores = db.GetAsesoresInfo();
            tiendasPorAsesor = db.GetTiendasNum();

            // Asignar el count a cada asesor
            foreach (var asesor in ListaAsesores)
            {
                if (tiendasPorAsesor.TryGetValue(asesor.IdUsuario, out int count))
                {
                    asesor.TiendasCount = count;
                }
                else
                {
                    asesor.TiendasCount = 0;
                }
            }

            
        }


    }
}
