using Domain.Entities;
using static Domain.Entities.TipoEnvioCorreo;

namespace Application.Interfaces.Services
{
    public interface ICorreoService
    {
        Guid? EnviarCorreo(Correo correo);
        Guid? EnviarCorreo_Nuevo(CorreoN correo);
        Task<IEnumerable<TipoEnvioCorreo>> ObtenerTiposEnvioCorreo();
        Task<TipoEnvioCorreo> ObtenerTipoEnvioCorreo(TipoEnvioCorreos tipoEnvio);
    }
}