using System.Text;
using System.Text.Json.Serialization;
using static Domain.Entities.TipoEnvioCorreo;

namespace Domain.Entities
{
    /// <summary>
    /// 
    /// </summary>
    public class FicheroAdjunto
    { 
        public byte[] Archivo { get; set; }
        public string NombreArchivo { get; set; }
        public string ContentType { get; set; }
    }
}