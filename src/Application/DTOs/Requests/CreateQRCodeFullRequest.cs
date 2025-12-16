using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Requests
{
    public record CreateQRCodeFullRequest(
        // Detalle Básico del QR
        string Payload,
        int? Ttl, // Se calcula desde la Fecha de Expiración (es nullable si no hay fecha)
        string Origen, // Producto o Actividad (string)

        // Asociación de Entidad
        int IdEntidad,

        // Asociación Condicional (Solo uno debe tener valor si el Origen aplica)
        int? IdProducto,  // Nullable
        int? IdActividad, // Nullable

        // Estado
        int Status        // 0, 1, 2 (int)
    );
}
