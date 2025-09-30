using System.Security.Cryptography;
using System.Text.Json.Serialization;

namespace Domain.Entities
{
    public class Usuario
    {

        public Usuario()
        {
            UsuarioRoles = new List<UsuarioRol>();
            UsuarioEntidades = new List<UsuarioEntidad>();
            UsuarioSegmentos = new List<UsuarioSegmentos>();
            UsuarioDirecciones = new List<UsuarioDireccion>();
            UsuarioRecompensas = new List<UsuarioRecompensa>();
        }

        private int? _id;
        public int? id {
            get => _id;
            set {
                //if (null == value)
                //    throw new ArgumentException("El campo 'id' tiene que estar informado.");
                _id = value;
            }
        }

        private string _nombre; 
        public string nombre {
            get => _nombre;
            set {
                //if (string.IsNullOrWhiteSpace(value))
                //    throw new ArgumentException("El nombre no puede estar vacío.");
                _nombre = value;
            }
        }

        private string _apellidos;
        public string apellidos {
            get => _apellidos;
            set {
                //if (string.IsNullOrWhiteSpace(value))
                //    throw new ArgumentException("Los apellidos no pueden estar vacíos.");
                _apellidos = value;
            }
        }

        private string _correo;
        public string correo {
            get => _correo;
            set {
                //if (string.IsNullOrWhiteSpace(value) || !value.Contains("@") || !value.Contains("."))
                    //throw new ArgumentException("El email no es válido.");
                _correo = value;
            }
        }

        private bool _activo;
        public bool activo {
            get => _activo;
            set {
                //if (null == value)
                //    throw new ArgumentException("El campo 'activo' tiene que estar informado.");
                _activo = value;
            }
        }

        private string _contrasena;
        public string contrasena {
            get => _contrasena;
            set {
                //if (string.IsNullOrWhiteSpace(value))
                //    throw new ArgumentException("La contraseña no puede estar vacía.");
                _contrasena = value;
            }
        }

        private DateTime _fechaNacimiento;
        public DateTime fechaNacimiento {
            get => _fechaNacimiento;
            set {
                //if (null == value || value > DateTime.Now)
                //    throw new ArgumentException("La fecha de nacimiento no puede estar vacía o en el futuro.");
                _fechaNacimiento = value;
            }
        }
         
        public bool suscrito { get; set; }

        private DateTime? _ultimaConexion;
        public DateTime? ultimaConexion
        {
            get => _ultimaConexion;
            set {
                //if (null != value && value > DateTime.Now)
                //    throw new ArgumentException("La fecha de ultima conexion no puede estar en el futuro.");
                _ultimaConexion = value;
            }
        }

        private DateTime _fechaCreacion;
        public DateTime fechaCreacion
        { 
            get => _fechaCreacion;
            set {
                //if (null != value && value > DateTime.Now)
                //    throw new ArgumentException("La fecha de creación no puede estar en el futuro.");
                _fechaCreacion = value;
            }
}

        private int? _puntos;
        public int? puntos {
            get => _puntos;
            set {
                //if (null != value &&  int.IsNegative(value.Value))
                //    throw new ArgumentException("Los puntos no pueden estar en negativo (-).");
                _puntos = value;
            }
        }
        public string? token { get; set; }

        public DateTime? expiracionToken { get; set; }

        /// <summary>
        /// Género del usuario
        /// </summary>
        private string _genero;
        public string genero
        {
            get => _genero;
            set
            {
                //if (string.IsNullOrWhiteSpace(value))
                //    throw new ArgumentException("El genero no puede estar vacío.");
                _genero = value;
            }
        }

        /// <summary>
        /// Código de recomendación para compartir con amigos
        /// </summary>
        private string _codigoRecomendacion;
        public string codigoRecomendacion
        {
            get => _codigoRecomendacion;
            set => _codigoRecomendacion = value;
        }

        /// <summary>
        /// Código de recomendación de referencia que un amigo ha compartido con el usuario
        /// </summary>
        private string _codigoRecomendacionRef;
        public string codigoRecomendacionRef
        {
            get => _codigoRecomendacionRef;
            set => _codigoRecomendacionRef = value;
        }

        /// <summary>
        /// Código de recomendación de referencia que un amigo ha compartido con el usuario
        /// </summary>
        private string? _telefono;
        public string? telefono
        {
            get => _telefono;
            set => _telefono = value;
        }

        //
        // Rread only properties
        //
        private int _edad;
        public int edad  {
             get {
                var hoy = DateTime.Today;
                _edad = hoy.Year - _fechaNacimiento.Year;

                // Si todavía no cumplió años este año, restamos 1
                if (hoy.Month < _fechaNacimiento.Month ||
                   (hoy.Month == _fechaNacimiento.Month && hoy.Day < _fechaNacimiento.Day)) {
                    _edad--;
                } 
                return _edad;
            }
        }     

        /// <summary>
        /// Enumeración con la lista de géneros 
        /// </summary>
        public enum Genero
        {
            Hombre,
            Mujer,
            Otro
        }

        [JsonIgnore]
        public ICollection<UsuarioRol> UsuarioRoles { get; set; }
        [JsonIgnore]
        public ICollection<UsuarioEntidad> UsuarioEntidades { get; set; }
        [JsonIgnore]
        public ICollection<UsuarioRecompensa> UsuarioRecompensas { get; set; }
        [JsonIgnore]
        public ICollection<UsuarioDireccion> UsuarioDirecciones { get; set; }
        [JsonIgnore]
        public ICollection<UsuarioSegmentos> UsuarioSegmentos { get; set; }
    }
}