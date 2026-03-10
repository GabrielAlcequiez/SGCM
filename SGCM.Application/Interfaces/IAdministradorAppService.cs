using SGCM.Application.Base;
using SGCM.Application.Dtos.Seguridad_Usuarios;

namespace SGCM.Application.Interfaces
{
    public interface IAdministradorAppService : IAppService<CrearAdministradorDto, AdministradorResponseDto>
    {
    }
}
