using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OxxoWeb.Models;
using System.Collections.Generic;

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

    public void OnGet(string? nombre)
    {
        if (!string.IsNullOrEmpty(nombre))
        {
            // Buscar el perfil por nombre
            UsuarioPerfil = _context.GetPerfilPorNombre(nombre);

            if (UsuarioPerfil == null)
            {
                // Si no se encuentra el perfil, redirigir a la página principal
                Response.Redirect("/"); // O mostrar un mensaje de error si prefieres
                return;
            }

            // Cargar estadísticas, publicaciones y certificados del usuario encontrado
            UsuarioEstadisticas = _context.GetEstadisticas(UsuarioPerfil.IdUsuario);
            UsuarioPublicaciones = _context.GetPublicaciones(UsuarioPerfil.IdUsuario);
            UsuarioCertificados = _context.GetCertificados(UsuarioPerfil.IdUsuario);
        }
        else
        {
            // Si no hay búsqueda, cargar el perfil del usuario actual
            int? idUsuario = HttpContext.Session.GetInt32("Id");
            if (idUsuario == null)
            {
                Response.Redirect("/Index");
                return;
            }

            UsuarioPerfil = _context.GetPerfil(idUsuario.Value);
            UsuarioEstadisticas = _context.GetEstadisticas(idUsuario.Value);
            UsuarioPublicaciones = _context.GetPublicaciones(idUsuario.Value);
            UsuarioCertificados = _context.GetCertificados(idUsuario.Value);
        }
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

