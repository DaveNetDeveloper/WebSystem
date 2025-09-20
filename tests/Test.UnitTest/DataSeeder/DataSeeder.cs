using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Bogus;

using Domain.Entities;
using Domain.Entities;
using Infrastructure.Persistence;
 
namespace Test.UnitTest.DataSeeder
{
    public class DataSeeder
    {
        private readonly ApplicationDbContext _context;
        public int Count = 1;

        public DataSeeder(ApplicationDbContext context) {
            _context = context;
        }

        public void Seed(IEnumerable<Enums.EntityType> entitiesToSeed, int count = 1, int? seed = null)
        {
            if (seed.HasValue)
                Randomizer.Seed = new Random(seed.Value);

            //
            int idEntidad, idUsuario, idDireccion, idRecompensa, idProducto, idActividad;
            Guid idRol, idTipoActividad, tipoEntidad;

            foreach (var entity in entitiesToSeed) {
                switch (entity) {
                    case Enums.EntityType.Usuario:
                        
                        idUsuario = SeedUsuarios(count); 

                        Guid idTipoEntidad = SeedTipoEntidades(count);
                        idEntidad = SeedEntidades(count, idTipoEntidad); 
                        SeedUsuarioEntidades(idUsuario, idEntidad);
                         
                        idRol = SeedRoles(count); 
                        SeedUsuarioRoles(idUsuario, idRol, idEntidad);

                        idDireccion = SeedDirecciones(count);
                        SeedUsuarioDirecciones(idUsuario, idDireccion);

                        idRecompensa = SeedRecompensas(count, idEntidad);
                        SeedUsuarioRecompensas(idUsuario, idRecompensa);
                        break; 
                    case Enums.EntityType.Producto:

                        Guid _idTipoEntidad = SeedTipoEntidades(count);
                        int _idEntidad = SeedEntidades(count, _idTipoEntidad); 
                        idProducto = SeedProductos(count, _idEntidad);
                        SeedProductoImagenes(count, idProducto);
                        break; 
                    case Enums.EntityType.Actividad:

                        idTipoActividad = SeedTipoActividades(count);
                        idActividad = SeedActividades(count, idTipoActividad);
                        SeedActividadImagenes(count, idActividad);                        
                        break;
                    case Enums.EntityType.Categoria:

                        Guid __idTipoEntidad = SeedTipoEntidades(count);
                        SeedCategorias(count, __idTipoEntidad); 
                        break;
                    case Enums.EntityType.TipoEntidad:

                        SeedTipoEntidades(count);
                        break;
                    case Enums.EntityType.Entidad:

                        tipoEntidad = SeedTipoEntidades(count);
                        SeedEntidades(count, tipoEntidad);
                        break;
                    case Enums.EntityType.TipoActividad:

                        SeedTipoActividades(count);
                        break;
                    case Enums.EntityType.QR:

                        Guid ___idTipoEntidad = SeedTipoEntidades(count);
                        int __idEntidad = SeedEntidades(count, ___idTipoEntidad);
                        idProducto = SeedProductos(count, __idEntidad);
                        SeedQRs(count, idProducto);
                        break;
                   
                    case Enums.EntityType.FAQ:

                        SeedFAQ(count);
                        break;
                    case Enums.EntityType.Rol:

                        SeedRoles(count);
                        break;
                    case Enums.EntityType.Transaccion:

                        Guid ____idTipoEntidad = SeedTipoEntidades(count);
                        int ___idEntidad = SeedEntidades(count, ____idTipoEntidad);
                        idProducto = SeedProductos(count, ___idEntidad);

                        int _idUsuario = SeedUsuarios(count); 
                        SeedTransacciones(count, idProducto, _idUsuario); 
                        break;
                    case Enums.EntityType.Testimonio:

                         SeedTestimonios(count);
                        break;

                    default:
                        throw new NotImplementedException($"Seeder no implementado para {entity}");
                }
            } 
            _context.SaveChanges();
        }
        
