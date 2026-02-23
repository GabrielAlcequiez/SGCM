namespace SGCM.Domain.Services.Interfaces.IPacientes
{
    public interface IPacienteDomainService
    {
            Task ValidarTelefonoUnicoAsync(string telefono);
            Task PuedeEliminarPacienteAsync(int pacienteId);

    }
}
