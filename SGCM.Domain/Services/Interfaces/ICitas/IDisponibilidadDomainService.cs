namespace SGCM.Domain.Services.Interfaces.ICitas
{
    public interface IDisponibilidadDomainService
    {
        Task ValidarDisponibilidadUnicaPorDiaAsync(int medicoId, int diaSemana);
    }
}