        // Usuarios 
        private int SeedUsuarios(int count)
        {
            var faker = new Faker<Usuario>()
                .RuleFor(u => u.id, f => f.IndexFaker + 1) // IndexFaker empieza en 0 y se incrementa
                .RuleFor(u => u.nombre, f => f.Person.FirstName)
                .RuleFor(u => u.apellidos, f => f.Person.LastName)
                .RuleFor(u => u.correo, f => f.Internet.Email())
                .RuleFor(u => u.contraseña, f => f.Internet.Password())
                .RuleFor(u => u.activo, f => f.Random.Bool())
                .RuleFor(u => u.fechaNacimiento, f => f.Person.DateOfBirth)
                .RuleFor(u => u.suscrito, f => f.Random.Bool())
                .RuleFor(u => u.fechaCreación, DateTime.Now)
                .RuleFor(u => u.token, f => f.Random.AlphaNumeric(45))
                .RuleFor(u => u.puntos, f => f.Random.Number(0, 1000));
            
            var usuarios = faker.Generate(count);
            _context.Usuarios.AddRange(usuarios);

            return usuarios.FirstOrDefault().id.Value;
        } 
        private void SeedUsuarioRoles(int idUsuario, Guid idRol, int idEntidad)
        {
            var faker = new Faker<UsuarioRol>()
                .RuleFor(u => u.identidad, idEntidad)
                .RuleFor(u => u.idrol, idRol)
                .RuleFor(u => u.idusuario, idUsuario);

            var usuarios = faker.Generate(1);
            _context.UsuarioRoles.AddRange(usuarios); 
        } 
        private void SeedUsuarioEntidades(int idUsuario, int idEntidad)
        {
            var faker = new Faker<UsuarioEntidad>()
                .RuleFor(u => u.identidad, idEntidad)
                .RuleFor(u => u.fecha, DateTime.Now)
                .RuleFor(u => u.idusuario, idUsuario);

            var usuarios = faker.Generate(1);
            _context.UsuarioEntidades.AddRange(usuarios);
        } 
        private void SeedUsuarioDirecciones(int idUsuario, int idDireccion)
        {
            var faker = new Faker<UsuarioDireccion>()
                .RuleFor(u => u.iddireccion, idDireccion)
                .RuleFor(u => u.fecha, DateTime.Now)
                .RuleFor(u => u.idusuario, idUsuario);

            var usuarioDireccion = faker.Generate(1);
            _context.UsuarioDirecciones.AddRange(usuarioDireccion);

        } 
        private void SeedUsuarioRecompensas(int idUsuario, int idRecompensa)
        {
            var faker = new Faker<UsuarioRecompensa>()
                .RuleFor(u => u.idrecompensa, idRecompensa)
                .RuleFor(u => u.fecha, DateTime.Now)
                .RuleFor(u => u.idusuario, idUsuario);

            var usuariosRecompensas = faker.Generate(1);
            _context.UsuarioRecompensas.AddRange(usuariosRecompensas);
        }

        // Productos 
        public Faker<Producto> GetFaker_Producto()
        {
            int nextId = _context.Productos.Count() + 1;

            var faker = new Faker<Producto>()
                    .RuleFor(r => r.id, nextId)
                    .RuleFor(r => r.nombre, f => "Producto " + (nextId).ToString()) 
                    .RuleFor(r => r.idEntidad, f => f.Random.Int(0, 3))
                    .RuleFor(r => r.activo, f => f.Random.Bool())
                    .RuleFor(r => r.puntos, f => f.Random.Int(0, 100))
                    .RuleFor(r => r.descripcion, f => "Descripción del producto " + (nextId).ToString());

            return faker;
        } 
        private int SeedProductos(int count, int idEntidad)
        {
            var faker = new Faker<Producto>()
                //.RuleFor(p => p.nombre, f => f.Commerce.ProductName())
                .RuleFor(r => r.id, f => f.IndexFaker + 1)
                .RuleFor(r => r.nombre, f => "Producto " + (f.IndexFaker + 1).ToString())
                .RuleFor(r => r.idEntidad, idEntidad)
                .RuleFor(r => r.activo, f => f.Random.Bool())
                .RuleFor(r => r.puntos, f => f.Random.Int(0, 100))
                .RuleFor(r => r.descripcion, f => "Descripción del producto " + (f.IndexFaker + 1).ToString());

            var productos = faker.Generate(count);
            _context.Productos.AddRange(productos);
            return productos.FirstOrDefault().id;
        } 
        private void SeedProductoImagenes(int count, int idProducto)
        {
            var faker = new Faker<ProductoImagen>()
                .RuleFor(a => a.id, f => f.IndexFaker + 1)
                .RuleFor(a => a.idproducto, idProducto)
                .RuleFor(a => a.imagen, "Producto" + idProducto.ToString() + ".jpg");

            var productoImagenes = faker.Generate(count);
            _context.ProductoImagenes.AddRange(productoImagenes);
        } 
        
