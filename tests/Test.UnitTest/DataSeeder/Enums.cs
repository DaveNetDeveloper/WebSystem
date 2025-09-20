using Domain;
using Domain.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
namespace Test.UnitTest.DataSeeder
{
    public class Enums { 
        public enum EntityType { 
            Actividad,
            Auth,
            Categoria,
            Entidad,
            FAQ,
            Producto,
            QR,
            Rol,
            Testimonio,
            TipoEntidad,
            TipoActividad, // TODO pdte. crear repositorio para entidad "Tipo Actividad"
            Transaccion,
            Usuario
        }
    }
}
