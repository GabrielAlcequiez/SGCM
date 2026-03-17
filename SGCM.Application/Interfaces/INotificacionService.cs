namespace SGCM.Application.Interfaces
{
    public interface INotificacionService
    {
        Task EnviarAsync(int[] usuarioIds, string tipo, string mensaje);
    }
}