        // Categorias 
        public Faker<Categoria> GetFaker_Categoria()
        {
            int nextId = _context.Categorias.Count() + 1; 
            var faker = new Faker<Categoria>()
                    .RuleFor(r => r.nombre, f => "Categoria " + (nextId).ToString())
                    .RuleFor(r => r.id, f => f.Random.Guid())
                    .RuleFor(r => r.idTipoEntidad, f => f.Random.Guid())
                    .RuleFor(r => r.descripcion, f => "Descripción de la categoria " + (nextId).ToString());

            return faker;
        } 
        private void SeedCategorias(int count, Guid idTipoEntidad)
        {
            var faker = new Faker<Categoria>()
                .RuleFor(c => c.nombre, f => "Categoria " + f.IndexFaker + 1)
                .RuleFor(c => c.id, f => f.Random.Guid())
                .RuleFor(c => c.idTipoEntidad, idTipoEntidad)
                .RuleFor(c => c.descripcion, f => "Descripción de la categoria " + f.IndexFaker + 1);
            
            var categorias = faker.Generate(count);
            _context.Categorias.AddRange(categorias);
        } 

        // Actividades 
        public Faker<Actividad> GetFaker_Actividad()
        {
            int nextId = _context.Actividades.Count() + 1;

            var faker = new Faker<Actividad>()
                    .RuleFor(a => a.nombre, f => "Actividad " + (nextId).ToString())
                    .RuleFor(a => a.id, nextId)
                    .RuleFor(a => a.idEntidad, f => f.Random.Number(1, 2))
                    .RuleFor(a => a.descripcion, f => "Descripción de la actividad " + nextId)
                    .RuleFor(a => a.linkEvento, f => f.Internet.Url())
                    .RuleFor(a => a.idTipoActividad, f => f.Random.Guid())
                    .RuleFor(a => a.ubicación, f => f.Address.ToString())
                    .RuleFor(a => a.popularidad, f => f.Random.Number(1, 5))
                    .RuleFor(a => a.descripcionCorta, f => "Descripción corta de la actividad " + nextId)
                    .RuleFor(a => a.fechaInicio, DateTime.Now.AddDays(7)) //f => f.Date.FutureDateOnly().ToDateTime())
                    .RuleFor(a => a.fechaFin, DateTime.Now.AddDays(8))
                    .RuleFor(a => a.gratis, f => f.Random.Bool())
                    .RuleFor(a => a.activo, f => true)
                    .RuleFor(a => a.informacioExtra, f => "Información extra de la actividad " + nextId)
                    .RuleFor(a => a.linkInstagram, f => "https://www.instagram.com/" + "Actividad" + nextId)
                    .RuleFor(a => a.linkFacebook, f => "https://www.facebook.com/" + "Actividad" + nextId)
                    .RuleFor(a => a.linkYoutube, f => "https://www.youtube.com/" + "Actividad" + nextId);

            return faker;
        } 
        private void SeedActividadImagenes(int count, int idactividad)
        {
            //var faker = GetFaker_Actividad();

            var faker = new Faker<ActividadImagen>() 
                .RuleFor(a => a.id, f => f.IndexFaker + 1)
                .RuleFor(a => a.idactividad, idactividad)
                .RuleFor(a => a.imagen, f => "Actividad" + idactividad.ToString() + ".jpg");

            var actividadImagenes = faker.Generate(count);
            _context.ActividadImagenes.AddRange(actividadImagenes);
        } 
        private int SeedActividades(int count, Guid idTipoActividad)
        {
            //var faker = GetFaker_Actividad();

            var faker = new Faker<Actividad>()
                .RuleFor(a => a.nombre, f => "Actividad " + f.IndexFaker + 1)
                .RuleFor(a => a.id, f => f.IndexFaker + 1)
                .RuleFor(a => a.idEntidad, f => f.Random.Number(1, 2))
                .RuleFor(a => a.descripcion, f => "Descripción de la actividad " + f.IndexFaker + 1)
                .RuleFor(a => a.linkEvento, f => f.Internet.Url())
                .RuleFor(a => a.idTipoActividad, idTipoActividad)
                .RuleFor(a => a.ubicación, f => f.Address.ToString())
                .RuleFor(a => a.popularidad, f => f.Random.Number(1, 5))
                .RuleFor(a => a.descripcionCorta, f => "Descripción corta de la actividad " + f.IndexFaker + 1)
                .RuleFor(a => a.fechaInicio, DateTime.Now.AddDays(7)) //f => f.Date.FutureDateOnly().ToDateTime())
                .RuleFor(a => a.fechaFin, DateTime.Now.AddDays(8))
                .RuleFor(a => a.gratis, f => f.Random.Bool())
                .RuleFor(a => a.activo, f => true)
                .RuleFor(a => a.informacioExtra, f => "Información extra de la actividad " + f.IndexFaker + 1)
                .RuleFor(a => a.linkInstagram, f => "https://www.instagram.com/" + "Actividad" + f.IndexFaker + 1)
                .RuleFor(a => a.linkFacebook, f => "https://www.facebook.com/" + "Actividad" + f.IndexFaker + 1)
                .RuleFor(a => a.linkYoutube, f => "https://www.youtube.com/" + "Actividad" + f.IndexFaker + 1);

            var actividades = faker.Generate(count);
            _context.Actividades.AddRange(actividades);

            return actividades.FirstOrDefault().id;
        }

