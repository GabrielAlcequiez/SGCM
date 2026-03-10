namespace SGCM.Domain.Services.Interfaces.ICitas
{
    public interface IMedicoDomainService
    {
        Task EsExequaturUnicoAsync(string exequatur);
        Task UsuarioYaTienePerfilMedicoAsync(int usuarioId); 

        Task PuedeEliminarMedicoAsync(int medicoId);

    }
}
