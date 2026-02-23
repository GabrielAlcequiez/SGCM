using SGCM.Domain.Entities.Pacientes;
using SGCM.Domain.Exceptions;
using SGCM.Domain.Repository;
using SGCM.Domain.Repository.Citas_Agenda;
using SGCM.Domain.Services.Interfaces.ISeguridad_Usuarios;

namespace SGCM.Domain.Services
{
    public class UsuarioDomainService : IUsuarioDomainService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IPacienteRepository _pacienteRepository;
        private readonly IMedicoRepository _medicoRepository;

        public UsuarioDomainService(
            IUsuarioRepository usuarioRepository,
            IPacienteRepository pacienteRepository,
            IMedicoRepository medicoRepository)
        {
            _usuarioRepository = usuarioRepository;
            _pacienteRepository = pacienteRepository;
            _medicoRepository = medicoRepository;
        }

        public async Task ValidarEmailUnicoAsync(string email)
        {
            var usuarioExistente = await _usuarioRepository.ObtenerPorEmailAsync(email);

            if (usuarioExistente != null)
            {
                throw new ExcepcionReglaNegocio(
                    $"El correo electrónico '{email}' ya se encuentra registrado en el sistema.",
                    "USUARIO_EMAIL_DUPLICADO"
                );
            }
        }

        public async Task PuedeEliminarUsuarioAsync(int usuarioId)
        {
            var usuario = await _usuarioRepository.ObtenerPorIdAsync(usuarioId);
            if (usuario == null)
            {
                throw new ExcepcionNoEncontrado("Usuario", usuarioId);
            }

            var pacienteVinculado = await _pacienteRepository.ObtenerPorIdAsync(usuarioId);
            if (pacienteVinculado != null)
            {
                throw new ExcepcionReglaNegocio(
                    "No se puede eliminar este usuario porque es la cuenta de acceso de un paciente registrado.",
                    "USUARIO_PERTENECE_PACIENTE"
                );
            }

            var medicoVinculado = await _medicoRepository.ObtenerPorUsuarioIdAsync(usuarioId);
            if (medicoVinculado != null)
            {
                throw new ExcepcionReglaNegocio(
                    "No se puede eliminar este usuario porque es la cuenta de acceso de un médico registrado.",
                    "USUARIO_PERTENECE_DOCTOR"
                );
            }
        }
    }
}
