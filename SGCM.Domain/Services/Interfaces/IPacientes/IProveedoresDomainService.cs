namespace SGCM.Domain.Services.Interfaces.IPacientes
{
    public interface IProveedoresDomainService
    {
        Task EsRNCUnicoAsync(string rnc);
        Task ValidarNombreUnicoAsync(string nombre);
    }
}
