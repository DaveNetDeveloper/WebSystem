using Domain.Entities;

namespace Application.Interfaces.Services
{
    public interface ICorreoService
    {
        Guid? EnviarCorreo(Correo correo);
        Task<IEnumerable<TipoEnvioCorreo>> ObtenerTiposEnvioCorreo();
    }
}