namespace SGCM.Application.Interfaces
{
    public interface IBaseNotificationService
    {
        Task EnviarAsync(string destinatario, string mensaje);
    }
}
