namespace SGCM.Application.Interfaces
{
    public interface ITokenService
    {
        string GenerarToken(int usuarioId, string email, string rol);
        int ObtenerUsuarioIdActual();
        int ObtenerUsuarioIdActual(bool permitirAnonimo);
    }
}
