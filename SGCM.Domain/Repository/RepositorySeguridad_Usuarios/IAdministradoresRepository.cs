
using SGCM.Domain.Entities.Seguridad_Usuarios;

namespace SGCM.Domain.Repository
{
    public interface IAdministradoresRepository : IBaseRepository<Administradores>
    {
        Task<Administradores?> ObtenerPorUsuarioIdAsync(int usuarioId);
    }
}
