using SGCM.Domain.Entities.Medicos;

namespace SGCM.Domain.Repository.Citas_Agenda
{
    public interface IDisponibilidadRepository : IBaseRepository<Disponibilidad>
    {
        Task<IEnumerable<Disponibilidad>> ObtenerPorMedicoIdAsync(int medicoId);
        Task<Disponibilidad?> ObtenerPorMedicoYDiaAsync(int medicoId, int diaSemana);
    }
}
