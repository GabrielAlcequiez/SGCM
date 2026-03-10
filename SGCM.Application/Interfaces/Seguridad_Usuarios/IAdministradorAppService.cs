using SGCM.Application.Base;
using SGCM.Application.Dtos.Seguridad_Usuarios;

namespace SGCM.Application.Interfaces.Seguridad_Usuarios
{
    public interface IAdministradorAppService : IAppService<CrearAdministradorDto, AdministradorResponseDto>
    {
    }
}
