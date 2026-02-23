namespace SGCM.Domain.Services.Interfaces.ICitas
{
    public interface IMedicoDomainService
    {
        Task<bool> EsExequaturUnicoAsync(string exequatur);
        Task<bool> UsuarioYaTienePerfilMedicoAsync(int usuarioId); 
    }
}
