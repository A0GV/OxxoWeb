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
                int userSessionId = GetUserId(Username); 
                if (userSessionId != 0)
                {
                    HttpContext.Session.SetInt32("Id", userSessionId); 
                }
                return RedirectToPage("/Home");
            }
            else
            {
                ErrorMessage = "El usuario o contraseña son incorrectos";
                return Page();
            }
        }

        private int GetUserId(string username)
        {
            using (var connection = new MySql.Data.MySqlClient.MySqlConnection("Server=127.0.0.1;Port=3306;Database=oxxo_base_e2_1;Uid=root;password=root"))
            {
                connection.Open();
                string query = "SELECT id_usuario FROM usuario WHERE correo = @username LIMIT 1;";
                using (var command = new MySql.Data.MySqlClient.MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@username", username);
                    var result = command.ExecuteScalar();
                    return result != null ? Convert.ToInt32(result) : 0;
                }
            }
        }
    }
}
