namespace SGCM.Domain.Services.Interfaces.ISeguridad_Usuarios
{
    public interface IUsuarioDomainService
    {
        Task ValidarEmailUnicoAsync(string email);
        Task PuedeEliminarUsuarioAsync(int usuarioId);
    }
}
