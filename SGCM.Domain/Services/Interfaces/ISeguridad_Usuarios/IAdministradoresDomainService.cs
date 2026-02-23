namespace SGCM.Domain.Services.Interfaces.ISeguridad_Usuarios
{
    public interface IAdministradoresDomainService
    {
        Task PuedeCrearPerfilAdminAsync(int usuarioId);
    }
}
