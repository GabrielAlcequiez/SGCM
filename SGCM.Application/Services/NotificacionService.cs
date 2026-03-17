using SGCM.Application.Interfaces;

namespace SGCM.Application.Services
{
    public class NotificacionService : INotificacionService
    {
        public Task EnviarAsync(int[] usuarioIds, string tipo, string mensaje)
        {
            foreach (var usuarioId in usuarioIds)
            {
                Console.WriteLine($"[NOTIFICACION] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - Tipo: {tipo} - UsuarioId: {usuarioId} - Mensaje: {mensaje}");
            }
            return Task.CompletedTask;
        }
    }
}
