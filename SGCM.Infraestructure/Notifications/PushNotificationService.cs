namespace SGCM.Infraestructure.Notifications
{
    public class PushNotificationService : IPushNotificationService
    {
        public Task EnviarAsync(string destinatario, string mensaje)
        {
            Console.WriteLine($"[PUSH] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - Destinatario: {destinatario} - Mensaje: {mensaje}");
            return Task.CompletedTask;
        }
    }
}
