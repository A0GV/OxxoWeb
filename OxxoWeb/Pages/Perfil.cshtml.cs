using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OxxoWeb.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace OxxoWeb.Pages;

public class PerfilModel : PageModel
{
    private readonly DataBaseContextPerfil _context;
    public Perfiles UsuarioPerfil { get; set; }
    public Estadisticas UsuarioEstadisticas { get; set; }
    public List<Publicacion> UsuarioPublicaciones { get; set; }
    public List<Cerificados> UsuarioCertificados { get; set; }

    public PerfilModel(DataBaseContextPerfil context)
    {
        _context = context;
    }

    public void OnGet(int? idUsuario)
    {
        // Si no se proporciona un idUsuario, usar el ID del usuario autenticado
        if (!idUsuario.HasValue)
        {
            idUsuario = HttpContext.Session.GetInt32("Id"); // Recuperar el ID desde la sesión

            if (!idUsuario.HasValue)
            {
                string mensaje = Uri.EscapeDataString("Debe iniciar sesión para acceder a su perfil");
                Response.Redirect($"/Index?message={mensaje}");
                return;
            }
        }

        // Cargar el perfil del usuario especificado
        UsuarioPerfil = _context.GetPerfil(idUsuario.Value);

        if (UsuarioPerfil == null)
        {
            string mensaje = Uri.EscapeDataString("Perfil no encontrado");
            Response.Redirect($"/Error?message={mensaje}");
            return;
        }

        UsuarioEstadisticas = _context.GetEstadisticas(UsuarioPerfil.IdUsuario) ?? new Estadisticas();
        UsuarioPublicaciones = _context.GetPublicaciones(UsuarioPerfil.IdUsuario) ?? new List<Publicacion>();
        UsuarioCertificados = _context.GetCertificados(UsuarioPerfil.IdUsuario) ?? new List<Cerificados>();
    }

    // Método para convertir la letra del tipo en descripción
    public string ObtenerDescripcion(string tipo)
    {
        return tipo switch
        {
            "a" => "Asesor de tienda",
            "g" => "Gerente",
            "o" => "Otro empleo",
            _ => "Desconocido"
        };
    }

}

