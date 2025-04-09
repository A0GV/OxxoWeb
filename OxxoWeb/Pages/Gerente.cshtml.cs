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

        public Dictionary<int, string> FotosDrive { get; set; } = new();


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

            // Asigna imágenes de Drive por ID de usuario
            FotosDrive = new Dictionary<int, string>
            {
                { 7, "https://tecmx-my.sharepoint.com/:i:/g/personal/a00840840_tec_mx/EU6ia9sgdb5Dhts-MFPets4B_WVekPJn3UMY-67WfpxyGQ?e=c4qI3V" },
                { 8, "https://tecmx-my.sharepoint.com/:i:/r/personal/a00840840_tec_mx/Documents/ASESORES%20PFP/asesor8.png?csf=1&web=1&e=aiOQy6"},
                { 9, "https://tecmx-my.sharepoint.com/:i:/r/personal/a00840840_tec_mx/Documents/ASESORES%20PFP/asesor9.png?csf=1&web=1&e=KqBPxn" },
                
            // Agrega los 16 aquí...
            };
        }


    }
}
