namespace SGCM.Domain.Services.Interfaces.ISeguridad_Usuarios
{
    public interface IAuditoriaLogsDomainService
    {
        bool EsAccionCritica(string accion);
    }
}
