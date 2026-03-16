using SGCM.Application.Dtos.Seguridad_Usuarios;

namespace SGCM.Application.Interfaces.Seguridad_Usuarios
{
    public interface IUsuarioAppService
    {
        Task<UsuarioResponseDto> CrearAsync(CrearUsuarioDto dto);
        Task<UsuarioResponseDto> ActualizarAsync(int id, ActualizarUsuarioDto dto);
        Task<UsuarioResponseDto> LeerAsync(int id);
        Task<IReadOnlyList<UsuarioResponseDto>> LeerTodosAsync();
        Task<bool> EliminarAsync(int id);
        Task<IReadOnlyList<UsuarioResponseDto>> LeerPorRolAsync(string rol);
        Task CambiarPasswordAsync(int id, CambiarPasswordUsuarioDto dto);
    }
}
