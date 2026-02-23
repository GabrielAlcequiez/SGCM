using SGCM.Domain.Entities.Seguridad_Usuarios;

namespace SGCM.Domain.Repository
{
    public interface IUsuarioRepository : IBaseRepository<Usuario>
    {
        Task<Usuario?> ObtenerPorEmailAsync(string email);
        Task<IEnumerable<Usuario>> ObtenerPorRolAsync(string rol);

    }
}
