namespace SGCM.Infraestructure.Notifications
{
    public class SmsNotificationService : ISmsNotificationService
    {
        public Task EnviarAsync(string destinatario, string mensaje)
        {
            Console.WriteLine($"[SMS] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - Destinatario: {destinatario} - Mensaje: {mensaje}");
            return Task.CompletedTask;
        }
    }
}
