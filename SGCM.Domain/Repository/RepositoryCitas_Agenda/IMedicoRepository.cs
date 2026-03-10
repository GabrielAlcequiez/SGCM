using SGCM.Domain.Entities.Medicos;

namespace SGCM.Domain.Repository.Citas_Agenda
{
    public interface IMedicoRepository : IBaseRepository<Medico>
    {
        Task<Medico?> ObtenerPorExequaturAsync(string exequatur);

        Task<Medico?> ObtenerPorUsuarioIdAsync(int usuarioId);

        Task<IEnumerable<Medico>> ObtenerPorEspecialidadAsync(int especialidadId);

        Task<bool> ExisteMedicoConEspecialidadAsync(int especialidadId);
    }
}
