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
            bool isValid = context.CheckCredentials(Username, Password);

            if (isValid)
            {
                int UserSessionId = context.GetUserId(Username);
                if (UserSessionId !=0)
                {
                    HttpContext.Session.SetInt32("Id", UserSessionId);
                };
                return RedirectToPage("/Home");
            }
            else
            {
                ErrorMessage = "El usuario o contraseña son incorrectos"; // Establece el mensaje de error
                return Page();
            }
        }
    }
}