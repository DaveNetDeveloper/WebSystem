using Application.DTOs.Filters;
using Application.DTOs.Responses;
using Application.Interfaces;
using Application.Interfaces.Common;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using DocumentFormat.OpenXml.Spreadsheet;
using Domain.Entities;
using Microsoft.Extensions.Options;
using System.Text.Json;
using Utilities;
using static Domain.Entities.Accion;

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
        public PermisosService()
        {

        }

        public List<MenuOption> ObtenerOpcionesMenu(string rol)
        {
            var opciones = new List<MenuOption>();
            switch (rol)
            {
                case string s when s.Equals(Rol.Roles.Manager, StringComparison.OrdinalIgnoreCase):

                    opciones.AddRange(CargarOpcionesMenu(MenuOpciones.Home));
                    opciones.AddRange(CargarOpcionesMenu(MenuOpciones.Administracion));
                    opciones.AddRange(CargarOpcionesMenu(MenuOpciones.Informes));
                    break;

                case string s when s.Equals(Rol.Roles.Admin, StringComparison.OrdinalIgnoreCase):

                    opciones.AddRange(CargarOpcionesMenu(MenuOpciones.Home));
                    opciones.AddRange(CargarOpcionesMenu(MenuOpciones.Administracion));
                    opciones.AddRange(CargarOpcionesMenu(MenuOpciones.Configuracion));
                    opciones.AddRange(CargarOpcionesMenu(MenuOpciones.Marketing));
                    opciones.AddRange(CargarOpcionesMenu(MenuOpciones.Informes));
                    opciones.AddRange(CargarOpcionesMenu(MenuOpciones.BaseDatos));
                    break;
            }
            return opciones;
        }

        public static class MenuOpciones
        {
            public const string Home = "Home";
            public const string Administracion = "Administración";
            public const string Marketing = "Marketing";
            public const string BaseDatos = "Base de Datos";
            public const string Configuracion = "Configuración";
            public const string Informes = "Informes";
        }

        private List<MenuOption> CargarOpcionesMenu(string menu)
        {
            var opciones = new List<MenuOption>();

            switch (menu)
            {
                case MenuOpciones.Home:
                    opciones.Add(new MenuOption { Nombre = "Home", Nivel = 1, Path = "#" });
                    break;

                case MenuOpciones.Administracion:
                    opciones.Add(new MenuOption { Nombre = "Administración", Nivel = 1, Path = "#" });
                    opciones.Add(new MenuOption { Nombre = "Consultas de Usuarios", Parent = "Administración", Nivel = 2, Path = "./admin/consultas-list.html" });
                    opciones.Add(new MenuOption { Nombre = "Entidades", Parent = "Administración", Nivel = 2, Path = "./admin/entidades-list.html" });
                    opciones.Add(new MenuOption { Nombre = "Categorias", Parent = "Administración", Nivel = 2, Path = "./admin/categorias-list.html" });
                    opciones.Add(new MenuOption { Nombre = "Usuarios", Parent = "Administración", Nivel = 2, Path = "./admin/usuarios-list.html" });
                    opciones.Add(new MenuOption { Nombre = "Transacciones", Parent = "Administración", Nivel = 2, Path = "./admin/transacciones-list.html" });
                    opciones.Add(new MenuOption { Nombre = "Recompensas", Parent = "Administración", Nivel = 2, Path = "./admin/recompensas-list.html" });
                    opciones.Add(new MenuOption { Nombre = "Productos", Parent = "Administración", Nivel = 2, Path = "./admin/productos-list.html" });
                    opciones.Add(new MenuOption { Nombre = "Actividades & Reservas", Parent = "Administración", Nivel = 2, Path = "./admin/actividades-list.html" });
                    opciones.Add(new MenuOption { Nombre = "QRs", Parent = "Administración", Nivel = 2, Path = "./admin/qrs-list.html" });
                    opciones.Add(new MenuOption { Nombre = "Recursos", Parent = "Administración", Nivel = 2, Path = "./admin/recursos-list.html" });
                    opciones.Add(new MenuOption { Nombre = "FAQS", Parent = "Administración", Nivel = 2, Path = "./admin/faqs-list.html" });
                    opciones.Add(new MenuOption { Nombre = "Testimonios", Parent = "Administración", Nivel = 2, Path = "./admin/testimonios-list.html" });
                    break;

                case MenuOpciones.Marketing:
                    opciones.Add(new MenuOption { Nombre = "Marketing", Nivel = 1, Path = "#" });
                        opciones.Add(new MenuOption { Nombre = "Campañas", Parent = "Marketing", Nivel = 2, Path = "./marketing/campanas-list.html" });
                        opciones.Add(new MenuOption { Nombre = "Segmentos", Parent = "Marketing", Nivel = 2, Path = "./marketing/segmentos-list.html" });
                        opciones.Add(new MenuOption { Nombre = "Campañas Segmentos", Parent = "Marketing", Nivel = 2, Path = "./marketing/campanas-segmentos-list.html" });
                        opciones.Add(new MenuOption { Nombre = "Usuarios Segmentos", Parent = "Marketing", Nivel = 2, Path = "./marketing/usuarios-segmentos-list.html" });
                        opciones.Add(new MenuOption { Nombre = "Ejecución de Campañas", Parent = "Marketing", Nivel = 2, Path = "./marketing/campanas-ejecuciones-list.html" });
                        opciones.Add(new MenuOption { Nombre = "Envío de Correos", Parent = "Marketing", Nivel = 2, Path = "./marketing/envio-correos-list.html" });
                        opciones.Add(new MenuOption { Nombre = "Envío de SMS", Parent = "Marketing", Nivel = 2, Path = "./marketing/envio-sms-list.html" });
                        opciones.Add(new MenuOption { Nombre = "Envío de In-Apps", Parent = "Marketing", Nivel = 2, Path = "./marketing/page.html" });
                    break;

                case MenuOpciones.Informes:
                    opciones.Add(new MenuOption { Nombre = "Informes", Nivel = 1, Path = "#" });
                        opciones.Add(new MenuOption { Nombre = "Procesos automaticos", Parent = "Informes", Nivel = 2, Path = "./workService-dashboard.html" });
                        opciones.Add(new MenuOption { Nombre = "Logins", Parent = "Informes", Nivel = 2, Path = "./reports/logins-list.html" });
                        opciones.Add(new MenuOption { Nombre = "Logs", Parent = "Informes", Nivel = 2, Path = "./reports/logs-list.html" });
                        opciones.Add(new MenuOption { Nombre = "DataQuery1", Parent = "Informes", Nivel = 2, Path = "./reports/dataQuery1-list.html" });
                        opciones.Add(new MenuOption { Nombre = "DataQuery2", Parent = "Informes", Nivel = 2, Path = "./reports/dataQuery2-list.html" });
                        opciones.Add(new MenuOption { Nombre = "DataQuery3", Parent = "Informes", Nivel = 2, Path = "./reports/dataQuery3-list.html" });
                        opciones.Add(new MenuOption { Nombre = "DataQuery3", Parent = "Informes", Nivel = 2, Path = "./reports/dataQuery4-list.html" });
                        opciones.Add(new MenuOption { Nombre = "DataQuery4", Parent = "Informes", Nivel = 2, Path = "./reports/dataQuery5-list.html" });
                    break;

                case MenuOpciones.Configuracion:
                    opciones.Add(new MenuOption { Nombre = "Configuración", Nivel = 1, Path = "#" });
                        opciones.Add(new MenuOption { Nombre = "Tipos de Entidad", Parent = "Configuración", Nivel = 2, Path = "./config/tipo-entidades-list.html" });
                        opciones.Add(new MenuOption { Nombre = "Tipos de Actividad", Parent = "Configuración", Nivel = 2, Path = "./config/tipo-actividades-list.html" });
                        opciones.Add(new MenuOption { Nombre = "Tipos de Transacción", Parent = "Configuración", Nivel = 2, Path = "./config/tipo-transacciones-list.html" });
                        opciones.Add(new MenuOption { Nombre = "Tipos de Recompensa", Parent = "Configuración", Nivel = 2, Path = "./config/tipo-recompensas-list.html" });
                        opciones.Add(new MenuOption { Nombre = "Tipos de Campaña", Parent = "Configuración", Nivel = 2, Path = "./config/tipo-camapanas-list.html" });
                        opciones.Add(new MenuOption { Nombre = "Tipos de Segmento", Parent = "Configuración", Nivel = 2, Path = "./config/tipo-segmentos-list.html" });
                        opciones.Add(new MenuOption { Nombre = "Plantillas de correo", Parent = "Configuración", Nivel = 2, Path = "./config/tipo-plantillas-list.html" });
                        opciones.Add(new MenuOption { Nombre = "Roles", Parent = "Configuración", Nivel = 2, Path = "./config/tipo-roles-list.html" });
                        opciones.Add(new MenuOption { Nombre = "Perfiles", Parent = "Configuración", Nivel = 2, Path = "./config/tipo-perfiles-list.html" });
                        opciones.Add(new MenuOption { Nombre = "Motivos de Consulta", Parent = "Configuración", Nivel = 2, Path = "./config/tipo-consultas-list.html" });
                    break;

                case MenuOpciones.BaseDatos:
                    opciones.Add(new MenuOption { Nombre = "Base de Datos", Nivel = 1, Path = "#" });
                        opciones.Add(new MenuOption { Nombre = "Proceso de anonimización", Parent = "Base de Datos", Nivel = 2, Path = "./bd/anonimizacion-form.html" });
                        opciones.Add(new MenuOption { Nombre = "Histórico de Migraciones", Parent = "Base de Datos", Nivel = 2, Path = "./bd/migraciones-list.html" });
                    break;  
            }
            return opciones;
        }
    }
}