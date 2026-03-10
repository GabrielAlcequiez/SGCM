using SGCM.Application.Dtos.Pacientes;
using SGCM.Application.Interfaces;
using SGCM.Application.Logger;
using SGCM.Domain.Entities.Pacientes;
using SGCM.Domain.Exceptions;
using SGCM.Domain.Services.Interfaces.IPacientes;

namespace SGCM.Application.Services
{
    public class PacienteAppService : IPacienteAppService
    {
        private readonly IPacienteRepository _repository;
        private readonly IPacienteDomainService _domainService;
        private readonly IAuditoriaLogger _auditoriaLogger;
        public PacienteAppService(IPacienteRepository repository, IAuditoriaLogger auditoriaLogger, IPacienteDomainService domainService)
        {
            _auditoriaLogger = auditoriaLogger;
            _repository = repository;
            _domainService = domainService;
        }


        public async Task<PacienteResponseDto> CrearAsync(CrearPacienteDto dto)
        {
            int usuarioIdTemp = 1; // Temporal hasta implementar autenticación y autorización
            var paciente = new Paciente(
                dto.Nombre,
                dto.Apellido,
                dto.Telefono,
                dto.Direccion,
                dto.FechaNacimiento,
                dto.ProveedorId,
                dto.NSS,
                usuarioIdTemp
            );

            await _domainService.ValidarTelefonoUnicoAsync(dto.Telefono);
            await _repository.AgregarAsync(paciente);
            await _auditoriaLogger.RegistrarAsync(usuarioIdTemp, "Crear", "Paciente", $"Paciente creado con ID: {paciente.Id}");

            return new PacienteResponseDto
            {
                Id = paciente.Id,
                Nombre = paciente.Nombre,
                Apellido = paciente.Apellido,
                Telefono = paciente.Telefono,
                Direccion = paciente.Direccion,
                FechaNacimiento = paciente.FechaNacimiento,
                ProveedorId = paciente.ProveedorId,
                NSS = paciente.NSS
            };
        }

        public async Task<PacienteResponseDto> LeerAsync(int id)
        {
            var paciente = await _repository.ObtenerPorIdAsync(id);

            if (paciente is null)
            {
                throw new ExcepcionNoEncontrado("Paciente", id);
            }
            return new PacienteResponseDto
            {
                Id = paciente.Id,
                Nombre = paciente.Nombre,
                Apellido = paciente.Apellido,
                Telefono = paciente.Telefono,
                Direccion = paciente.Direccion,
                FechaNacimiento = paciente.FechaNacimiento,
                ProveedorId = paciente.ProveedorId,
                NSS = paciente.NSS
            };

        }

        public async Task<IReadOnlyList<PacienteResponseDto>> LeerTodosAsync()
        {
            var pacientes = await _repository.ObtenerTodosAsync();

            var listaPacientes = pacientes.Select(p => new PacienteResponseDto
            {
                Id = p.Id,
                Nombre = p.Nombre,
                Apellido = p.Apellido,
                Telefono = p.Telefono,
                Direccion = p.Direccion,
                FechaNacimiento = p.FechaNacimiento,
                ProveedorId = p.ProveedorId,
                NSS = p.NSS
            }).ToList();

            return listaPacientes;
        }

        public async Task<PacienteResponseDto> ActualizarAsync(int id, CrearPacienteDto dto)
        {
            var paciente = await _repository.ObtenerPorIdAsync(id);
            if (paciente is null)
            {
                throw new ExcepcionNoEncontrado("Paciente", id);
            }

            if(paciente.Telefono != dto.Telefono)
            {
                await _domainService.ValidarTelefonoUnicoAsync(dto.Telefono);
            }

            paciente.Actualizar(
                dto.Nombre,
                dto.Apellido,
                dto.Telefono,
                dto.Direccion,
                dto.FechaNacimiento,
                dto.ProveedorId,
                dto.NSS
            );

            await _repository.ActualizarAsync(paciente);
            int usuarioIdTemp = 1; // Temporal hasta implementar autenticación y autorización
            await _auditoriaLogger.RegistrarAsync(usuarioIdTemp, "Actualizar", "Paciente", $"Paciente actualizado con ID: {paciente.Id}");

            return new PacienteResponseDto
            {
                Id = paciente.Id,
                Nombre = paciente.Nombre,
                Apellido = paciente.Apellido,
                Telefono = paciente.Telefono,
                Direccion = paciente.Direccion,
                FechaNacimiento = paciente.FechaNacimiento,
                ProveedorId = paciente.ProveedorId,
                NSS = paciente.NSS
            };
        }

        public async Task<bool> EliminarAsync(int id)
        {
            await _domainService.PuedeEliminarPacienteAsync(id);

            var paciente = await _repository.ObtenerPorIdAsync(id);

            if (paciente is null)
            {
                throw new ExcepcionNoEncontrado("Paciente", id);
            }

            await _repository.EliminarAsync(id);
            
            int usuarioIdTemp = 1; // Temporal hasta implementar autenticación y autorización
            await _auditoriaLogger.RegistrarAsync(usuarioIdTemp, "Eliminar", "Paciente", $"Paciente eliminado con ID: {id}");
            return true;
        }




    }
}
