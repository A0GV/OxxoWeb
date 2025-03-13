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

    public void OnGet(int idUsuario)
    {
        UsuarioPerfil = _context.GetPerfil(idUsuario);
        UsuarioEstadisticas = _context.GetEstadisticas(idUsuario);
        UsuarioPublicaciones = _context.GetPublicaciones(idUsuario);
        UsuarioCertificados = _context.GetCertificados(idUsuario);
    }

        // 📌 Método para convertir la letra del tipo en descripción
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

