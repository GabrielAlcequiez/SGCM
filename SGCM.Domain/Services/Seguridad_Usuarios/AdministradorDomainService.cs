using SGCM.Domain.Exceptions;
using SGCM.Domain.Repository;
using SGCM.Domain.Services.Interfaces.ISeguridad_Usuarios;

namespace SGCM.Domain.Services
{
    public class AdministradoresDomainService : IAdministradoresDomainService
    {
        private readonly IAdministradoresRepository _administradoresRepository;
        private readonly IUsuarioRepository _usuarioRepository;

        public AdministradoresDomainService(
            IAdministradoresRepository administradoresRepository,
            IUsuarioRepository usuarioRepository)
        {
            _administradoresRepository = administradoresRepository;
            _usuarioRepository = usuarioRepository;
        }

        public async Task PuedeCrearPerfilAdminAsync(int usuarioId)
        {

            var usuario = await _usuarioRepository.ObtenerPorIdAsync(usuarioId);
            if (usuario == null)
            {

                throw new ExcepcionNoEncontrado("Usuario", usuarioId);
            }

            if (usuario.Rol != "Admin" && usuario.Rol != "Administrador")
            {
                throw new ExcepcionReglaNegocio(
                    "El usuario seleccionado no tiene el rol de Administrador asignado.",
                    "ROL_USUARIO_INVALIDO"
                );
            }


            var perfilExistente = await _administradoresRepository.ObtenerPorUsuarioIdAsync(usuarioId);
            if (perfilExistente != null)
            {
                throw new ExcepcionReglaNegocio(
                    "Este usuario ya tiene un perfil de administrador creado previamente.",
                    "PERFIL_ADMIN_DUPLICADO"
                );
            }
        }
    }
}
