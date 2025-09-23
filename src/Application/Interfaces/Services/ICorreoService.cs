using Domain.Entities;

namespace Application.Interfaces.Services
{
    public interface ICorreoService
    {
        Guid EnviarCorreo(Correo correo, string servidorSmtp, string puertoSmtp, string usuarioSmtp, string contraseñaSmtp);
        Task<IEnumerable<TipoEnvioCorreo>> ObtenerTiposEnvioCorreo();
    }
}