        // Tipo Actividades
        private Guid SeedTipoActividades(int count)
        {
            int nextId = _context.TipoActividades.Count() + 1;

            var faker = new Faker<TipoActividad>()
                .RuleFor(ta => ta.nombre, f => "Actividad Tipo " + nextId)
                .RuleFor(ta => ta.id, f => f.Random.Guid())
                .RuleFor(ta => ta.descripcion, f => "Descripción del tipo de actividad " + nextId);
             
            var tipoActividades = faker.Generate(count);
            _context.TipoActividades.AddRange(tipoActividades);
            return tipoActividades.FirstOrDefault().id;
        }

        // Tipo Entidades
        private Guid SeedTipoEntidades(int count)
        {
            var faker = new Faker<TipoEntidad>()
                .RuleFor(te => te.id, f => f.Random.Guid())
                .RuleFor(te => te.nombre, f => "Entidad Tipo " + f.IndexFaker + 1)
                .RuleFor(te => te.descripcion, f => "Descripción del tipo de entidad " + f.IndexFaker + 1);

            var tipoEntidades = faker.Generate(count);
            _context.TipoEntidades.AddRange(tipoEntidades);
            return tipoEntidades.FirstOrDefault().id;
        }

        //Entidades
        private int SeedEntidades(int count, Guid idTipoEntidad)
        {
            var faker = new Faker<Entidad>()
                .RuleFor(e => e.id, f => f.IndexFaker + 1)
                .RuleFor(e => e.idTipoEntidad, idTipoEntidad)
                .RuleFor(e => e.nombre, f => "Entidad " + f.IndexFaker + 1)
                .RuleFor(e => e.ubicacion, f => f.Address.ToString())
                .RuleFor(e => e.fechaAlta, DateTime.Now.AddDays(-7))
                .RuleFor(e => e.popularidad, f => f.Random.Number(1, 5))
                .RuleFor(e => e.descripcion, f => "Descripción de la entidad " + f.IndexFaker + 1)
                .RuleFor(e => e.activo, f => f.Random.Bool()) 
                .RuleFor(e => e.imagen, f => "image_" + (f.IndexFaker + 1).ToString() + ".png");
             
            var entidades = faker.Generate(count);
            _context.Entidades.AddRange(entidades);
            return entidades.FirstOrDefault().id;
        }

