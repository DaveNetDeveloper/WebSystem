using Domain;
using Domain.DataQuery;
using Domain.Entities;
using Domain.DataQuery;
using Microsoft.EntityFrameworkCore;
using static Domain.Entities.Transaccion;

namespace Infrastructure.Persistence
{
   public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) {
        }

        //
        // Tables
        //
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<UsuarioRol> UsuarioRoles { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<TipoEntidad> TipoEntidades { get; set; }
        public DbSet<TipoTransaccion> TipoTransacciones { get; set; }
        public DbSet<Transaccion> Transacciones { get; set; }
        public DbSet<QR> QRs { get; set; }
        public DbSet<Entidad> Entidades { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<ProductoImagen> ProductoImagenes { get; set; } 
        public DbSet<Actividad> Actividades { get; set; }
        public DbSet<ActividadImagen> ActividadImagenes { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<FAQ> FAQS { get; set; }
        public DbSet<TipoActividad> TipoActividades { get; set; } 
        public DbSet<Testimonio> Testimonios { get; set; }
        public DbSet<Recompensa> Recompensas { get; set; }
        public DbSet<TipoRecompensa> TipoRecompensas { get; set; } 
        public DbSet<Direccion> Direcciones { get; set; }
        public DbSet<TipoEnvioCorreo> TipoEnvioCorreos { get; set; }
        public DbSet<InAppNotification> InAppNotifications { get; set; }
        public DbSet<SmsNotification> SmsNotifications { get; set; }

        //public DbSet<EmailNotification> EmailNotifications { get; set; }
        public DbSet<EmailToken> EmailTokens { get; set; }
        public DbSet<Consulta> Consultas { get; set; } 
        public DbSet<MotivoConsulta> MotivosConsultas { get; set; } 
        public DbSet<EntidadCategoria> EntidadCategorias { get; set; }   
        public DbSet<UsuarioDireccion> UsuarioDirecciones { get; set; }
        public DbSet<UsuarioEntidad> UsuarioEntidades { get; set; }
        public DbSet<UsuarioRecompensa> UsuarioRecompensas { get; set; }
        public DbSet<UsuarioSegmentos> UsuarioSegmentos { get; set; }
        public DbSet<Campana> Campanas { get; set; }
        public DbSet<TipoCampana> TipoCampanas { get; set; }
        public DbSet<CampanaSegmentos> CampanaSegmentos { get; set; }
        public DbSet<CampanaAcciones> CampanaAcciones  { get; set; }
        public DbSet<CampanaExecution> CampanaExecutions { get; set; }
        public DbSet<Accion> Acciones { get; set; }
        public DbSet<TipoSegmento> TipoSegmentos { get; set; }
        public DbSet<Segmento> Segmentos { get; set; }
        public DbSet<Login> Logins { get; set; } 

        //
        // Views
        //
        public DbSet<v_UsuariosInactivos> v_UsuariosInactivos { get; set; }
        public DbSet<v_ActividadUsuarios> v_ActividadUsuarios { get; set; }
        public DbSet<v_UsuariosDispositivos> v_UsuariosDispositivos { get; set; }
        public DbSet<v_UsuariosIdiomas> v_UsuariosIdiomas { get; set; }
        public DbSet<v_RolesUsuarios> v_RolesUsuarios { get; set; }
        public DbSet<v_VisitasTipoDispositivo> v_VisitasTipoDispositivo { get; set; }
        public DbSet<v_TotalUltimasTransacciones> v_TotalUltimasTransacciones { get; set; }
        public DbSet<v_CampanasUsuarios> v_CampanasUsuarios { get; set; }
        public DbSet<v_AllUserData> v_AllUserData { get; set; }
        public DbSet<v_AllCampanasData> v_AllCampanasData { get; set; }

        //
        // Jobs
        //
        public DbSet<WorkerServiceExecution> WorkerServiceExecutions { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //
            // TABLES
            //

            //UsuarioRol
            modelBuilder.Entity<UsuarioRol>().HasKey(ur => new { ur.idrol, ur.idusuario, ur.identidad }); 

            modelBuilder.Entity<UsuarioRol>()
                .HasOne(ur => ur.Usuario)
                .WithMany(u => u.UsuarioRoles)
               // .WithMany()
                .HasForeignKey(ur => ur.idusuario);

            modelBuilder.Entity<UsuarioRol>()
                .HasOne(r => r.Rol)
               .WithMany(ur => ur.UsuarioRoles)
                //.WithMany()
                .HasForeignKey(ur => ur.idrol);

            modelBuilder.Entity<UsuarioRol>()
               .HasOne(e => e.Entidad)
               .WithMany(ur => ur.UsuarioRoles)
               //.WithMany()
               .HasForeignKey(ur => ur.identidad);

            //UsuarioEntidad
            modelBuilder.Entity<UsuarioEntidad>().HasKey(ue => new { ue.idusuario, ue.identidad }); 

            modelBuilder.Entity<UsuarioEntidad>()
                .HasOne(e => e.Entidad)
                .WithMany(ue => ue.UsuarioEntidades)
                // .WithMany()
                .HasForeignKey(ue => ue.identidad);

            modelBuilder.Entity<UsuarioEntidad>()
                .HasOne(e => e.Usuario)
                .WithMany(ue => ue.UsuarioEntidades)
                // .WithMany()
                .HasForeignKey(ue => ue.idusuario);

            //EntidadCategoria
            modelBuilder.Entity<EntidadCategoria>().HasKey(ec => new { ec.identidad, ec.idcategoria }); 

            modelBuilder.Entity<EntidadCategoria>()
                .HasOne(e => e.Entidad)
                .WithMany(ec => ec.EntidadadCategorias)
                // .WithMany()
                .HasForeignKey(ec => ec.identidad);

            modelBuilder.Entity<EntidadCategoria>()
                .HasOne(c => c.Categoria)
                .WithMany(ec => ec.EntidadadCategorias)
                // .WithMany()
                .HasForeignKey(ec => ec.idcategoria); 

             
            //UsuarioRecompensa
            modelBuilder.Entity<UsuarioRecompensa>().HasKey(ur => new { ur.idusuario, ur.idrecompensa }); 

            modelBuilder.Entity<UsuarioRecompensa>()
                .HasOne(r => r.Recompensa)
                .WithMany(ur => ur.UsuarioRecompensas)
                // .WithMany()
                .HasForeignKey(ur => ur.idrecompensa);

            modelBuilder.Entity<UsuarioRecompensa>()
                .HasOne(u => u.Usuario)
                .WithMany(ur => ur.UsuarioRecompensas)
                // .WithMany()
                .HasForeignKey(ur => ur.idusuario);


            //UsuarioDireccion
            modelBuilder.Entity<UsuarioDireccion>().HasKey(ud => new { ud.idusuario, ud.iddireccion }); 

            modelBuilder.Entity<UsuarioDireccion>()
                .HasOne(d => d.Direccion)
                .WithMany(ud => ud.UsuarioDirecciones)
                // .WithMany()
                .HasForeignKey(ud => ud.iddireccion);

            modelBuilder.Entity<UsuarioDireccion>()
                .HasOne(u => u.Usuario)
                .WithMany(ud => ud.UsuarioDirecciones)
                // .WithMany()
                .HasForeignKey(ud => ud.idusuario);

            //ActividadImagenes
            modelBuilder.Entity<ActividadImagen>().HasKey(ai => new { ai.id });

            modelBuilder.Entity<ActividadImagen>()
                .HasOne(a => a.Actividad)
                //.WithMany(ai => ai.UsuarioDirecciones)
                .WithMany()
                .HasForeignKey(ai => ai.idactividad);

            //ProductoImagenes
            modelBuilder.Entity<ProductoImagen>().HasKey(pi => new { pi.id});

            modelBuilder.Entity<ProductoImagen>()
                .HasOne(p => p.Producto)
                //.WithMany(ud => ud.UsuarioDirecciones)
                .WithMany()
                .HasForeignKey(pi => pi.idproducto);

            //UsuarioSegmentos
            modelBuilder.Entity<UsuarioSegmentos>().HasKey(ud => new { ud.idUsuario, ud.idSegmento });
            
            modelBuilder.Entity<UsuarioSegmentos>()
                .HasOne(d => d.Usuario)
                .WithMany(ud => ud.UsuarioSegmentos)
                // .WithMany()
                .HasForeignKey(ud => ud.idUsuario);

            modelBuilder.Entity<UsuarioSegmentos>()
               .HasOne(d => d.Segmento)
               .WithMany(ud => ud.UsuarioSegmentos)
               // .WithMany()
               .HasForeignKey(ud => ud.idSegmento);

            // CampanaSegmentos
            modelBuilder.Entity<CampanaSegmentos>().HasKey(ud => new { ud.idCampana, ud.idSegmento });

            modelBuilder.Entity<CampanaSegmentos>()
                .HasOne(d => d.Segmento)
                .WithMany(ud => ud.CampanaSegmentos)
                // .WithMany()
                .HasForeignKey(ud => ud.idSegmento);

            modelBuilder.Entity<CampanaSegmentos>()
                .HasOne(d => d.Campana)
                .WithMany(ud => ud.CampanaSegmentos)
                // .WithMany()
                .HasForeignKey(ud => ud.idCampana);

            // CampanaAcciones
            modelBuilder.Entity<CampanaAcciones>().HasKey(ud => new { ud.idCampana, ud.idAccion });

            modelBuilder.Entity<CampanaAcciones>()
                .HasOne(d => d.Campana)
                .WithMany(ud => ud.CampanaAcciones)
                // .WithMany()
                .HasForeignKey(ud => ud.idCampana);

            modelBuilder.Entity<CampanaAcciones>()
                .HasOne(d => d.Accion)
                .WithMany(ud => ud.CampanaAcciones)
                // .WithMany()
                .HasForeignKey(ud => ud.idAccion);



            //
            // VIEWS
            //
            modelBuilder.Entity<v_UsuariosInactivos>(eb => {
                eb.HasNoKey();
                eb.ToView("v_UsuariosInactivos");
            });

            modelBuilder.Entity<v_ActividadUsuarios>(eb => {
                eb.HasNoKey();
                eb.ToView("v_ActividadUsuarios");
            });

            modelBuilder.Entity<v_UsuariosDispositivos>(eb => {
                eb.HasNoKey();
                eb.ToView("v_UsuariosDispositivos");
            });

            modelBuilder.Entity<v_UsuariosIdiomas>(eb => {
                eb.HasNoKey();
                eb.ToView("v_UsuariosIdiomas");
            });

            modelBuilder.Entity<v_RolesUsuarios>(eb => {
                eb.HasNoKey();
                eb.ToView("v_RolesUsuarios");
            });

            modelBuilder.Entity<v_VisitasTipoDispositivo>(eb => {
                eb.HasNoKey();
                eb.ToView("v_VisitasTipoDispositivo");
            });

            modelBuilder.Entity<v_TotalUltimasTransacciones>(eb => {
                eb.HasNoKey();
                eb.ToView("v_TotalUltimasTransacciones");
            });

            modelBuilder.Entity<v_CampanasUsuarios>(eb => {
                eb.HasNoKey();
                eb.ToView("v_CampanasUsuarios");
            });

            modelBuilder.Entity<v_AllUserData>(eb => {
                eb.HasNoKey();
                eb.ToView("v_AllUserData");
            });

            modelBuilder.Entity<v_AllCampanasData>(eb => {
                eb.HasNoKey();
                eb.ToView("v_AllCampanasData");
            });

            //
            // Aplicar configuraciones con Fluent API
            //
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }
    }
}
