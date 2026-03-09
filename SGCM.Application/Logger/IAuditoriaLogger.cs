namespace SGCM.Application.Logger
{
    public interface IAuditoriaLogger
    {
        Task RegistrarAsync(int usuarioId, string accion, string entidadAfectada, string detalles);
    }
}