        // Roles 
        public Faker<Rol> GetFaker_Rol()
        {
            int nextId = _context.Roles.Count() + 1; 
            var faker = new Faker<Rol>()
                    .RuleFor(r => r.id, f => f.Random.Guid())
                    .RuleFor(r => r.nombre, f => "Rol " + (nextId).ToString()) 
                    .RuleFor(r => r.descripcion, f => "Descripción del rol " + (nextId).ToString());

            return faker;
        }  
        private Guid SeedRoles(int count)
        {
            var faker = new Faker<Rol>()
                .RuleFor(r => r.id, f => f.Random.Guid())
                .RuleFor(r => r.nombre, f => "Rol " + (f.IndexFaker + 1).ToString())
                .RuleFor(r => r.descripcion, f => "Descripción para el rol " + (f.IndexFaker + 1).ToString());
            
             var roles = faker.Generate(count);
            _context.Roles.AddRange(roles);
            return roles.FirstOrDefault().id;
        }

        // Direcciones
        private int SeedDirecciones(int count)
        {
            var faker = new Faker<Direccion>()
                .RuleFor(r => r.id, f => f.IndexFaker + 1)
                .RuleFor(r => r.tipoVia, f => f.PickRandom(new[] { "Calle", "Plaza", "Avenina", "NºKm" }))
                .RuleFor(r => r.nombreVia, f => "Via " + (f.IndexFaker + 1).ToString())
                .RuleFor(r => r.numero, f => f.Random.Int(1, 150))
                .RuleFor(r => r.bloque, f => f.PickRandom(new[] { "1", "2", "3", "4" }))
                .RuleFor(r => r.escalera, f => f.PickRandom(new[] { "A", "B", "C", "D" }))
                .RuleFor(r => r.piso, f => f.PickRandom(new[] { "1", "2", "3", "4", "5", "A", "SA" }))
                .RuleFor(r => r.puerta, f => f.Random.Int(1, 4))
                .RuleFor(r => r.codigoPostal, f => f.Random.Replace("#####"))
                .RuleFor(r => r.ciudad, f => f.PickRandom(new[] {
                                                                "Madrid", "Barcelona", "Valencia", "Sevilla", "Zaragoza",
                                                                "Málaga", "Murcia", "Palma de Mallorca", "Bilbao", "Alicante"
                                                          }))
                .RuleFor(r => r.provincia, f => f.PickRandom(new[] {
                                                                "Álava", "Albacete", "Alicante", "Almería", "Asturias", "Ávila",
                                                                "Badajoz", "Barcelona", "Burgos", "Cáceres", "Cádiz", "Cantabria",
                                                                "Castellón", "Ciudad Real", "Córdoba", "Cuenca", "Girona", "Granada",
                                                                "Guadalajara", "Guipúzcoa", "Huelva", "Huesca", "Islas Baleares",
                                                                "Jaén", "La Coruña", "La Rioja", "Las Palmas", "León", "Lleida",
                                                                "Lugo", "Madrid", "Málaga", "Murcia", "Navarra", "Ourense",
                                                                "Palencia", "Pontevedra", "Salamanca", "Santa Cruz de Tenerife",
                                                                "Segovia", "Sevilla", "Soria", "Tarragona", "Teruel", "Toledo",
                                                                "Valencia", "Valladolid", "Vizcaya", "Zamora", "Zaragoza"
                                                            }))
                .RuleFor(r => r.comunidadAutonoma, f => f.PickRandom(new[] {
                                                                    "Andalucía", "Aragón", "Asturias", "Baleares", "Canarias",
                                                                    "Cantabria", "Castilla-La Mancha", "Castilla y León", "Cataluña",
                                                                    "Extremadura", "Galicia", "Madrid", "Murcia", "Navarra",
                                                                    "La Rioja", "País Vasco", "Valencia", "Ceuta", "Melilla"
                                                                    }))
                .RuleFor(r => r.pais, "España");

            var direcciones = faker.Generate(count);
            _context.Direcciones.AddRange(direcciones);
            return direcciones.FirstOrDefault().id;
        } 

