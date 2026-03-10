using SGCM.Domain.Exceptions;
using SGCM.Domain.Repository.Citas_Agenda;
using SGCM.Domain.Services.Interfaces.ICitas;
using System.Data;

namespace SGCM.Domain.Services
{
    public class MedicoDomainService : IMedicoDomainService
    {
        private readonly IMedicoRepository _medicoRepository;
        private readonly ICitaRepository _citaRepository;

        public MedicoDomainService(IMedicoRepository medicoRepository, ICitaRepository citaRepository)
        {
            _medicoRepository = medicoRepository;
            _citaRepository = citaRepository;
        }

        public async Task EsExequaturUnicoAsync(string exequatur)
        {
            var medicoExistente = _medicoRepository.ObtenerPorExequaturAsync(exequatur);
            if (medicoExistente is not null)
                throw new ExcepcionReglaNegocio($"Ya existe un médico registrado con el exequatur: {exequatur}", "EXEQUATUR_YA_EXISTE");
        }

        public async Task PuedeEliminarMedicoAsync(int medicoId)
        {
            var medico = await _medicoRepository.ObtenerPorIdAsync(medicoId);
            if (medico is null)
                throw new ExcepcionNoEncontrado("Medico", medicoId);

            var citasMedico = await _citaRepository.ObtenerPorMedicoAsync(medicoId);
            bool tieneCitasActivas = citasMedico.Any(c => c.Estado == 1 || c.Estado == 2);

            if (tieneCitasActivas)    
                throw new ExcepcionReglaNegocio($"No se puede eliminar el médico con ID {medicoId} porque tiene citas activas.", "MEDICO_TIENE_CITAS_ACTIVAS")
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
