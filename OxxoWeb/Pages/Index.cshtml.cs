using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OxxoWeb.Pages;

public class IndexModel : PageModel
{
    // private readonly ILogger<IndexModel> _logger;

    // public IndexModel(ILogger<IndexModel> logger)
    // {
    //     _logger = logger;
    // }
    [BindProperty]
    public string Username { get; set; }

    [BindProperty]
    public string Password { get; set; }

    public void OnGet()
    {
        
    }

    public IActionResult OnPost()
    {
        var context = new OxxoWeb.Models.AdolfoDatabaseContext();
        bool isValid = context.CheckCredentials(Username, Password);
        Console.WriteLine("El valor de valid es:", isValid);
        Console.WriteLine("El valor de USer es:", Username);
        Console.WriteLine("El valor de Pswrsd es:", Password);

        if (isValid)
        {
            // ...handle successful login...
            return RedirectToPage("/Home"); 
        }
        else
        {
            // ...handle failed login...
            return Page();
        }
    }
}
