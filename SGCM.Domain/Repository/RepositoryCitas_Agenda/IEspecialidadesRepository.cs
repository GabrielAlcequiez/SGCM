using SGCM.Domain.Entities.Medicos;

namespace SGCM.Domain.Repository.Citas_Agenda
{
    public interface IEspecialidadesRepository : IBaseRepository<Especialidades>
    {
        Task<Especialidades?> ObtenerPorNombreAsync(string nombre);
    }
}
