 using SGCM.Domain.Base;

namespace SGCM.Domain.Repository
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        Task<T?> ObtenerPorIdAsync(int id);
        Task<IEnumerable<T>> ObtenerTodosAsync();
        Task AgregarAsync(T entidad);
        Task EliminarAsync(int id);
        Task ActualizarAsync(T entidad);
    }
}
