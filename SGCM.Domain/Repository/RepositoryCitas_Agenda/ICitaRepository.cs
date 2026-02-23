using SGCM.Domain.Base;
using SGCM.Domain.Entities.Citas_Agenda;

namespace SGCM.Domain.Repository.Citas_Agenda
{
    public interface ICitaRepository : IBaseRepository<Citas>
    {
        Task<IEnumerable<Citas>> ObtenerPorPacienteAsync(int pacienteId);
        Task<IEnumerable<Citas>> ObtenerPorMedicoAsync(int medicoId);
        Task<IEnumerable<Citas>> ObtenerPorFechaAsync(DateTime fecha);
    }
}
