using SGCM.Domain.Repository.Citas_Agenda;
using SGCM.Domain.Services.Interfaces.ICitas;

namespace SGCM.Domain.Services
{
    public class MedicoDomainService : IMedicoDomainService
    {
        private readonly IMedicoRepository _medicoRepository;

        public MedicoDomainService(IMedicoRepository medicoRepository)
        {
            _medicoRepository = medicoRepository;
        }

        public Task<bool> EsExequaturUnicoAsync(string exequatur)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UsuarioYaTienePerfilMedicoAsync(int usuarioId)
        {
            throw new NotImplementedException();
        }
    }
}
