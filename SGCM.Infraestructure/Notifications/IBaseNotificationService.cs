namespace SGCM.Infraestructure.Notifications
{
    public interface IBaseNotificationService
    {
        Task EnviarAsync(string destinatario, string mensaje);
    }
}
