using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace OxxoWeb.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public string ErrorMessage { get; set; } // Propiedad para el mensaje de error

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            var context = new OxxoWeb.Models.AdolfoDatabaseContext();
            var context2 = new OxxoWeb.Models.ReginaDataBaseContext();

            int tipoUsuario = context2.GetTipoUsuario(Username);

            bool isValid = context.CheckCredentials(Username, Password);

            if (isValid)
            {
                int UserSessionId = context.GetUserId(Username);
                if (UserSessionId !=0)
                {
                    HttpContext.Session.SetInt32("Id", UserSessionId);
                    //maybe?
                    HttpContext.Session.SetInt32("TipoUsuario", tipoUsuario);
                };

                
                if (tipoUsuario == 2){
                    return RedirectToPage("/Gerente");
                    }
                    else
                    {
                        return RedirectToPage("/Home");
                    }

            }
            else
            {
                ErrorMessage = "El usuario o contraseña son incorrectos"; // Establece el mensaje de error
                return Page();
            }
        }
    }
}
