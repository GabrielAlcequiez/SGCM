using SGCM.Domain.Exceptions;
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

        public async Task EsExequaturUnicoAsync(string exequatur)
        {
            var medicoExistente = _medicoRepository.ObtenerPorExequaturAsync(exequatur);
            if (medicoExistente is not null)
                throw new ExcepcionReglaNegocio($"Ya existe un médico registrado con el exequatur: {exequatur}", "EXEQUATUR_YA_EXISTE");
        }

        public async Task UsuarioYaTienePerfilMedicoAsync(int usuarioId)
        {
            var medicoExistente = await _medicoRepository.ObtenerPorUsuarioIdAsync(usuarioId);

            if (medicoExistente != null)
            {
                throw new ExcepcionReglaNegocio($"El usuario con ID {usuarioId} ya tiene un perfil de médico asignado.", "MEDICO_EXISTE");
            }
        }
    }
}
