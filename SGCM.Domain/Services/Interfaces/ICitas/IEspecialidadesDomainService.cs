namespace SGCM.Domain.Services.Interfaces.ICitas
{
    public interface IEspecialidadDomainService
    {
        Task EsNombreUnicoAsync(string nombre);
        Task PuedeEliminarEspecialidadAsync(int id);
    }
}