        // Recompensas
        private int SeedRecompensas(int count, int idEntidad)
        {
            var faker = new Faker<Recompensa>()
                .RuleFor(r => r.id, f => f.IndexFaker + 1)
                .RuleFor(r => r.identidad, idEntidad)
                .RuleFor(r => r.nombre, f => "Recompensa " + (f.IndexFaker + 1).ToString())
                .RuleFor(r => r.descripcion, f => "Descripción para la recompensa " + (f.IndexFaker + 1).ToString());

            
            var recompensas = faker.Generate(count);
            _context.Recompensas.AddRange(recompensas);
            return recompensas.FirstOrDefault().id;
        } 

        // Transacciones
        private void SeedTransacciones(int count, int idProducto, int idUsuario)
        {
            var faker = new Faker<Transaccion>()
                .RuleFor(c => c.id, f => f.IndexFaker + 1)
                .RuleFor(c => c.nombre, f => "Transaccion " + f.IndexFaker + 1)
                .RuleFor(c => c.idProducto, idProducto)
                .RuleFor(c => c.puntos, f => f.PickRandom(new[] { 50, 100, 300, 500 }))
                .RuleFor(c => c.idUsuario, idUsuario)
                .RuleFor(c => c.fecha, DateTime.Now); 

            var transacciones = faker.Generate(count);
            _context.Transacciones.AddRange(transacciones);
        }

        // FAQ 
        private Guid SeedFAQ(int count)
        {
            var faker = new Faker<FAQ>()
                .RuleFor(r => r.id, f => f.Random.Guid())
                .RuleFor(r => r.orden, f => f.IndexFaker + 1)
                .RuleFor(r => r.pregunta, f => "Pregunta de la FAQ" + (f.IndexFaker + 1).ToString())
                .RuleFor(r => r.respuesta, f => "Respuesta de la FAQ " + (f.IndexFaker + 1).ToString());
             
            var faqs = faker.Generate(count);
            _context.FAQS.AddRange(faqs);
            return faqs.FirstOrDefault().id;
        }

        // QR 
        private Guid SeedQRs(int count, int idProducto)
        {
            var faker = new Faker<QR>()
                .RuleFor(r => r.id, f => f.Random.Guid())
                .RuleFor(r => r.idProducto, idProducto)
                .RuleFor(r => r.activo, true)
                .RuleFor(r => r.multicliente, false)
                .RuleFor(r => r.consumido, false)
                .RuleFor(r => r.qrCode, f => f.Random.AlphaNumeric(12))
                .RuleFor(r => r.fechaExpiracion, DateTime.Now); 
             
            var qrs = faker.Generate(count);
            _context.QRs.AddRange(qrs);
            return qrs.FirstOrDefault().id;
        }

        // Testimonios
        private int SeedTestimonios(int count)
        {
            var faker = new Faker<Testimonio>()
                .RuleFor(r => r.id, f => f.IndexFaker + 1)  
                .RuleFor(r => r.nombreUsuario, f => f.PickRandom(new[] { "Usuario 1", "Usuario 2", "Usuario 3", "Usuario 4" }))
                .RuleFor(r => r.texto, f => "Texto para el testimonio " + (f.IndexFaker + 1).ToString())
                .RuleFor(r => r.imagen, f => "Imagen" + (f.IndexFaker + 1).ToString() + ".jpg")
                .RuleFor(r => r.fecha, DateTime.Now); 

            var testimonios = faker.Generate(count);
            _context.Testimonios.AddRange(testimonios);
            return testimonios.FirstOrDefault().id;
        }
    }
}