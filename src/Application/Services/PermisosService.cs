using Application.DTOs.Filters;
using Application.DTOs.Responses;
using Application.Interfaces;
using Application.Interfaces.Common;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;
using static Domain.Entities.Accion;
using Utilities;

using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Application.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class PermisosService
    {
        /// <summary>
        /// Contructor
        /// </summary>
        public PermisosService() {
        }

        public List<MenuOption> ObtenerOpcionesMenu(string rol)
        {
            var opciones = new List<MenuOption>();
            switch (rol)
            {
                case Rol.Roles.Manager:
                    opciones = new List<MenuOption>
                    {
                        new MenuOption { Nombre = "Dashboard", Nivel = 1, Path = "/dashboard" },
                        new MenuOption { Nombre = "Informes", Nivel = 1, Path = "/informes" }
                    };
                    break;

                case Rol.Roles.Admin:
                    opciones = new List<MenuOption>
                    {
                        new MenuOption { Nombre = "Dashboard", Nivel = 1, Path = "/dashboard" },
                        new MenuOption { Nombre = "Administración", Nivel = 1, Path = "/admin" },
                        new MenuOption { Nombre = "Usuarios", Nivel = 2, Path = "/admin/usuarios" }
                    };
                    break;
            }

            return opciones;
        }
    }
}