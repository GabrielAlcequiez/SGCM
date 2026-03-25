namespace SGCM.Infraestructure.Notifications
{
    public class EmailNotificationService : IEmailNotificationService
    {
        public Task EnviarAsync(string destinatario, string mensaje)
        {
            Console.WriteLine($"[EMAIL] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - Destinatario: {destinatario} - Mensaje: {mensaje}");
            return Task.CompletedTask;
        }
    }
}
