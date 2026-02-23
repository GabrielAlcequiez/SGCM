namespace SGCM.Domain.Services.Interfaces.ICitas
{
    public interface ICitasDomainService
    {
        Task ValidarHorarioDisponibleAsync(int medicoId, DateTime fechaHora);

        Task ValidarDentroDeHorarioLaboralAsync(int medicoId, DateTime fechaHora);

        void ValidarTransicionEstado(int estadoActual, int nuevoEstado);
    }
}
