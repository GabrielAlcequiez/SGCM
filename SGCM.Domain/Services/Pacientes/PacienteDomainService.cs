using SGCM.Domain.Entities.Pacientes;
using SGCM.Domain.Exceptions;
using SGCM.Domain.Repository.Citas_Agenda;
using SGCM.Domain.Services.Interfaces.IPacientes;

namespace SGCM.Domain.Services
{
    public class PacienteDomainService : IPacienteDomainService
    {
        private readonly IPacienteRepository _pacienteRepository;
        private readonly ICitaRepository _citaRepository;

        public PacienteDomainService(IPacienteRepository pacienteRepository, ICitaRepository citaRepository)
        {
            _pacienteRepository = pacienteRepository;
            _citaRepository = citaRepository;
        }

        public async Task ValidarTelefonoUnicoAsync(string telefono)
        {
            var pacientesConEseTelefono = await _pacienteRepository.ObtenerPorTelefonoAsync(telefono);

            if (pacientesConEseTelefono.Any())
            {
                throw new ExcepcionReglaNegocio(
                    $"El número de teléfono '{telefono}' ya está vinculado a otro paciente o ya está registrado.",
                    "NUMERO_TELEFONICO_DUPLICADO"
                );
            }
        }

        public async Task PuedeEliminarPacienteAsync(int pacienteId)
        {
            var paciente = await _pacienteRepository.ObtenerPorIdAsync(pacienteId);
            if (paciente == null)
            {
                throw new ExcepcionNoEncontrado("Paciente", pacienteId);
            }
            var citasDelPaciente = await _citaRepository.ObtenerPorPacienteAsync(pacienteId);

            bool tieneCitasActivas = citasDelPaciente.Any(c => c.Estado == 1 || c.Estado == 2);

            if (tieneCitasActivas)
            {
                throw new ExcepcionReglaNegocio(
                    "No se puede eliminar el paciente porque tiene citas registradas en el sistema. Considere desactivarlo.",
                    "PACIENTE_TIENE_HISTORIAL_ACTIVO"
                );
            }
        }
    }
}
