using System.Text;
using System.Text.Json.Serialization;
using static Domain.Entities.TipoEnvioCorreo;

namespace Domain.Entities
{
    public sealed class EnvioEscanearProductoEmailContext : EmailContextBase
    {
        public string NombreEntidad { get; }
        public string NombreProducto { get; }
        public string PuntosProducto { get; }
        public string FechaEscaneo { get; }

        public EnvioEscanearProductoEmailContext(string email, 
                                                 string nombre, 
                                                 string nombreEntidad,
                                                 string nombreProducto,
                                                 string puntosProducto,
                                                 string fechaEscaneo) : base(email, nombre) {
            NombreEntidad = nombreEntidad;
            NombreProducto = nombreProducto;
            PuntosProducto = puntosProducto;
            FechaEscaneo = fechaEscaneo;
        }

        public IDictionary<string, string> GetTags()
        {
            var tags = GetBaseTags();
            tags["[_ENTITY_]"] = NombreEntidad;
            tags["[_NOMBRE-PRODUCTO_]"] = NombreProducto;
            tags["[_PUNTOS-PRODUCTO_]"] = PuntosProducto;
            tags["[_FECHA-ESCANEO-QR_]"] = FechaEscaneo;
            return tags;
        }
    }
}