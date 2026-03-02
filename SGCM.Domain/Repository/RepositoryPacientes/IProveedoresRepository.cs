using SGCM.Domain.Repository;

namespace SGCM.Domain.Entities.Pacientes
{
    public interface IProveedoresRepository : IBaseRepository<Proveedores>
    {
        Task<Proveedores?> ObtenerPorNombreAsync(string nombre);
        Task<Proveedores?> ObtenerPorRNCAsync(string rnc);
    }
}
