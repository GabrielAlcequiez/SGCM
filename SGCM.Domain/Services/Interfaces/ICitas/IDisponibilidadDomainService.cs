namespace SGCM.Domain.Services.Interfaces.ICitas
{
    public interface IDisponibilidadDomainService
    {
        Task ValidarDisponibilidadUnicaPorDiaAsync(int medicoId, int diaSemana);
        string ObtenerNombreDia(int diaSemana);
        void ValidarConsistenciaHorario(TimeSpan inicio, TimeSpan fin, bool esDiaLibre);
        Task PuedeEliminarCitaAsync(int disponibilidadId);
    }
}
