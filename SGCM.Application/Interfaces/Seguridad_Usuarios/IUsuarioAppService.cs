using SGCM.Application.Base;
using SGCM.Application.Dtos.Seguridad_Usuarios;

namespace SGCM.Application.Interfaces.Seguridad_Usuarios
{
    public interface IUsuarioAppService : IAppService<CrearUsuarioDto, UsuarioResponseDto>
    {
        Task<IReadOnlyList<UsuarioResponseDto>> LeerPorRolAsync(string rol);
        Task CambiarPasswordAsync(int id, CambiarPasswordUsuarioDto dto);
    }
}
